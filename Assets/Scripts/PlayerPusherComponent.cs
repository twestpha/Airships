using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPusherComponent : MonoBehaviour {

    private GameObject player;

    public GameObject airship;

    void Start(){
        player = GameObject.FindWithTag("Player");
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            player.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            player.transform.parent = airship.transform;
        }
    }
}
