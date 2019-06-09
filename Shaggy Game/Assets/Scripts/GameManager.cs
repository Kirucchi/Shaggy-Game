using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class GameManager : MonoBehaviour {

    public GameObject shaggy1;
    public GameObject shaggy2;
    public SpriteRenderer shaggy2renderer;
    public Sprite shaggy3;
    public Transform Position1;
    public Transform Position2;
    public Transform Position3;
    private int phase = 0;

    public GameObject ship;
    public GameObject explosion;
    public AudioClip explosionSound;
    private AudioSource audioSource;
    public AudioManager audioManager;

    //Healthbar stuff
    public RectTransform healthbar;
    public RectTransform healthTransform;
    public RectTransform healthDelayTransform;
    public Canvas canvas;
    private float cachedY;
    private float minXValue;
    private float maxXValue;
    private float currentHealth;
    private float maxHealth;

    //Player Health
    public Image heart1;
    public Image heart2;
    public Image heart3;
    private int playerLife;

    //Is the game playable?
    //[HideInInspector] public bool canMove = false;
    [HideInInspector] public bool canShoot = false;
    [HideInInspector] public bool canMove = false;
    [HideInInspector] public bool inCutscene = true;
    private bool loadingHealth = false;

    //Sets barriers
    public GameObject bottomBarrier;
    public GameObject topBarrier;
    public GameObject rightBarrier;
    public GameObject leftBarrier;

    //Menu stuff
    public GameObject pauseMenu;
    public GameObject endScreen;
    public GameObject ResumeButton;
    public Text timer;

    public Dialogue dialogue1;
    public Dialogue dialogue2;
    public Dialogue dialogue3;

    private decimal time = 0;
    public string playerName;

    // Use this for initialization
    void Start () {
        //Set health properties
        cachedY = healthTransform.position.y;
        maxXValue = healthTransform.position.x;
        minXValue = healthTransform.position.x - (healthTransform.rect.width - 10) * canvas.scaleFactor;
        currentHealth = maxHealth;
        
        RectTransform botrect = (RectTransform)bottomBarrier.transform;
        bottomBarrier.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 0f)) + new Vector3(0f, -0.14f, 10f);
        RectTransform toprect = (RectTransform)topBarrier.transform;
        topBarrier.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0.5f, 1f)) + new Vector3(0f, 0.14f, 10f);
        RectTransform rightrect = (RectTransform)rightBarrier.transform;
        rightBarrier.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(1f, 0.5f)) + new Vector3(0.16f, 0f, 10f);
        RectTransform leftrect = (RectTransform)leftBarrier.transform;
        leftBarrier.transform.position = Camera.main.ViewportToWorldPoint(new Vector2(0f, 0.5f)) + new Vector3(-0.16f, 0f, 10f);

        //Setting up menu
        Button pause = ResumeButton.GetComponent<Button>();
        pause.GetComponent<Button>().onClick.AddListener(() => {
            TogglePauseMenu();
        });
        pauseMenu.SetActive(false);
        endScreen.SetActive(false);
        timer.text = "";

        playerName = PlayerPrefs.GetString("PlayerName");

        audioSource = gameObject.GetComponent<AudioSource>();

        healthbar.transform.localPosition = healthbar.transform.localPosition + new Vector3(0, 20, 0);
        shaggy1.transform.position = new Vector3(12, 0, 0);
        shaggy2.transform.position = new Vector3(12, 0, 0);
        StartCoroutine(EnterFight());
        //EnterPhase1();
    }

    IEnumerator DisplayHealthBar() {
        for (int i=1; i<=20; i++) {
            healthbar.transform.localPosition += new Vector3(0, -1, 0);
            yield return null;
        }
    }

    IEnumerator HideHealthBar() {
        for (int i=1; i<=20; i++) {
            healthbar.transform.localPosition += new Vector3(0, 1, 0);
            yield return null;
        }
    }

    IEnumerator moveTo(GameObject obj, Vector3 pos1, Vector3 pos2, float t) {
        for (int i = 1; i <= t * 100; i++) {
            obj.transform.position = Vector3.Lerp(pos1, pos2, i / (t * 100));
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator EnterFight() {
        StartCoroutine(moveTo(shaggy1, shaggy1.transform.position, Position2.position, 2f));
        yield return new WaitForSeconds(5f);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue1);
    }

    public void TogglePauseMenu() {
        if (!pauseMenu.activeSelf && GameObject.Find("InstructionsPanel") == null) {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else if (pauseMenu.activeSelf) {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }

    public void StartNextPhase() {
        switch (phase) {
            case 0: EnterPhase1(); break;
            case 1: StartCoroutine(EnterPhase2()); break;
            case 2: StartCoroutine(EnterPhase3()); break;
            default: break;
        }
    }

    // Update is called once per frame
    void Update () {
        if (phase == 1 && !inCutscene) {
            currentHealth = shaggy1.GetComponent<Shaggy>().currentHealth;
            if (currentHealth <= 0) {
                StartCoroutine(HideHealthBar());
                StartCoroutine(Shaggy1Death());
                StartCoroutine(audioManager.FadeOut());
                canMove = false;
                canShoot = false;
                inCutscene = true;
            }
        }
        else if (phase == 2 && !inCutscene) {
            currentHealth = shaggy2.GetComponent<Shaggy2>().currentHealth;
            if (currentHealth <= 0) {
                StartCoroutine(HideHealthBar());
                StartCoroutine(audioManager.FadeOut());
                canMove = false;
                canShoot = false;
                inCutscene = true;
            }
        }
        if (!loadingHealth && !inCutscene) {
            HandleHealth();
        }
        if (!inCutscene) {
            if (Input.GetKeyDown(KeyCode.Escape))
                TogglePauseMenu();

            time += System.Math.Round((decimal)Time.deltaTime, 2);
            timer.text = time.ToString();
        }
        if (ship != null)
            playerLife = ship.GetComponent<CharacterController>().getLife();
        if (playerLife == 0 && !inCutscene) {
            StartCoroutine(PlayerDeath());
        }
	}
    

    private void HandleHealth() {
        float currentXValue = MapValues(currentHealth, 0, maxHealth, minXValue, maxXValue);
        healthTransform.position = new Vector2(currentXValue, cachedY);
        healthDelayTransform.position = Vector2.Lerp(healthDelayTransform.position, new Vector2(currentXValue, cachedY), Time.deltaTime * 2);

    }

    private float MapValues(float x, float inMin, float inMax, float outMin, float outMax) {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    IEnumerator LoadHealth() {
        canShoot = false;
        inCutscene = true;
        loadingHealth = true;
        healthDelayTransform.position = new Vector3(minXValue, cachedY, 0f);
        healthTransform.position = new Vector3(minXValue, cachedY, 0f);
        currentHealth = 0;
        yield return new WaitForSeconds(0.2f);
        for (int i=1; i<=100; i++) {
            healthTransform.position = Vector3.Lerp(new Vector3(minXValue, cachedY, 0f), new Vector3(maxXValue, cachedY, 0f), 1f * i / 100);
            yield return new WaitForSeconds(0.015f);
        }
        healthDelayTransform.position = new Vector3(maxXValue, cachedY, 0f);
        canShoot = true;
        inCutscene = false;
        loadingHealth = false;
    }

    void EnterPhase1() {
        StartCoroutine(audioManager.playMusic1());
        shaggy1.SetActive(true);
        shaggy2.SetActive(false);
        maxHealth = shaggy1.GetComponent<Shaggy>().maxHealth;
        StartCoroutine(DisplayHealthBar());
        StartCoroutine(LoadHealth());
        phase = 1;
        canMove = true;
        shaggy1.GetComponent<Shaggy>().BeginPhase1();
    }

    IEnumerator EnterPhase2() {
        StartCoroutine(audioManager.playMusic2());
        shaggy2.SetActive(true);
        maxHealth = shaggy2.GetComponent<Shaggy2>().maxHealth;
        StartCoroutine(moveTo(shaggy2, shaggy2.transform.position, new Vector3(4.5f, 0.5f, 0), 3f));
        yield return new WaitForSeconds(5f);
        shaggy2.GetComponent<Shaggy2>().BeginPhase2();
        yield return new WaitForSeconds(2);
        StartCoroutine(DisplayHealthBar());
        StartCoroutine(LoadHealth());
        phase = 2;
        canMove = true;
    }
    
    IEnumerator EnterPhase3() {
        yield return new WaitForSeconds(0.5f);
        audioSource.volume = 0.5f;
        audioSource.Play();
        yield return new WaitForSeconds(0.2f);
        shaggy2renderer.sprite = shaggy3;
        for (int i=0; i<110; i++) {
            shaggy2.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        Quit();
    }

    public void Quit() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
    }

    public void setTime(decimal time) {
        this.time = time;
    }

    public List<PlayerTimeEntry> LoadPreviousTimes() {
        //refer to note 1
        var scoresFile = Application.persistentDataPath +
              "/2c" + "_times.dat";
        try {
            using (var stream = File.Open(scoresFile, FileMode.Open)) {
                var bin = new BinaryFormatter();
                var times = (List<PlayerTimeEntry>)bin.Deserialize(stream);
                return times;
            }
        }
        //refer to note 2
        catch (IOException ex) {
            Debug.LogWarning("Couldn't load previous times for: " +
              playerName + ".Exception: " + ex.Message);
            return new List<PlayerTimeEntry>();
        }
    }

    public void SaveTime(decimal time) {
        //refer to note 3
        var times = LoadPreviousTimes();
        //refer to note 4
        var newTime = new PlayerTimeEntry();
        newTime.entryDate = DateTime.Now;
        newTime.time = time;
        newTime.PlayerName = playerName;
        newTime.Lives = ship.GetComponent<CharacterController>().getLife();
        //refer to note 5
        var bFormatter = new BinaryFormatter();
        var filePath = Application.persistentDataPath +
          "/2c" + "_times.dat";
        using (var file = File.Open(filePath, FileMode.Create)) {
            times.Add(newTime);
            bFormatter.Serialize(file, times);
        }
    }

    public void EndGame() {
        StartCoroutine(FinishGame());
    }

    IEnumerator FinishGame() {
        if (playerName != "xXMLGHackerXx")
            SaveTime(time);
        yield return new WaitForSeconds(2.5f);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue3);
    }
    /*
    IEnumerator EndGameRoutine() {
        yield return new WaitForSeconds(1f);
        endScreen.SetActive(true);
        GameObject.Find("ScoreText").GetComponent<Text>().text = "Your time is: " + time;
        if (playerName != "xXMLGHackerXx")
            SaveTime(time);
    }
    */
    IEnumerator PlayerDeath() {
        inCutscene = true;
        canShoot = false;
        canMove = false;
        ship.GetComponent<CharacterController>().StopAllCoroutines();
        if (phase == 1)
            shaggy1.GetComponent<Shaggy>().StopAllCoroutines();
        else
            shaggy2.GetComponent<Shaggy2>().StopAllCoroutines();
        //GameObject timerObject = GameObject.Find("Timer");
        //timerObject.GetComponent<Timer>().enabled = false;
        yield return new WaitForSeconds(3f);
        GameObject exp = Instantiate(explosion);
        audioSource.PlayOneShot(explosionSound);
        exp.transform.position = ship.transform.position + new Vector3(0.3f, -0.2f, 0f);
        exp.transform.localScale = new Vector3(1f, 1f, 1f);
        Destroy(exp, 2f);
        yield return new WaitForSeconds(0.2f);
        Destroy(ship);
        yield return new WaitForSeconds(1f);
        endScreen.SetActive(true);
        GameObject.Find("ScoreText").GetComponent<Text>().text = "You're like, really bad at this game, man.";
    }

    IEnumerator Shaggy1Death() {
        shaggy1.GetComponent<Shaggy>().StopAllCoroutines();
        yield return new WaitForSeconds(1f);
        GameObject exp1 = Instantiate(explosion);
        audioSource.PlayOneShot(explosionSound);
        exp1.transform.position = shaggy1.transform.position + new Vector3(0.8f, 0.2f, -2);
        Destroy(exp1, 2f);
        yield return new WaitForSeconds(0.5f);
        GameObject exp2 = Instantiate(explosion);
        audioSource.PlayOneShot(explosionSound);
        exp2.transform.position = shaggy1.transform.position + new Vector3(-1f, 0, -2);
        Destroy(exp2, 2f);
        yield return new WaitForSeconds(0.5f);
        GameObject exp3 = Instantiate(explosion);
        audioSource.PlayOneShot(explosionSound);
        exp3.transform.position = shaggy1.transform.position + new Vector3(0f, -2, -2);
        Destroy(exp3, 2f);
        yield return new WaitForSeconds(1.5f);
        GameObject exp4 = Instantiate(explosion);
        audioSource.PlayOneShot(explosionSound);
        exp4.transform.position = new Vector3(5f, -2f, -5);//transform.position + new Vector3(0, -3f, -5);
        exp4.transform.localScale = new Vector3(4f, 4f, 4f);
        yield return new WaitForSeconds(0.2f);
        Destroy(shaggy1);
        yield return new WaitForSeconds(3f);
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue2);
    }
    
}
