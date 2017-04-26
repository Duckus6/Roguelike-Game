using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MovingObject {

	public IntRange Damage = new IntRange(1,3);
	public int health = 3;
	private Transform target;
	private bool canAttack;
	// Use this for initialization
	protected override void Start () 
	{
		canAttack = true;
		health += 2 * GameManager.instance.Level;
		GameManager.instance.AddEnemiesToList (this);
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}
	public void LoseHealth(int loss)
	{
		health -= loss;
		if (health <= 0) 
		{
			GameManager.instance.playerEXP += 0.3f;
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
		if (!(Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon))
			xDir = target.position.x > transform.position.x ? 1 : -1;
		if (!(Mathf.Abs(target.position.y - transform.position.y) <float.Epsilon))
			yDir = target.position.y > transform.position.y ? 1 : -1;
		AttemptMove <Player> (xDir, yDir);
	}
	protected override void OnCantMove <T> (T component)
	{
		Player hitPlayer = component as Player;
		if (canAttack)
			hitPlayer.LoseHealth ((Damage.Random)+GameManager.instance.Level);
		else
			hitPlayer.LoseHealth (0);
	}
}
