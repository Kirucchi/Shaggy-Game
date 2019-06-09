using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Shaggy : MonoBehaviour {

    //Health bar properties
    [HideInInspector] public float currentHealth;
    private bool restoringHealth;
    public float maxHealth;


    //idle animation properties
    private float startX;
    private float startY;

    ObjectPooler objectPooler;
    public GameObject objectPoolerObject;

    private AudioSource audioSource;
    public AudioClip damageSound;

    //Shaggy Positions
    public Transform Position1;
    public Transform Position2;
    public Transform Position3;

    void Awake() {
        //set hpbar properties
        /*
        cachedY = healthTransform.position.y;
        maxXValue = healthTransform.position.x;
        minXValue = healthTransform.position.x - (healthTransform.rect.width - 10) * canvas.scaleFactor;
        */
        currentHealth = maxHealth;
        restoringHealth = false;

        //set idle animation properties
        startX = gameObject.transform.position.x;
        startY = gameObject.transform.position.y;

        //objectPooler = ObjectPooler.Instance;
        objectPooler = objectPoolerObject.GetComponent<ObjectPooler>();

        audioSource = gameObject.GetComponent<AudioSource>();

        //transform.position = new Vector3(12f, 0f, 0f);
        //StartCoroutine(moveTo(gameObject, transform.position, Position2.position, 2f));
    }

    public void ReduceHealth() {
        audioSource.PlayOneShot(damageSound);
        if (currentHealth > 0 && !restoringHealth){
            currentHealth -= 1;
        }
    }

    public void BeginPhase1() {
        //transform.position = new Vector3(12f, 0f, 0f);
        StartCoroutine(Phase1());
    }


    IEnumerator moveTo(GameObject obj, Vector3 pos1, Vector3 pos2, float t) {
        for (int i = 1; i <= t * 100; i++) {
            obj.transform.position = Vector3.Lerp(pos1, pos2, i / (t * 100));
            yield return new WaitForSeconds(0.01f);
        }
    }

    private void Update() {
        //idleMove();
        /*
        if (Input.GetKeyDown(KeyCode.Z)) {
            StartCoroutine(Attack1());
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            StartCoroutine(Attack2());
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            StartCoroutine(Attack3());
        }
        if (Input.GetKeyDown(KeyCode.V)) {
            StartCoroutine(Attack4());
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            StartCoroutine(Attack5());
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            StartCoroutine(Attack6());
        }
        */
    }

    private void FixedUpdate() {

    }
 
    private void idleMove() {
        gameObject.transform.position = new Vector2(startX + Mathf.Sin(-Time.time * 2), startY + 2.5f * Mathf.Cos(-Time.time * 2));
    }








    //Attacks

    //public GameObject ship;


    //Attack 1
    public GameObject attack1Origin;

    IEnumerator Attack1() {//2 seconds
        for (int j = 0; j < 50; j++) {
            for (int i = 0; i < 8; i++) {
                float angle = Mathf.PI * 2 * i / 8 + j * 0.18f;
                Vector3 pos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f) * 0.3f + attack1Origin.transform.position;
                Vector3 direction = (pos - attack1Origin.transform.position).normalized;
                GameObject obj = objectPooler.SpawnFromPool("EnemyBullet1", pos, Quaternion.identity);
                obj.GetComponent<Attack1>().setDirection(direction, 4f);
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.03f);//0.03
        }
    }


    //Attack 2
    public GameObject attack2Origin;

    IEnumerator Attack2() {//5.3 seconds
        for (int i = 0; i < 252; i++) {
            float angle = i * 0.1f + Mathf.PI;
            for (int j = 0; j < 2; j++) {
                Vector3 pos;
                GameObject obj;
                if (j == 0) {
                    pos = (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f)) * 0.3f + attack2Origin.transform.position;
                    obj = objectPooler.SpawnFromPool("EnemyBullet2", pos, Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * angle + 90f));
                }
                else {
                    pos = (new Vector3(Mathf.Cos(angle), -Mathf.Sin(angle), 0f)) * 0.3f + attack2Origin.transform.position;
                    obj = objectPooler.SpawnFromPool("EnemyBullet2", pos, Quaternion.Euler(0f, 0f, -Mathf.Rad2Deg * angle + 90f));
                }
                Vector3 direction = (pos - attack2Origin.transform.position).normalized;
                obj.GetComponent<Attack1>().setDirection(direction, 4f);
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.01f);
        }
    }



    //Attack 3
    public GameObject attack3Origin;

    IEnumerator Attack3() {
        float totalAngle = 0f;
        float angle;
        Vector3 pos;
        Vector3 direction;
        for (int i = 0; i < 30; i++) {//1 second
            totalAngle = 0.1f * (i);
            for (int j = 0; j <= i; j++) {
                if (i == 0) {
                    angle = 0f;
                }
                else {
                    angle = totalAngle / 2 - ((totalAngle / i) * j);
                }
                GameObject obj;
                pos = (new Vector3(-Mathf.Cos(angle), Mathf.Sin(angle), 0f)) * 0.3f + attack3Origin.transform.position;
                obj = objectPooler.SpawnFromPool("EnemyBullet3", pos, Quaternion.Euler(0f, 0f, -Mathf.Rad2Deg * angle + 90f));
                direction = (pos - attack3Origin.transform.position).normalized;
                obj.GetComponent<Attack1>().setDirection(direction, 6f);
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.02f);
        }
    }



    //Attack 4
    public GameObject attack4Origin;

    IEnumerator Attack4() {// 4 seconds
        float angle;
        Vector3 pos;
        Vector3 direction;
        for (int i = 0; i < 100; i++) {
            for (int j = 0; j < 3; j++) {
                for (int k = 0; k < 3; k++) {
                    angle = 2f * Mathf.PI / 3f * k + 0.23f * j + i * 0.1f;
                    GameObject obj;
                    pos = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * 0.3f + attack4Origin.transform.position;
                    obj = objectPooler.SpawnFromPool("EnemyBullet4", pos, Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * angle + 90f));
                    direction = (pos - attack4Origin.transform.position).normalized;
                    obj.GetComponent<Attack1>().setDirection(direction, 6f);
                }
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.03f);
        }
    }


    //Attack 5

    public GameObject ship;
    public GameObject laserOrigin;
    public GameObject laser;

    IEnumerator Attack5() {
        GameObject obj = Instantiate(laser);
        obj.GetComponent<TrackingLaser>().setOrigin(laserOrigin);
        obj.GetComponent<TrackingLaser>().setTarget(ship);
        obj.GetComponent<TrackingLaser>().fireLaser();
        obj.transform.parent = gameObject.transform;
        yield return new WaitForSeconds(0.01f);
        obj.GetComponent<SpriteRenderer>().enabled = true;
    }




    IEnumerator Phase1() {
        /*
        StartCoroutine(moveTo(gameObject, transform.position, Position2.position, 2f));
        yield return new WaitForSeconds(5f);
        */
        yield return new WaitForSeconds(2f);
        while (true) {
            for (int i = 0; i < 2; i++) {
                StartCoroutine(Attack1());
                yield return new WaitForSeconds(2.2f);
                StartCoroutine(moveTo(gameObject, transform.position, Position1.position, 0.3f));
                yield return new WaitForSeconds(0.6f);
                StartCoroutine(Attack1());
                yield return new WaitForSeconds(2.2f);
                StartCoroutine(moveTo(gameObject, transform.position, Position3.position, 0.3f));
                yield return new WaitForSeconds(0.6f);
                StartCoroutine(Attack1());
                yield return new WaitForSeconds(2.2f);
                StartCoroutine(moveTo(gameObject, transform.position, Position2.position, 0.3f));
                yield return new WaitForSeconds(0.6f);
            }
            yield return new WaitForSeconds(1f);
            StartCoroutine(moveTo(gameObject, transform.position, Position1.position, 0.3f));
            yield return new WaitForSeconds(0.6f);
            StartCoroutine(Attack4());
            StartCoroutine(moveTo(gameObject, transform.position, Position3.position, 2f));
            yield return new WaitForSeconds(4f);
            StartCoroutine(Attack4());
            StartCoroutine(moveTo(gameObject, transform.position, Position1.position, 2f));
            yield return new WaitForSeconds(4f);
            StartCoroutine(moveTo(gameObject, transform.position, Position2.position, 0.5f));
            yield return new WaitForSeconds(1f);
            StartCoroutine(Attack2());
            yield return new WaitForSeconds(1f);
            StartCoroutine(Attack5());
            yield return new WaitForSeconds(3f);
            StartCoroutine(Attack5());
            yield return new WaitForSeconds(4f);
            StartCoroutine(Attack1());
            yield return new WaitForSeconds(2.2f);
            StartCoroutine(moveTo(gameObject, transform.position, Position1.position, 0.3f));
            yield return new WaitForSeconds(0.6f);
            StartCoroutine(Attack3());
            yield return new WaitForSeconds(2f);
            StartCoroutine(moveTo(gameObject, transform.position, Position3.position, 0.3f));
            yield return new WaitForSeconds(0.6f);
            StartCoroutine(Attack3());
            yield return new WaitForSeconds(2f);
            StartCoroutine(moveTo(gameObject, transform.position, Position2.position, 0.3f));
            yield return new WaitForSeconds(1f);
        }
        
    }
}