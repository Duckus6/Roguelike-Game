﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExtrasLoadScene : MonoBehaviour {
	public void Load () {
		// Only specifying the sceneName or sceneBuildIndex will load the scene with the Single mode
		SceneManager.LoadScene (1, LoadSceneMode.Single);
	}
}