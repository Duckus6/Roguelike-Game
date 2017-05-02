using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class OptionsLoadScene : MonoBehaviour {
	public void Load () {
		//Loads the Options scene
		SceneManager.LoadScene (2, LoadSceneMode.Single);
	}
}
