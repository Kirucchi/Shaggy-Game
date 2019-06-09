using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralBullet : MonoBehaviour {

    Vector2 max;
    Vector2 min;

	// Use this for initialization
	void Start () {
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
	}
	
	void Update () {
		if (transform.position.x > max.x || transform.position.x < min.x || transform.position.y > max.y || transform.position.y < min.y) {
            gameObject.SetActive(false);
        }
	}
    
}
