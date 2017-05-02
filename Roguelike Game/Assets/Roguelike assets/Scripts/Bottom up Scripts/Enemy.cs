using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MovingObject {
	//Base damage
	public IntRange Damage = new IntRange(1,3);
	//Base health
	public int health = 3;
	//Used to store player pos
	private Transform target;
	//Used for when the enemy dies
	private bool canAttack;
	protected override void Start () 
	{
		canAttack = true;
		//scailing health
		health += 2 * GameManager.instance.Level;
		//Adds enemy to list
		GameManager.instance.AddEnemiesToList (this);
		//Sets target to the player
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}
	//Function called when enemy loses health
	public void LoseHealth(int loss)
	{
		health -= loss;
		if (health <= 0) 
		{
			GameManager.instance.playerEXP += 0.3f;
			//Used so player can still move but enemy is not seen
			canAttack = false;
			gameObject.SetActive(false);
		}
	}
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		base.AttemptMove <T> (xDir, yDir);
	}
	public void MoveEnemy()
	{
		int xDir = 0;
		int yDir = 0;
		//allows enemy to move diagonally so they can catch up with the player
		if (!(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon))
			xDir = target.position.x > transform.position.x ? 1 : -1;
		if (!(Mathf.Abs(target.position.y - transform.position.y) <float.Epsilon))
			yDir = target.position.y > transform.position.y ? 1 : -1;
		AttemptMove <Player> (xDir, yDir);
	}
	protected override void OnCantMove <T> (T component)
	{
		//damage set to 0 if enemy is defeated
		Player hitPlayer = component as Player;
		if (canAttack)
			hitPlayer.LoseHealth ((Damage.Random)+GameManager.instance.Level);
		else
			hitPlayer.LoseHealth (0);
	}
}
