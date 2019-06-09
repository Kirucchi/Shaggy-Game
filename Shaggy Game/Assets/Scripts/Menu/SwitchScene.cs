using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour {


	public void LoadScene(string sceneName)
	{
        Time.timeScale = 1;
		SceneManager.LoadScene(sceneName);
	}
	/*
	public void LoadOnDelay(int delay)
	{
		StartCoroutine(RestartLevelDelay(delay));
	}

	private IEnumerator RestartLevelDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		if(SceneManager.GetActiveScene().name.Equals("Game"))
			SceneManager.LoadScene("Menu");
		else if (SceneManager.GetActiveScene().name.Equals("Menu"))
			SceneManager.LoadScene("Game");

	}
	*/
}
