﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationFlight : MonoBehaviour {

    public float speed = 1.0f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
        transform.position += transform.forward * speed * Time.deltaTime;
	}
}
