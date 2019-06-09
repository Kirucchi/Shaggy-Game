using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Goal : MonoBehaviour
{
	public void win()
	{
		/*
		var timer = FindObjectOfType<Timer>();
		GameManager.instance.endScreen.SetActive(true);
		var timer = FindObjectOfType<Timer>();
		GameObject.Find("ScoreText").GetComponent<Text>().text = "Your time is: " + timer.time;
		timer.Pause();
		GameManager.instance.SaveTime(timer.time);
		*/

		var timer = FindObjectOfType<Timer>();
        GameObject.Find("Timer").GetComponent<Timer>().enabled = false;//.SetActive(false);
		GameManagerStart.instance.endScreen.SetActive(true);
		GameObject.Find("ScoreText").GetComponent<Text>().text = "Your time is: " + timer.time;
		GameManagerStart.instance.SaveTime(timer.time);
		Time.timeScale = 0;//freezes background
		GameManagerStart.win = true;

	}
}
