using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementalLaser : MonoBehaviour {

    public Sprite sprite1;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D lineCollider;
    public GameObject explosion;
    private AudioSource source;

    private int i = 0;
    private float[] action = new float[4];//0-aimTime, 1-waitTime, 2-shotTime, 3-waitTime2
    float timeRemaining;
    private float t;
    private float timer;

    private float aimTime = 1.5f;//duration of aiming
    private float waitTime = 1f;//time between aiming and shooting
    private float shotTime = 10f;//duration of shooting
    private float shrinkTime = 1f;//time after shooting laser

    private Vector3 originalScale = new Vector3(1f, 80f, 1f);

    private Vector2 min;
    private Vector2 max;

    void Start () {
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;

        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        gameObject.transform.localScale = originalScale;
        
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite1;
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        
        action[0] = aimTime;
        action[1] = waitTime;
        action[2] = shotTime;
        action[3] = shrinkTime;

        lineCollider = gameObject.GetComponent<BoxCollider2D>();

        lineCollider.enabled = false;
        timeRemaining = action[0];

        source = gameObject.GetComponent<AudioSource>();
        Invoke("playLaserSound", 2.4f);
    }

    void playLaserSound() {
        source.Play();
    }
	
	void Update () {
        timeRemaining -= Time.deltaTime;
        t += Time.deltaTime / waitTime;

        if (i == 0) {//aimTime
            lineCollider.enabled = false;
        }
        else if (i == 1) {//waitTime
            gameObject.transform.localScale = Vector3.Lerp(originalScale, new Vector3(0f, 80f, 1f), t);
            lineCollider.enabled = false;
        }
        else if (i == 2) {
            gameObject.transform.localScale = originalScale;
            lineCollider.enabled = true;
            //spriteRenderer.sprite = sprite2;
            animator.enabled = true;
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            RaycastHit2D[] hits;
            hits = Physics2D.RaycastAll(Vector3.zero, gameObject.transform.position);
            foreach (RaycastHit2D hit in hits) {
                if (hit.collider.CompareTag("Barrier")) {
                    GameObject particleExplosion = Instantiate(explosion);
                    particleExplosion.transform.position = hit.point;
                    Destroy(particleExplosion, 3f);
                }
            }
        }
        else if (i == 3) {
            gameObject.transform.localScale = Vector3.Lerp(originalScale, new Vector3(0f, 80f, 1f), t);
            lineCollider.enabled = true;
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
}
