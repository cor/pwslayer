using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

	public void loadGame(){
		// Application.LoadLevel("Level_04");
		SceneManager.LoadScene("Level_04");
	}
}
