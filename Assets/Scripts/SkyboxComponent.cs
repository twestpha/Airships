using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxComponent : MonoBehaviour {

    private GameObject player;

	void Start () {
        player = GameObject.FindWithTag("Player");
	}

	void LateUpdate () {
        transform.position = player.transform.position;
	}
}
