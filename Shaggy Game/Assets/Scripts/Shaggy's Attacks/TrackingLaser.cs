using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingLaser : MonoBehaviour {

    private GameObject laserStart;
    private GameObject laserEnd;
    private GameObject laser;
    public Sprite sprite1;
    public Sprite sprite2;

    private BoxCollider2D lineCollider;
    private Renderer renderer;
    private SpriteRenderer spriteRenderer;

    private GameObject audioController;
    private AudioManager audioManager;
    private bool isPlayingSound = false;
    private AudioSource audioSource;

    private float laserLength = 120f;
    private float actualLaserLength;
    private float laserWidth = 1.6f;
    private float aimTime = 2f;//duration of aiming
    private float waitTime = 0.5f;//time between aiming and shooting
    private float shotTime = 1f;//duration of shooting
    private float shrinkTime = 0.5f;//time after shooting laser
    private Vector3 originalScale;

    private int i = 4;
    private float[] action = new float[4];//0-aimTime, 1-waitTime, 2-shotTime, 3-waitTime2
    float timeRemaining;
    private float t;
    private float timer;

    public void setOrigin(GameObject origin) {
        laserStart = origin;
    }
    public void setTarget(GameObject target) {
        laserEnd = target;
    }

    private void Start() {
        laser = gameObject;

        originalScale = new Vector3(laserWidth, laserLength, 1f);
        laser.transform.localScale = originalScale;

        renderer = laser.GetComponent<Renderer>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite1;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);

        actualLaserLength = renderer.bounds.size.y;
        action[0] = aimTime;
        action[1] = waitTime;
        action[2] = shotTime;
        action[3] = shrinkTime;

        lineCollider = laser.gameObject.GetComponent<BoxCollider2D>();

        lineCollider.enabled = false;
        timeRemaining = action[0];

        audioController = GameObject.Find("AudioManager");
        audioManager = audioController.GetComponent<AudioManager>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update() {
        timeRemaining -= Time.deltaTime;
        t += Time.deltaTime / waitTime;
        
        if (i == 0) {//aimTime
            follow(laserStart, laserEnd, actualLaserLength, laserWidth);
            lineCollider.enabled = false;
        }
        else if (i == 1) {//waitTime
            laser.transform.localScale = Vector3.Lerp(originalScale, new Vector3(0f, laserLength, 1f), t);
            lineCollider.enabled = false;
        }
        else if (i == 2){
            laser.transform.localScale = new Vector3(laserWidth, laserLength, 1f);
            lineCollider.enabled = true;
            laserWidth = 2f;
            spriteRenderer.sprite = sprite2;
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            if (!isPlayingSound) {
                //StartCoroutine(audioManager.FadeIn(audioSource, 0.1f));
                audioSource.Play();
                isPlayingSound = true;
            }
        }
        else {
            laser.transform.localScale = Vector3.Lerp(new Vector3(laserWidth, laserLength, 1f), new Vector3(0f, laserLength, 1f), t);
            lineCollider.enabled = true;
            if (isPlayingSound) {
                //StartCoroutine(audioManager.FadeOut(audioSource, 0.1f));
                isPlayingSound = false;
            }
        }

        if (timeRemaining <= 0) {
            if (i < action.Length - 1)
                i++;
            else
                Destroy(gameObject);
            timeRemaining = action[i];
            t = 0;
        }
    }

    float getAngle(GameObject shooter, GameObject target) {
        float x = target.transform.position.x - shooter.transform.position.x;
        float y = target.transform.position.y - shooter.transform.position.y;
        return Mathf.Atan(y / x) * Mathf.Rad2Deg;
    }

    void follow(GameObject shooter, GameObject target, float lineLength, float lineWidth) {
        float angle = getAngle(shooter, target);
        Vector3 pos = shooter.transform.position;

        if (target.transform.position.x <= shooter.transform.position.x)
            laser.transform.position = new Vector3(-Mathf.Cos(angle * Mathf.Deg2Rad), -Mathf.Sin(angle * Mathf.Deg2Rad), 0).normalized * lineLength / 2 + shooter.transform.position;
        else
            laser.transform.position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0).normalized * lineLength / 2 + shooter.transform.position;
        
        laser.transform.rotation = Quaternion.Euler(0, 0, angle + 90);//rotates cube
    }

    public void fireLaser() {
        i = 0;
    }
}
