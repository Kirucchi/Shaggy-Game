using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour {

    public GameManager gameManager;
    public GameObject playerBullet;
    public GameObject bulletStart;
    public Image heart1;
    public Image heart2;
    public Image heart3;
    public Image heart4;
    public Image heart5;

    public AudioClip deathSound;
    public AudioClip shootSound;
    private AudioSource audioSource;

    public SpriteRenderer shipSprite;
    public SpriteRenderer hitboxSprite;
    
    private float speed;
    private int life = 5;
    public int getLife() {
        return life;
    }
    private bool invuln = true;

    private float myTime = 0.0f;
    private float shootDelay = 0.05f;

    private bool canShoot;
    private bool canMove = true;
    private bool inCutScene = true;
    private bool inCoolDown = false;
    private string playerName;

    private void Awake() {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    void Update () {
        canShoot = gameManager.canShoot;
        canMove = gameManager.canMove;
        inCutScene = gameManager.inCutscene;
        playerName = gameManager.playerName;

        if (inCutScene) {
            invuln = true;
        }
        else if (!inCoolDown && playerName != "xXMLGHackerXx") {
            invuln = false;
        }

        if (canShoot) {
            //Creates delay between shots
            myTime = myTime + Time.deltaTime;
            if (Input.GetButton("Fire1") && myTime > shootDelay) {
                GameObject bullet1 = Instantiate(playerBullet);
                bullet1.transform.position = bulletStart.transform.position;
                audioSource.PlayOneShot(shootSound);

                myTime = 0.0f;
            }
        }
        //Set the speed of the player
        if (Input.GetKey(KeyCode.LeftShift)) {
            speed = 3f;
        }
        else {
            speed = 7f;
        }

        //Moving the player
        if (canMove) {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            Vector2 direction = new Vector2(x, y).normalized;
            Move(direction);
        }
        
    }

    private void Move(Vector2 direction) {
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //max.x = max.x;
        min.x = min.x + 0.1f;
        max.y = max.y - 0.575f;
        min.y = min.y + 0.15f;

        Vector2 pos = transform.position;

        pos += direction * speed * Time.deltaTime;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        transform.position = pos;
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Shaggy1") || collision.CompareTag("Shaggy2")) && !invuln) {
            removeLife();
        }
    }

    private void removeLife() {

        audioSource.PlayOneShot(deathSound);
        switch (life) {
            case 5:
                heart5.enabled = false;
                break;
            case 4:
                heart4.enabled = false;
                break;
            case 3:
                heart3.enabled = false;
                break;
            case 2:
                heart2.enabled = false;
                break;
            case 1:
                heart1.enabled = false;
                break;
            default:
                break;
        }
        
        life--;
        if (life > 0)
            StartCoroutine(DamageCooldown());
    }

    IEnumerator DamageCooldown() {
        inCoolDown = true;
        invuln = true;
        for (int i=0; i<5; i++) {
            shipSprite.enabled = false;
            hitboxSprite.enabled = false;
            yield return new WaitForSeconds(0.1f);
            shipSprite.enabled = true;
            hitboxSprite.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        for (int i=0; i<10; i++) {
            shipSprite.enabled = false;
            hitboxSprite.enabled = false;
            yield return new WaitForSeconds(0.05f);
            shipSprite.enabled = true;
            hitboxSprite.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
        for (int i=0; i<20; i++) {
            shipSprite.enabled = false;
            hitboxSprite.enabled = false;
            yield return new WaitForSeconds(0.025f);
            shipSprite.enabled = true;
            hitboxSprite.enabled = true;
            yield return new WaitForSeconds(0.025f);
        }
        invuln = false;
        inCoolDown = false;
    }
    
}