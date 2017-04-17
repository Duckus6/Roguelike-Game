﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {

	public IntRange Damage = new IntRange(1,3);
	private Transform target;
	// Use this for initialization
	protected override void Start () 
	{
		target = GameObject.FindGameObjectWithTag ("Player").transform;
	}
	
	// Update is called once per frame
	void Update () 
	{
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
