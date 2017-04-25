using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
	public float TurnDelay = .1f;
	public float levelStartDelay = 2f;
	public static GameManager instance = null;
	public int playerHP = 10;
	public int Level = 0;
	public int playerLevel = 1;
	public float playerEXP = 0f;
	public Text healthText;
	[HideInInspector] public bool playersTurn = true;

	private Text levelText;
	private BoardCreator Boardscript;
	public List <Enemy> enemies;
	private bool enemiesMoving;
	private bool doingSetup;
	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
		Boardscript = GetComponent<BoardCreator> ();
		enemies = new List<Enemy>();
	}
	//This is called each time a scene is loaded.
	void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		//Add one to our level number.
		//Call InitGame to initialize our level.
		InitGame();
		Level++;
	}
	void OnEnable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function tostart listening for a scene change event as soon as this script is enabled.
			SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}
	void OnDisable()
	{
		//Tell our ‘OnLevelFinishedLoading’ function to stop listening for a scene change event as soon as this script is disabled.
			//Remember to always have an unsubscription for every delegate you subscribe to!
			SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}
	void destroyedenemy()
	{
		
	}
	void InitGame()
	{
		doingSetup = true;
		levelText = GameObject.Find ("Level Text").GetComponent<Text> ();
		levelText.text = "Level:" + Level.ToString();
		enemies.Clear();
		Boardscript.Setup (Level);
		Invoke ("setup", levelStartDelay);
	}
	void setup()
	{
		doingSetup = false;
	}
	void Update()
	{
		if (playersTurn || enemiesMoving|| doingSetup)
			return;
		StartCoroutine (MoveEnemies ());
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
	public void AddEnemiesToList (Enemy script)
	{
		enemies.Add (script);
	}
	public void GameOver()
	{
		enemies.Clear ();
		enabled = false;
	}
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