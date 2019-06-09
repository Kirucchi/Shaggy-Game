using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBolt : MonoBehaviour {

    private GameObject player;
    private AudioSource audioSource;

    private Vector3 target = Vector3.zero;
    private float time = 0.2f;
    private Vector3 velocity = Vector3.zero;
    private float timer = 0f;
    private Vector3 pos = Vector3.zero;

    private SpriteRenderer spriteRenderer;
    private Renderer renderer;
    private float length;
    private BoxCollider2D collider;

    public Sprite bolt;
    public Sprite laser;

    private float angle = 0;
    private bool playSound = true;

    public void setTarget(Vector3 newTarget, GameObject targetPlayer) {
        spriteRenderer.sprite = bolt;
        collider.size = new Vector2(0.15f, 0.5f);
        timer = 0f;
        target = newTarget;
        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan((target.y - transform.position.y) / (target.x - transform.position.x)) * Mathf.Rad2Deg - 90f);
        transform.localScale = new Vector3(1f, 1f, 1f);
        player = targetPlayer;
        playSound = true;
    }

    private void Awake() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        collider = gameObject.GetComponent<BoxCollider2D>();
        renderer = gameObject.GetComponent<Renderer>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update() {
        if (timer < 1f)
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, time);
        else if (timer < 1.5f) {
            angle = getAngle(gameObject, player);
            transform.eulerAngles = new Vector3(0f, 0f, angle - 90);
            pos = transform.position;
        }
        else if (timer > 2f && timer < 2.2f) {
            spriteRenderer.sprite = laser;
            collider.size = new Vector2(0.32f, 0.28f);
            transform.localScale = new Vector3(0.7f, 80f, 1f);
            length = renderer.bounds.size.x;
            transform.position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f).normalized * length / 2 + pos;
            if (playSound) {
                audioSource.Play();
                playSound = false;
            }
        }
        else if (timer > 2.2f)
            gameObject.SetActive(false);
        timer += Time.deltaTime;
    }

    float getAngle(GameObject shooter, GameObject target) {
        float x = target.transform.position.x - shooter.transform.position.x;
        float y = target.transform.position.y - shooter.transform.position.y;
        return Mathf.Atan2(y, x) * Mathf.Rad2Deg;
    }
    
}
