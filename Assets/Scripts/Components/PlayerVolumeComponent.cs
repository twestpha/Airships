using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVolumeComponent : MonoBehaviour {

    public bool playerInVolume;

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            playerInVolume = true;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.tag == "Player"){
            playerInVolume = false;
        }
    }
}
