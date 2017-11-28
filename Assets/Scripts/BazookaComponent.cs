using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaComponent : MonoBehaviour {

    public float speed;

    void Start(){

    }

    void Update(){
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
