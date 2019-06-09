using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbsRotation : MonoBehaviour {
    /*
    public GameObject fireOrb;
    public GameObject lightningOrb;
    public GameObject waterOrb;
    public GameObject iceOrb;
    
    
    private Vector3 originalSize;
    */
    
	void Start () {
        //originalSize = fireOrb.transform.localScale;
	}
	
	void Update () {
        transform.Rotate(Vector3.forward * Time.deltaTime * 100);
        /*
        if (Input.GetKeyDown(KeyCode.N)) {
            StartCoroutine(SpawnFireOrb());
            StartCoroutine(SpawnLightningOrb());
            StartCoroutine(SpawnWaterOrb());
            StartCoroutine(SpawnIceOrb());
        }
        */
	}
    /*
    public IEnumerator SpawnFireOrb() {
        fireOrb.transform.localScale = new Vector3(0f, 0f, 0f);
        fireOrb.transform.localPosition = new Vector3(0f, 0f, 0f);
        for (int i=1; i<=100; i++) {
            fireOrb.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), originalSize, 0.01f * i);
            fireOrb.transform.localPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(0f, 5f, 0f), 0.01f * i);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator SpawnLightningOrb() {
        lightningOrb.transform.localScale = new Vector3(0f, 0f, 0f);
        lightningOrb.transform.localPosition = new Vector3(0f, 0f, 0f);
        for (int i = 1; i <= 100; i++) {
            lightningOrb.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), originalSize, 0.01f * i);
            lightningOrb.transform.localPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(5f, 0f, 0f), 0.01f * i);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator SpawnWaterOrb() {
        waterOrb.transform.localScale = new Vector3(0f, 0f, 0f);
        waterOrb.transform.localPosition = new Vector3(0f, 0f, 0f);
        for (int i = 1; i <= 100; i++) {
            waterOrb.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), originalSize, 0.01f * i);
            waterOrb.transform.localPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(0f, -5f, 0f), 0.01f * i);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public IEnumerator SpawnIceOrb() {
        iceOrb.transform.localScale = new Vector3(0f, 0f, 0f);
        iceOrb.transform.localPosition = new Vector3(0f, 0f, 0f);
        for (int i = 1; i <= 100; i++) {
            iceOrb.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), originalSize, 0.01f * i);
            iceOrb.transform.localPosition = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(-5f, 0f, 0f), 0.01f * i);
            yield return new WaitForSeconds(0.01f);
        }
    }
    */
}
