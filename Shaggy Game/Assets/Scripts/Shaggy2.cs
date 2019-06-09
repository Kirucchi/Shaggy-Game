using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Shaggy2 : MonoBehaviour {
    
    [HideInInspector] public float currentHealth;
    private bool restoringHealth;
    public float maxHealth;


    //idle animation properties
    private float startX;
    private float startY;

    ObjectPooler objectPooler;
    public GameObject objectPoolerObject;

    private AudioSource audioSource;
    public AudioClip shotSound;
    public AudioClip damageSound;
    public AudioClip explosion2Sound;

    public GameObject explosion;

    public GameObject gameManager;

    void Awake() {
        //set hpbar properties
        currentHealth = maxHealth;
        restoringHealth = false;

        //set idle animation properties
        startX = gameObject.transform.position.x;
        startY = gameObject.transform.position.y;

        //objectPooler = ObjectPooler.Instance
        objectPooler = objectPoolerObject.GetComponent<ObjectPooler>();

        audioSource = gameObject.GetComponent<AudioSource>();

        fireOrb.transform.localPosition = new Vector3(0, 60, 0);
        lightningOrb.transform.localPosition = new Vector3(60, 0, 0);
        waterOrb.transform.localPosition = new Vector3(0, -60, 0);
        iceOrb.transform.localPosition = new Vector3(-60, 0, 0);
    }

    public void BeginPhase2() {
        //transform.position = new Vector3(4.5f, 0.5f, 0);
        StartCoroutine(Phase2());
    }

    public void ReduceHealth() {
        audioSource.PlayOneShot(damageSound);
        if (currentHealth > 0 && !restoringHealth) {
            currentHealth -= 1;
        }
        if (currentHealth <= 0) {
            StopAllCoroutines();
            StartCoroutine(Shaggy2Death());
        }
    }

    private void Update() {
        //idleMove();
        /*
        if (Input.GetKeyDown(KeyCode.Z)) {
            StartCoroutine(FireOrbAttack());
        }
        if (Input.GetKeyDown(KeyCode.X)) {
            StartCoroutine(LightningOrbAttack());
        }
        if (Input.GetKeyDown(KeyCode.C)) {
            StartCoroutine(WaterOrbAttack());
        }
        if (Input.GetKeyDown(KeyCode.V)) {
            StartCoroutine(IceOrbAttack());
        }
        if (Input.GetKeyDown(KeyCode.B)) {
            StartCoroutine(Attack5());
        }
        if (Input.GetKeyDown(KeyCode.N)) {
            StartCoroutine(Attack6());
        }
        */
    }
    
    private void idleMove() {
        gameObject.transform.position = new Vector2(startX + Mathf.Sin(-Time.time * 2), startY + 2.5f * Mathf.Cos(-Time.time * 2));
    }

    IEnumerator moveTo(GameObject obj, Vector3 pos1, Vector3 pos2, float t) {
        for (int i = 1; i <= t * 100; i++) {
            obj.transform.position = Vector3.Lerp(pos1, pos2, i / (t * 100));
            yield return new WaitForSeconds(0.01f);
        }
    }
    

    IEnumerator moveToLocal(GameObject obj, Vector3 pos1, Vector3 pos2, float t) {
        for (int i=1; i <= 100 * t; i++) {
            obj.transform.localPosition = Vector3.Lerp(pos1, pos2, i / (t * 100));
            yield return new WaitForSeconds(0.01f);
        }
    }






    //Attacks

    public GameObject ship;



    //Attack 5
    public GameObject laserOrigin;
    public GameObject laser;

    IEnumerator Attack5() {
        GameObject obj = Instantiate(laser);
        obj.GetComponent<TrackingLaser>().setOrigin(laserOrigin);
        obj.GetComponent<TrackingLaser>().setTarget(ship);
        obj.GetComponent<TrackingLaser>().fireLaser();
        yield return new WaitForSeconds(0.01f);
        obj.GetComponent<SpriteRenderer>().enabled = true;
    }



    //Attack 6
    public GameObject orbCenter;

    public GameObject fireOrb;
    public GameObject lightningOrb;
    public GameObject waterOrb;
    public GameObject iceOrb;

    public GameObject fireParticle;
    public GameObject lightningParticle;
    public GameObject waterParticle;
    public GameObject iceParticle;

    private Vector3 originalSize = new Vector3(2.5f, 2.5f, 1f);
    private Vector3 originalParticleSize = new Vector3(0.5f, 0.5f, 0.5f);
    private List<GameObject> orbs = new List<GameObject>();

    IEnumerator Attack6() {//7 seconds
        orbs.Add(fireOrb); orbs.Add(lightningOrb); orbs.Add(waterOrb); orbs.Add(iceOrb);
        Vector3 pos;
        Vector3 direction;
        float angle;
        for (int i = 0; i < 100; i++) {
            for (int j = 1; j <= 4; j++) {
                pos = orbs[j - 1].transform.position;
                direction = (ship.transform.position - pos).normalized;
                angle = Mathf.Atan(direction.y / direction.x);
                GameObject obj = objectPooler.SpawnFromPool("ElementBullet" + j, pos, Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * angle + 90));
                obj.GetComponent<Attack1>().setDirection(direction, 15f);
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.05f);
        }
    }


    IEnumerator SpawnFireOrb() {
        fireOrb.SetActive(true);
        fireOrb.transform.parent = orbCenter.transform;
        fireOrb.transform.localScale = new Vector3(0f, 0f, 0f);
        fireOrb.transform.localPosition = new Vector3(0f, 0f, 0f);
        for (int i = 1; i <= 100; i++) {
            fireOrb.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), originalSize, 0.01f * i);
            fireOrb.transform.localPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(0f, 5f, 0f), 0.01f * i);
            fireParticle.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), originalParticleSize, 0.01f * i);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator SpawnLightningOrb() {
        lightningOrb.SetActive(true);
        lightningOrb.transform.parent = orbCenter.transform;
        lightningOrb.transform.localScale = new Vector3(0f, 0f, 0f);
        lightningOrb.transform.localPosition = new Vector3(0f, 0f, 0f);
        for (int i = 1; i <= 100; i++) {
            lightningOrb.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), originalSize, 0.01f * i);
            lightningOrb.transform.localPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(5f, 0f, 0f), 0.01f * i);
            lightningParticle.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), originalParticleSize, 0.01f * i);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator SpawnWaterOrb() {
        waterOrb.SetActive(true);
        waterOrb.transform.parent = orbCenter.transform;
        waterOrb.transform.localScale = new Vector3(0f, 0f, 0f);
        waterOrb.transform.localPosition = new Vector3(0f, 0f, 0f);
        for (int i = 1; i <= 100; i++) {
            waterOrb.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), originalSize, 0.01f * i);
            waterOrb.transform.localPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(0f, -5f, 0f), 0.01f * i);
            waterParticle.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), originalParticleSize, 0.01f * i);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator SpawnIceOrb() {
        iceOrb.SetActive(true);
        iceOrb.transform.parent = orbCenter.transform;
        iceOrb.transform.localScale = new Vector3(0f, 0f, 0f);
        iceOrb.transform.localPosition = new Vector3(0f, 0f, 0f);
        for (int i = 1; i <= 100; i++) {
            iceOrb.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), originalSize, 0.01f * i);
            iceOrb.transform.localPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(-5f, 0f, 0f), 0.01f * i);
            iceParticle.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), originalParticleSize, 0.01f * i);
            yield return new WaitForSeconds(0.01f);
        }
    }


    public AudioClip explosionSound;

    IEnumerator playExplosionSound() {
        audioSource.clip = explosionSound;
        audioSource.volume = 0.05f;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        audioSource.clip = shotSound;
        audioSource.volume = 0.01f;
    }




    //FireOrbAttack

    IEnumerator FireOrbAttack() { //23.0 seconds
        fireOrb.transform.parent = null;
        StartCoroutine(moveTo(fireOrb, fireOrb.transform.position, Vector3.zero, 1f));
        StartCoroutine(moveTo(gameObject, gameObject.transform.position, gameObject.transform.position + new Vector3(10f, 0f, 0f), 1f));
        yield return new WaitForSeconds(3f);
        spawnElementalLasers();
        yield return new WaitForSeconds(1.5f);
        for (int i=0; i<22; i++) {
            for (int j=0; j<12; j++) {
                float angle = 2 * Mathf.PI * j / 12;
                if (i % 2 == 1) {
                    angle += 2 * Mathf.PI / 24;
                }
                Vector3 pos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f) * 0.1f + fireOrb.transform.position;
                Vector3 direction = (pos - fireOrb.transform.position).normalized;
                GameObject obj = objectPooler.SpawnFromPool("ElementBullet5", pos, Quaternion.identity);
                obj.GetComponent<Attack1>().setDirection(direction, 3f);
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(playExplosionSound());
        for (int i=0; i<40; i++) {
            Vector3 pos = new Vector3(Mathf.Sin(i * 2 * Mathf.PI / 40), Mathf.Cos(i * 2 * Mathf.PI / 40), 0f) * 0.1f + fireOrb.transform.position;
            Vector3 direction = (pos - fireOrb.transform.position).normalized;
            GameObject obj = objectPooler.SpawnFromPool("ElementBullet5", pos, Quaternion.identity);
            obj.GetComponent<Attack1>().setDirection(direction, 4f);
        }
        fireOrb.SetActive(false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(moveTo(gameObject, gameObject.transform.position, gameObject.transform.position - new Vector3(10f, 0f, 0f), 1f));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(SpawnFireOrb());
    }

    public GameObject elementalLaser;

    void spawnElementalLasers() {
        GameObject laser1 = Instantiate(elementalLaser);
        laser1.transform.parent = fireOrb.transform;
        laser1.transform.localPosition = new Vector3(0f, -11.2f, 0f);
        laser1.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        GameObject laser2 = Instantiate(elementalLaser);
        laser2.transform.parent = fireOrb.transform;
        laser2.transform.localPosition = new Vector3(0f, 11.2f, 0f);
        laser2.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        GameObject laser3 = Instantiate(elementalLaser);
        laser3.transform.parent = fireOrb.transform;
        laser3.transform.localPosition = new Vector3(11.2f, 0f, 0f);
        laser3.transform.localEulerAngles = new Vector3(0f, 0f, 90f);
        GameObject laser4 = Instantiate(elementalLaser);
        laser4.transform.parent = fireOrb.transform;
        laser4.transform.localPosition = new Vector3(-11.2f, 0f, 0f);
        laser4.transform.localEulerAngles = new Vector3(0f, 0f, -90f);
    }




    //Ice Orb attack

    IEnumerator IceOrbAttack() {//22.3 seconds
        iceOrb.transform.parent = null;
        StartCoroutine(moveTo(iceOrb, iceOrb.transform.position, new Vector3(3f, 0f, 0f), 1f));
        StartCoroutine(moveTo(gameObject, gameObject.transform.position, gameObject.transform.position + new Vector3(10f, 0f, 0f), 1f));
        yield return new WaitForSeconds(3f);
        float minX = iceOrb.transform.position.x +0.2f;
        float maxX = Camera.main.ViewportToWorldPoint(new Vector2(1, 0.5f)).x - 0.1f;
        float minY = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y;
        float maxY = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y;
        for (int i=0; i<150; i++) {
            for (int j=0; j<2; j++) {
                Vector3 destination = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);
                GameObject obj = objectPooler.SpawnFromPool("Icicle", iceOrb.transform.position, Quaternion.identity);
                obj.GetComponent<Icicles>().setTarget(destination);
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.06f);//0.04
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(playExplosionSound());
        for (int i = 0; i < 40; i++) {
            float angle = i * 2 * Mathf.PI / 40;
            Vector3 pos = new Vector3(-Mathf.Sin(angle), Mathf.Cos(angle), 0f) * 0.1f + iceOrb.transform.position;
            Vector3 direction = (pos - iceOrb.transform.position).normalized;
            GameObject obj = objectPooler.SpawnFromPool("ElementBullet8", pos, Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * angle));
            obj.GetComponent<Attack1>().setDirection(direction, 4f);
        }
        iceOrb.SetActive(false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(moveTo(gameObject, gameObject.transform.position, gameObject.transform.position - new Vector3(10f, 0f, 0f), 1f));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(SpawnIceOrb());
    }





    //Lightning orb attack

    IEnumerator LightningOrbAttack() {//28.5 seconds
        lightningOrb.transform.parent = null;
        StartCoroutine(moveTo(lightningOrb, lightningOrb.transform.position, new Vector3(3f, 0f, 0f), 1f));
        StartCoroutine(moveTo(gameObject, gameObject.transform.position, gameObject.transform.position + new Vector3(10f, 0f, 0f), 1f));
        yield return new WaitForSeconds(3f);
        float minX = lightningOrb.transform.position.x + 1f;
        float maxX = Camera.main.ViewportToWorldPoint(new Vector2(1, 0.5f)).x - 0.2f;
        float minY = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y + 0.2f;
        float maxY = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y - 0.4f;
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 10; j++) {
                Vector3 destination = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);
                GameObject obj = objectPooler.SpawnFromPool("LightningBolt", lightningOrb.transform.position, Quaternion.identity);
                obj.GetComponent<LightningBolt>().setTarget(destination, ship);
                audioSource.Play();
                yield return new WaitForSeconds(0.04f);
            }
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);
        for (int i=0; i<150; i++) {
            Vector3 destination = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);
            GameObject obj = objectPooler.SpawnFromPool("LightningBolt", lightningOrb.transform.position, Quaternion.identity);
            obj.GetComponent<LightningBolt>().setTarget(destination, ship);
            audioSource.Play();
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(2f);
        StartCoroutine(playExplosionSound());
        for (int i = 0; i < 40; i++) {
            float angle = i * 2 * Mathf.PI / 40;
            Vector3 pos = new Vector3(-Mathf.Sin(angle), Mathf.Cos(angle), 0f) * 0.1f + lightningOrb.transform.position;
            Vector3 direction = (pos - lightningOrb.transform.position).normalized;
            GameObject obj = objectPooler.SpawnFromPool("ElementBullet6", pos, Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * angle));
            obj.GetComponent<Attack1>().setDirection(direction, 4f);
        }
        lightningOrb.SetActive(false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(moveTo(gameObject, gameObject.transform.position, gameObject.transform.position - new Vector3(10f, 0f, 0f), 1f));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(SpawnLightningOrb());
    }






    //Water orb attack

    IEnumerator WaterOrbAttack() {//31.5 seconds
        waterOrb.transform.parent = null;
        StartCoroutine(moveTo(waterOrb, waterOrb.transform.position, new Vector3(0f, 0f, 0f), 1f));
        StartCoroutine(moveTo(gameObject, gameObject.transform.position, gameObject.transform.position + new Vector3(10f, 0f, 0f), 1f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(waterPattern());
        yield return new WaitForSeconds(6f);
        StartCoroutine(spawnRain());
        yield return new WaitForSeconds(11f);
        StartCoroutine(waterPattern());
        yield return new WaitForSeconds(6f);
        StartCoroutine(playExplosionSound());
        for (int i = 0; i < 40; i++) {
            float angle = i * 2 * Mathf.PI / 40;
            Vector3 pos = new Vector3(-Mathf.Sin(angle), Mathf.Cos(angle), 0f) * 0.1f + waterOrb.transform.position;
            Vector3 direction = (pos - waterOrb.transform.position).normalized;
            GameObject obj = objectPooler.SpawnFromPool("ElementBullet7", pos, Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * angle));
            obj.GetComponent<Attack1>().setDirection(direction, 4f);
        }
        waterOrb.SetActive(false);
        yield return new WaitForSeconds(2f);
        StartCoroutine(moveTo(gameObject, gameObject.transform.position, gameObject.transform.position - new Vector3(10f, 0f, 0f), 1f));
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(SpawnWaterOrb());
    }

    IEnumerator waterPattern() {//4.5 seconds
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 10; j++) {
                for (int k = 0; k < 50; k++) {
                    float angle = 0f;
                    if (i % 2 == 1)
                        angle = k * 2 * Mathf.PI / 50;
                    else
                        angle = (k + 1) * 2 * Mathf.PI / 50;
                    Vector3 pos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f) * 0.2f + waterOrb.transform.position;
                    Vector3 direction = (pos - waterOrb.transform.position).normalized;
                    GameObject obj = objectPooler.SpawnFromPool("WaterWave", pos, Quaternion.identity);
                    bool inverse = false;
                    if (k % 2 == 1)
                        inverse = true;
                    obj.GetComponent<WaterWave>().setDirection(direction, 6f, inverse, 4f, 1.3f);
                }
                audioSource.Play();
                yield return new WaitForSeconds(0.09f);
            }
        }
    }

    IEnumerator spawnRain() {//10.25 seconds
        for (int i = 0; i < 150; i++) {
            for (int j = 0; j < 2; j++) {
                float angle = Random.Range(-Mathf.PI / 10, Mathf.PI / 10);
                Vector3 pos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0f) * 0.2f + waterOrb.transform.position;
                Vector3 direction = (pos - waterOrb.transform.position).normalized;
                GameObject obj = objectPooler.SpawnFromPool("ElementBullet7", pos, Quaternion.Euler(0f, 0f, -Mathf.Rad2Deg * angle));
                obj.GetComponent<Attack1>().setDirection(direction, 15f);
            }
            audioSource.Play();
            yield return new WaitForSeconds(0.015f);
        }

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 5; i++) {//7 seconds
            float minX = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x - 0.1f;
            float maxX = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x - 0.1f;
            float maxY = Camera.main.ViewportToWorldPoint(new Vector2(1, 1)).y - 0.1f;
            Vector3[] positions = new Vector3[15];
            for (int j = 0; j < 15; j++) {
                positions[j] = new Vector3(Random.Range(minX, maxX), maxY, 0f);
            }
            for (int j = 0; j < 10; j++) {
                for (int k = 0; k < 15; k++) {
                    GameObject obj = objectPooler.SpawnFromPool("WaterWave", positions[k], Quaternion.Euler(0f, 0f, 180));
                    bool inverse = false;
                    if (k % 2 == 1)
                        inverse = true;
                    obj.GetComponent<WaterWave>().setDirection(Vector3.down, 8f, inverse, 9f, 0.8f);
                }
                yield return new WaitForSeconds(0.09f);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Phase2() {
        StartCoroutine(moveToLocal(fireOrb, fireOrb.transform.localPosition, new Vector3(0, 5, 0), 2.5f));
        StartCoroutine(moveToLocal(lightningOrb, lightningOrb.transform.localPosition, new Vector3(5, 0, 0), 2.5f));
        StartCoroutine(moveToLocal(waterOrb, waterOrb.transform.localPosition, new Vector3(0, -5, 0), 2.5f));
        StartCoroutine(moveToLocal(iceOrb, iceOrb.transform.localPosition, new Vector3(-5, 0, 0), 2.5f));
        yield return new WaitForSeconds(4.5f);
        while (true) {
            StartCoroutine(Attack6());
            yield return new WaitForSeconds(8f);
            StartCoroutine(IceOrbAttack());
            yield return new WaitForSeconds(22f);
            StartCoroutine(Attack6());
            yield return new WaitForSeconds(8f);
            StartCoroutine(LightningOrbAttack());
            yield return new WaitForSeconds(29.5f);
            StartCoroutine(Attack6());
            yield return new WaitForSeconds(8f);
            StartCoroutine(WaterOrbAttack());
            yield return new WaitForSeconds(32f);
            StartCoroutine(Attack6());
            yield return new WaitForSeconds(8f);
            StartCoroutine(FireOrbAttack());
            yield return new WaitForSeconds(24f);
        }
    }





    IEnumerator Shaggy2Death() {
        //GameObject timerObject = GameObject.Find("Timer");
        //timerObject.GetComponent<Timer>().enabled = false;
        //gameManager.GetComponent<GameManager>().setTime(decimal.Parse(timerObject.GetComponent<Text>().text));
        //gameManager.GetComponent<GameManager>().setTime(System.Math.Round((decimal)(Time.timeSinceLevelLoad), 2));
        yield return new WaitForSeconds(1f);
        GameObject exp1 = Instantiate(explosion);
        audioSource.PlayOneShot(explosion2Sound);
        exp1.transform.position = transform.position + new Vector3(0.8f, 0.2f, -2);
        Destroy(exp1, 2f);
        yield return new WaitForSeconds(0.5f);
        GameObject exp2 = Instantiate(explosion);
        audioSource.PlayOneShot(explosion2Sound);
        exp2.transform.position = transform.position + new Vector3(-1f, 0, -2);
        Destroy(exp2, 2f);
        yield return new WaitForSeconds(0.5f);
        GameObject exp3 = Instantiate(explosion);
        audioSource.PlayOneShot(explosion2Sound);
        exp3.transform.position = transform.position + new Vector3(0f, -2, -2);
        Destroy(exp3, 2f);
        yield return new WaitForSeconds(1.5f);
        GameObject exp4 = Instantiate(explosion);
        audioSource.PlayOneShot(explosion2Sound);
        exp4.transform.position = new Vector3(5f, -2f, -5);//transform.position + new Vector3(0, -3f, -5);
        exp4.transform.localScale = new Vector3(4f, 4f, 4f);
        yield return new WaitForSeconds(0.2f);
        gameManager.GetComponent<GameManager>().EndGame();
        //gameObject.SetActive(false);
        gameObject.transform.position = new Vector3(4.5f, 0.5f, 0);
        fireOrb.SetActive(false);
        lightningOrb.SetActive(false);
        waterOrb.SetActive(false);
        iceOrb.SetActive(false);
    }
}