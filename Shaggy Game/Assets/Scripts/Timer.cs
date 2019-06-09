using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	private Text timerText;
	public decimal time;
	private float pause;
	private bool paused;

	void Awake()
	{
		timerText = GetComponent<Text>();
	}

	void Update()
	{
		if (paused)
			pause += Time.deltaTime;
		time = System.Math.Round((decimal)(Time.timeSinceLevelLoad - pause), 2);
		timerText.text = time.ToString();

	}

	public void Pause()
	{
		paused = true;
	}

	public void Unpause()
	{
		paused = false;
	}

}