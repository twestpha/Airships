﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaComponent : MonoBehaviour {

    public float speed;
    private Vector3 startPoint;

    public GameObject groundSplash;
    public GameObject waterSplash;

    private const int WaterLayer = 4;
    private const int GroundLayer = 10;

    private const float DespawnRange = 800.0f;

    void Start(){
        startPoint = transform.position;
    }

    void Update(){
        transform.position += transform.forward * speed * Time.deltaTime;

        if((transform.position - startPoint).magnitude > DespawnRange){
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.layer == WaterLayer && groundSplash){
            Object.Instantiate(groundSplash, other.contacts[0].point + new Vector3(0.0f, 6.0f, 0.0f), new Quaternion());
        } else if(other.gameObject.layer == GroundLayer && waterSplash){
            Object.Instantiate(waterSplash, other.contacts[0].point + new Vector3(0.0f, 6.0f, 0.0f), new Quaternion());
        }

        Destroy(gameObject);
    }
}
