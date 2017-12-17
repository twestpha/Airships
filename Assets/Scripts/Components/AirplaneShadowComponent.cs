using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneShadowComponent : MonoBehaviour {

    private GameObject player;

    void Start(){
         transform.parent = null;

         player = GameObject.FindWithTag("Player");
    }

    void Update(){
        Vector3 newPosition = player.transform.position;
        newPosition.y = 0.01f;

        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(90.0f, 0.0f, -player.transform.rotation.eulerAngles.y - 90.0f);
    }
}
