using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirshipComponent : MonoBehaviour {

    public float speed;
    public float turnSpeed;

    public GameObject throttleInteract;
    private ThrottleInteract throttle;

    public GameObject wheelInteract;
    private WheelInteract wheel;

    void Start(){
        throttle = throttleInteract.GetComponent<ThrottleInteract>();
        wheel = wheelInteract.GetComponent<WheelInteract>();
    }

    void Update(){
        float speedMultiplier = 1.0f;

        if(throttle.state == 0){
            speedMultiplier = 0.0f;
        } else if(throttle.state == 1){
            speedMultiplier = 0.5f;
        } else if(throttle.state == 2){
            speedMultiplier = 1.0f;
        }

        transform.position += transform.forward * Time.deltaTime * speed * speedMultiplier;

        float turnMultiplier = 0.0f;

        if(wheel.state == 0){
            turnMultiplier = -1.0f;
        } else if(wheel.state == 1){
            turnMultiplier = 0.0f;
        } else if(wheel.state == 2){
            turnMultiplier = 1.0f;
        }

        transform.rotation *= Quaternion.Euler(0.0f, turnSpeed * Time.deltaTime * turnMultiplier * speedMultiplier, 0.0f);
    }
}
