using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadLevels : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadStartMenu() {
		SceneManager.LoadScene ("StartMenu");
	}

	public void LoadGameLevel() {

		SceneManager.LoadScene ("ComposersBlock");
	}
}
