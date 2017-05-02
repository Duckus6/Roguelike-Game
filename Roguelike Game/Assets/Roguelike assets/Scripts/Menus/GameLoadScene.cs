using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameLoadScene : MonoBehaviour {
	public void Load () {
		//Loads the main game scene
		SceneManager.LoadScene (3, LoadSceneMode.Single);
	}
}
