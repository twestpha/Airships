using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardComponent : MonoBehaviour {

    void Start(){
    }

    void Update(){
        // transform.forward = Camera.main.transform.forward;

        Vector3 tocamera = Camera.main.transform.position - transform.position;
        tocamera.y = 0.0f;
        transform.LookAt(transform.position + (-tocamera), Vector3.up);
    }
}
