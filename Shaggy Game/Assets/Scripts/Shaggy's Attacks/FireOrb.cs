using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOrb : MonoBehaviour {
    
	void Start () {
		
	}
	
	void Update () {
        transform.Rotate(Vector3.forward * Time.deltaTime * 45f);
    }
}
