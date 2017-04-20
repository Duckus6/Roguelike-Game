using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
	public float TurnDelay = .1f;
	public float levelStartDelay = 2f;
	public static GameManager instance = null;
	public int playerHP = 10;
	public int Level = 1;
	public int playerLevel = 1;
	[HideInInspector] public bool playersTurn = true;

	private Text levelText;
	private BoardCreator Boardscript;
	private List <Enemy> enemies;
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
		InitGame ();
	}
	private void OnLevelWasLoaded(int index)
	{
		Level++;
		InitGame ();
	}

	void InitGame()
	{
		doingSetup = true;
		levelText = GameObject.Find ("Level Text").GetComponent<Text> ();
		levelText.text = "Level:" + Level.ToString();
		enemies.Clear();
		Boardscript.Setup (Level);
		doingSetup = false;
	}
	void Update()
	{
		if (playersTurn || enemiesMoving)
			return;
		StartCoroutine (MoveEnemies ());
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