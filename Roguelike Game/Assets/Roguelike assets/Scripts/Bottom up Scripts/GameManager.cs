using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
	//Delays used so things dont load instantly (looks off when they do)
	public float TurnDelay = .1f;
	public float levelStartDelay = 2f;
	public static GameManager instance = null;
	//Base stats
	public int playerHP = 10;
	public int Level = 0;
	public int playerLevel = 1;
	public float playerEXP = 0f;
	//Text objects
	public Text healthText;
	public GameObject EndGame;
	private Text levelText;
	//Used for movement
	[HideInInspector] public bool playersTurn = true;

	private BoardCreator Boardscript;
	//List of all enemies
	public List <Enemy> enemies;
	//Setup bools
	private bool enemiesMoving;
	private bool doingSetup;
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
		//instantiate board and creates the enemies list
		Boardscript = GetComponent<BoardCreator> ();
		enemies = new List<Enemy>();
	}
	//This is called each time a scene is loaded.
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		//Add one to level number.
		//Call InitGame to initialize our level.
		InitGame();
		Level++;
	}
	void OnEnable()
	{
		//Tells ‘OnLevelFinishedLoading’ function to start listening for a scene change event as soon as this script is enabled.
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}
	void OnDisable()
	{
		//Tells ‘OnLevelFinishedLoading’ function to stop listening for a scene change event as soon as this script is disabled.
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}
	void InitGame()
	{
		//Setup for game
		doingSetup = true;
		levelText = GameObject.Find ("Level Text").GetComponent<Text> ();
		levelText.text = "Level:" + Level.ToString();
		enemies.Clear();
		Boardscript.Setup (Level);
		//Done in this way to have a start delay
		Invoke ("setup", levelStartDelay);
	}
	//function ends setup
	void setup()
	{
		doingSetup = false;
	}
	void Update()
	{
		//If enemies cannot move or are already moving run the remaining script
		if (playersTurn || enemiesMoving|| doingSetup)
			return;
		StartCoroutine (MoveEnemies ());
		//Done as object == null if its a start of a new (not first) level
		if (healthText == null) 
		{
			healthText = GameObject.Find ("Health Text").GetComponent<Text> ();
		}
		healthText.text = "Health:" + playerHP;
		if (playerEXP >= 1*playerLevel)
		{
			playerLevel += 1;
			playerEXP -= 1*playerLevel;
		}
	}
	//adds all enemies to the list
	public void AddEnemiesToList (Enemy script)
	{
		enemies.Add (script);
	}
	//Clears the board and returns to the main menu, removing all game objects from the current session
	public void GameOver()
	{
		enemies.Clear ();
		enabled = false;
		//Used for same reason as healthText == null
		if (EndGame == null)
			EndGame = GameObject.Find ("EndGame");
		EndGame.gameObject.SetActive (true);
		healthText.text = "Health:0";
		//Used so Game Over text is displayed for more than a fraction of a second
		Invoke("loadscene",3);


	}
	//loads main menu and destroys GameManager
	void loadscene ()
	{
		SceneManager.LoadScene (0, LoadSceneMode.Single);
		Destroy(gameObject);
	}
	//Moves enemies and then makes it the players turn
	IEnumerator MoveEnemies()
	{
		enemiesMoving = true;
		yield return new WaitForSeconds (TurnDelay);
		for (int i = 0; i < enemies.Count; i++) 
		{
			enemies [i].MoveEnemy();
			yield return new WaitForSeconds (enemies[i].moveTime);
		}
		playersTurn = true;
		enemiesMoving = false;
	}
}