using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Player : MovingObject
{
	public float restartLevelDelay = 1f;
	public int basedamage = 2;
	private int health;
	public int playerLevel = GameManager.instance.playerLevel;

	protected override void Start()
	{
		health = GameManager.instance.playerHP;
		health += 5*playerLevel ;
		GameManager.instance.playerHP = health;
		base.Start();
	}

	private void OnDisable()
	{
		GameManager.instance.playerHP = health;
	}
	private void Update ()
	{
		bool turn = GameManager.instance.playersTurn;
		if (turn != true) return;
		int horizontal = 0;
		int vertical = 0;

		horizontal = (int)(Input.GetAxisRaw ("Horizontal"));
		vertical = (int)(Input.GetAxisRaw ("Vertical"));
		if (horizontal != 0) 
		{
			vertical = 0;
		}
		if (horizontal != 0 || vertical != 0) 
		{
			AttemptMove<Enemy>(horizontal,vertical);
		}
	}

	protected override void AttemptMove <T> (int xDir, int yDir)

	{
		base.AttemptMove<T> (xDir, yDir);
		RaycastHit2D hit;
		if (Move (xDir,yDir,out hit))
		{
			//possible sound effects
		}
		CheckifGameOver ();
		GameManager.instance.playersTurn = false;
	}
	protected override void OnCantMove <T> (T component)
	{
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
		//check if collides with trap
		else if (other.tag == "Trap") {
		}

	}
	private void Restart()
	{
		SceneManager.LoadScene(3, LoadSceneMode.Single);
	}
	public int DamageEnemy()
	{
		return basedamage;
	}
	public void LoseHealth(int loss)
	{
		health -= loss;
		GameManager.instance.playerHP = health;
		CheckifGameOver ();
	}
	private void CheckifGameOver()
	{
		if (health <= 0) 
		{
			GameManager.instance.GameOver ();
		}
	}
}