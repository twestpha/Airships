using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardComponent : MonoBehaviour {

    void Start(){
    }

    void Update(){
        Vector3 tocamera = Camera.main.transform.position - transform.position;
        transform.LookAt(transform.position + (-tocamera), Vector3.up);
    }
}
