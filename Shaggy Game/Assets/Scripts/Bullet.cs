using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;

    private void FixedUpdate() {
        transform.position = transform.position + new Vector3(speed * Time.deltaTime, 0, 0);
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        if (transform.position.x > max.x) {
            Destroy(gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Shaggy2")) {
            other.gameObject.GetComponentInParent<Shaggy2>().ReduceHealth();
            Destroy(gameObject);
        }
        if (other.CompareTag("Shaggy1")) {
            other.gameObject.GetComponentInParent<Shaggy>().ReduceHealth();
            Destroy(gameObject);
        }
    }
}
