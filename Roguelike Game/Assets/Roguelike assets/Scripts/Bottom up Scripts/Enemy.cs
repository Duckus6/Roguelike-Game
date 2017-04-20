using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MovingObject {

	public IntRange Damage = new IntRange(1,3);
	public int health = 3;
	private Transform target;
	// Use this for initialization
	protected override void Start () 
	{
		GameManager.instance.AddEnemiesToList (this);
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		base.Start ();
	}
	protected override void AttemptMove <T> (int xDir, int yDir)
	{
		base.AttemptMove <T> (xDir, yDir);
	}
	public void MoveEnemy()
	{
		int xDir = 0;
		int yDir = 0;
		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon)
			yDir = target.position.y > transform.position.y ? 1 : -1;
		else
			xDir = target.position.x > transform.position.x ? 1 : -1;
		AttemptMove <Player> (xDir, yDir);
	}
	protected override void OnCantMove <T> (T component)
	{
		Player hitPlayer = component as Player;
		hitPlayer.LoseHealth (Damage.Random);
	}
}
