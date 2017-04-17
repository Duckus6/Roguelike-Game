using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;
	public int playerHP = 10;
	public int Level = 1;
	public int playerLevel = 1;
	[HideInInspector] public bool playersTurn = true;
	private BoardCreator Boardscript;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
		DontDestroyOnLoad (gameObject);
		 Boardscript = GetComponent<BoardCreator> ();
		Boardscript.Setup (Level);
	}
	public void GameOver()
	{
		enabled = false;
	}
}