using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour 
{
	public Transform canvas;

	void Update ()
	{	//If the escape key is pressed, pause/unpause the game
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Pause ();
		}
	}

	public void Pause()
	{
		if (canvas.gameObject.activeInHierarchy == false) 
			{	//Pause the game if the pause menu is not active
				canvas.gameObject.SetActive (true);
				Time.timeScale = 0;

			}else 
			{//Unpause the game if the pause menu is active
				canvas.gameObject.SetActive (false);
				Time.timeScale = 1;
			}
	}
}