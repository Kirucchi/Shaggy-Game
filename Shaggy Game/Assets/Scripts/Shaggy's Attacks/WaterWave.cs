using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWave : MonoBehaviour {

    private Vector3 direction;
    private Vector3 oldPos;
    private Vector3 newPos;
    private Vector3 parallel;
    private float speed;
    private float frequency = 4f;
    private float startTime;
    private float amplitude = 1.3f;

    private bool inverse = false;

    Vector2 max;
    Vector2 min;

    public void setDirection(Vector3 dir, float spd, bool isInverse, float frequency, float amplitude) {
        direction = dir;
        speed = spd;
        startTime = Time.time;
        parallel = transform.position;
        inverse = isInverse;
        this.frequency = frequency;
        this.amplitude = amplitude;
    }

    private void Awake() {
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
    }

    void Update () {
        oldPos = transform.position;
        parallel += direction * Time.deltaTime * speed;
        Vector3 perpendicular = Vector3.zero;
        if (!inverse)
            perpendicular = Quaternion.Euler(0f, 0f, -90f) * direction * Mathf.Sin((Time.time - startTime) * frequency) * amplitude;
        else
            perpendicular = Quaternion.Euler(0f, 0f, -90f) * direction * Mathf.Sin((Time.time - startTime) * frequency) * -amplitude;
        transform.position = parallel + perpendicular;
        newPos = transform.position;
        float angle = Mathf.Atan2((newPos - oldPos).y, (newPos - oldPos).x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0f, 0f, angle - 90);
        if (transform.position.x > max.x + 3f || transform.position.x < min.x - 3f || transform.position.y > max.y + 1f|| transform.position.y < min.y - 1f) {
            gameObject.SetActive(false);
        }
    }
}
