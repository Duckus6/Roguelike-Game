using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMain : MonoBehaviour {
	public void Load () {
		//Loads the Main Menu scene
		SceneManager.LoadScene (0, LoadSceneMode.Single);

	}
}