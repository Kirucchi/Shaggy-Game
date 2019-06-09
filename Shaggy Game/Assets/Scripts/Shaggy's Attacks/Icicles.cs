using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Icicles : MonoBehaviour {

    private Vector3 target = Vector3.zero;
    private float time = 0.5f;
    private Vector3 velocity = Vector3.zero;
    private float speed = 7;
    private float timer = 0f;

    public void setTarget(Vector3 newTarget) {
        timer = 0f;
        target = newTarget;
        transform.eulerAngles = new Vector3(0f, 0f, Mathf.Atan((target.y - transform.position.y) / (target.x - transform.position.x)) * Mathf.Rad2Deg - 90f);
    }

    private void Start() {
    }

    private void Update() {
        if (timer < 2f)
            transform.position = Vector3.SmoothDamp(transform.position, target, ref velocity, time);
        else {
            transform.position = transform.position + Vector3.left * Time.deltaTime * speed;
            transform.eulerAngles = new Vector3(0f, 0f, 90);
        }
        timer += Time.deltaTime;
    }
}
