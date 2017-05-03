using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Player : MovingObject
{
	//delay when loading the next level
	public float restartLevelDelay = 1f;
	//Base damage of unit
	public int basedamage = 2;
	//Variable used to store the health of player
	private int health;
	//Level of the player
	public int playerLevel = GameManager.instance.playerLevel;

	protected override void Start()
	{
		//Initiates the players health and adds to it when a new level loads
		health = GameManager.instance.playerHP;
		health += 5*playerLevel ;
		GameManager.instance.playerHP = health;
		base.Start();
	}

	private void OnDisable()
	{
		//If the Game Object is disabled (when new dungeon loaded) return players current health
		GameManager.instance.playerHP = health;
	}
	private void Update ()
	{
		//Checks if the player can move
		bool turn = GameManager.instance.playersTurn;
		if (turn != true) return;
		int horizontal = 0;
		int vertical = 0;
		//Default inputs for unity are WASD or the arrow keys
		horizontal = (int)(Input.GetAxisRaw ("Horizontal"));
		vertical = (int)(Input.GetAxisRaw ("Vertical"));
		//Player cannot move diagonally
		if (horizontal != 0) 
		{
			vertical = 0;
		}
		//Move the player
		if (horizontal != 0 || vertical != 0) 
		{
			AttemptMove<Enemy>(horizontal,vertical);
		}
	}

	protected override void AttemptMove <T> (int xDir, int yDir)

	{
		//Attempt to move
		base.AttemptMove<T> (xDir, yDir);
		//See if hit
		RaycastHit2D hit;
		if (Move (xDir,yDir,out hit))
		{
			//Put possible sound effects here (OPTIONAL)
		}
		//Checks if the game is over (player hp = 0)
		CheckifGameOver ();
		//Ends players movement
		GameManager.instance.playersTurn = false;
	}
	protected override void OnCantMove <T> (T component)
	{
		//If the player cannot move and hit the enemy, damage the enemy
		Enemy hitEnemy = component as Enemy;
		hitEnemy.LoseHealth (basedamage);
	}
	private void OnTriggerEnter2D (Collider2D other)
	{
		//check if collides with the exit
		if (other.tag == "Exit") {
			Invoke ("Restart", restartLevelDelay);
			enabled = false;
		}
		//check if collides with trap (NOT IMPLEMENTED)
		else if (other.tag == "Trap") {
		}

	}
	private void Restart()
	{
		//If reach exit, create new dungeon etc.
		SceneManager.LoadScene(3, LoadSceneMode.Single);
	}
	public int DamageEnemy()
	{
		//not used but possibly used in future to have scailing damage
		return basedamage;
	}
	public void LoseHealth(int loss)
	{
		//If player is damaged, lose health and check if players health is equal to 0
		health -= loss;
		GameManager.instance.playerHP = health;
		CheckifGameOver ();
	}
	private void CheckifGameOver()
	{
		//If health is less than or equal to 0, game over
		if (health <= 0) 
		{
			GameManager.instance.GameOver ();
		}
	}
}