//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class Player : MovingObject {
//	public int Level = 1;
//	public int damage = 2 + Mathf.Floor((0.6*Level));
//	public float restartLevelDelay = 1f;
//
//	private int score;
//	private int health;
//	protected override void Start ()
//	{
//		health = 10;
//		base.Start();
//	}
//	private void OnDisable()
//	{
//		
//	}
//	
//	// Update is called once per frame
//	void Update () 
//	{
//		int horizontal = 0;
//		int vertical = 0;
//		horizontal = (int)Input.GetAxisRaw ("Horizontal");
//		vertical = (int)Input.GetAxisRaw ("Vertical");
//		if (horizontal != 0)
//			vertical = 0;
//		if (horizontal != 0 || vertical != 0)
//			
//	}
//	protected override void AttemptMove<T>(int xDir, int yDir)
//	{
//		base.AttemptMove <T> (xDir, yDir);
//		RaycastHit2D hit;
//		GameManager.instance.PlayersTurn = false;
//	}
//	private void CheckIfGameOver()
//	{
//		if (health <= 0) 
//		{
//			GameManager.instance.GameOver ();
//		}
//	}
//}
