using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class GameManagerStart : MonoBehaviour {
	public static GameManagerStart instance;
	public string playerName;
	private GameObject pauseMenu;
	public GameObject endScreen;
	public static bool win;
	private static bool start;
	private int lives;


	void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
		DontDestroyOnLoad(gameObject);
		start = false;
		win = false;
		//playerName = EditorPrefs.GetString("PlayerName");
		playerName = PlayerPrefs.GetString("PlayerName");

		lives = 3;
	}

	void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}
	

	private void OnSceneLoaded(Scene scene, LoadSceneMode loadsceneMode)
	{
		if (scene.name == "Game")
			Debug.Log("");
			//DisplayPreviousTimes();
	}


	public void RestartLevel(float delay)
	{
		StartCoroutine(RestartLevelDelay(delay));
	}

	private IEnumerator RestartLevelDelay(float delay)
	{
		yield return new WaitForSeconds(delay);
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public List<PlayerTimeEntry> LoadPreviousTimes()
	{
        //refer to note 1
        var scoresFile = Application.persistentDataPath +
              "/2b" + "_times.dat";
        try
		{
			using (var stream = File.Open(scoresFile, FileMode.Open))
			{
				var bin = new BinaryFormatter();
				var times = (List<PlayerTimeEntry>)bin.Deserialize(stream);
				return times;
			}
		}
		//refer to note 2
		catch (IOException ex)
		{
			Debug.LogWarning("Couldn't load previous times for: " +
			  playerName + ".Exception: " + ex.Message);
			return new List<PlayerTimeEntry>();
		}
	}

	public void SaveTime(decimal time)
	{
		//refer to note 3
		var times = LoadPreviousTimes();
		//refer to note 4
		var newTime = new PlayerTimeEntry();
		newTime.entryDate = DateTime.Now;
		newTime.time = time;
		newTime.PlayerName = playerName;
		newTime.Lives = lives;
		//refer to note 5
		var bFormatter = new BinaryFormatter();
		var filePath = Application.persistentDataPath +
		  "/2b" + "_times.dat";
		using (var file = File.Open(filePath, FileMode.Create))
		{
			times.Add(newTime);
			bFormatter.Serialize(file, times);
		}
	}

	public void DisplayPreviousTimes()
	{
		//Note 1
		var times = LoadPreviousTimes();
		var topTen = times.OrderBy(Lives => -Lives.Lives).ThenBy(Time =>Time.time).Take(10);
		//Note 2
		var timesLabel = GameObject.Find("HighScoresText").GetComponent<Text>();
		//Note 3
		int i = 1;
		string temp = "";
		foreach (var time in topTen)
		{
			temp += i +  ") "+ time.PlayerName + "   Lives: " + time.Lives + "   " + time.entryDate.ToShortDateString() +
			  ": " + time.time + "\n";
			i++;
		}
		timesLabel.text = temp;
	}

	public void TogglePauseMenu()
	{
		if (!pauseMenu.activeSelf && GameObject.Find("InstructionsPanel")==null)
		{
			Time.timeScale = 0;
			pauseMenu.SetActive(true);
		}
		else if (pauseMenu.activeSelf)
		{
			Time.timeScale = 1;
			pauseMenu.SetActive(false);
		}
	}

	private void gameStart()
	{
		Time.timeScale = 0;

		Button pause = GameObject.Find("ResumeButton").GetComponent<Button>();
		pause.GetComponent<Button>().onClick.AddListener(() => {
			GameManagerStart.instance.TogglePauseMenu();
		});

		pauseMenu = GameObject.Find("PausePanel");
		pauseMenu.SetActive(false);

		endScreen = GameObject.Find("EndgamePanel");
		endScreen.SetActive(false);

		start = true;
		Time.timeScale = 1;

	}

	private void menuStart()
	{
		GameObject.Find("HighScoresButton").GetComponent<Button>().onClick.AddListener(() => {
			GameManagerStart.instance.DisplayPreviousTimes();
		});
		start = true;
	}

	private void Update()
	{
		if (SceneManager.GetActiveScene().name.Equals("Game"))
			if (Input.GetKeyDown(KeyCode.M) && !win)
				TogglePauseMenu();

		if (!start)
		{
			if (SceneManager.GetActiveScene().name.Equals("Menu"))
				menuStart();
			else if (SceneManager.GetActiveScene().name.Equals("Game"))
				gameStart();
		}
	}


}
