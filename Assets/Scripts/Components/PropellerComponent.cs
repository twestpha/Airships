using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerComponent : MonoBehaviour {

    public float angularVelocity;

    void Start(){

    }

    void Update(){
        transform.rotation *= Quaternion.Euler(0.0f, 0.0f, angularVelocity * Time.deltaTime);
    }
}
