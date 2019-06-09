using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : MonoBehaviour {
    
    private Vector3 direction;
    private float speed;

    public void setDirection(Vector3 dir, float spd) {
        direction = dir;
        speed = spd;
    }

    private void Update() {
        transform.position = transform.position + (speed * Time.deltaTime * direction);
    }



}
