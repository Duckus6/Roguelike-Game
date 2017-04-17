//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//public class GameManager : MonoBehaviour
//{
//	public static GameManager instance = null;
//	private DungeonCreator boardScript;
//	void Awake()
//	{
//		if (instance == null)
//			instance = this;
//		else if (instance != this)
//			Destroy (gameObject);
//		DontDestroyOnLoad (gameObject);
//		boardScript = GetComponent<DungeonCreator> ();
//		InitGame ();
//
//	}
//	void InitGame()
//	{
//
//	}
//}