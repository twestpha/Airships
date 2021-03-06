﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneComponent : MonoBehaviour {

    public enum AirplaneType {
        Interceptor,
        Bomber,
        Boss,
    }
    public enum AirplaneTeam {
        Ally,
        Enemy,
    }
    public enum AirplaneBehavior {
        Simple,
        Engaging,
        Custom,
    }
    [Header("Types")]
    public AirplaneType type;
    public AirplaneTeam team;
    public AirplaneBehavior behavior;

    [Header("Speed")]
    public float maxSpeed;
    public float minSpeed;

    [Header("Throttle Controls")]
    public bool throttleEnabled;
    public float throttle;
    public float throttleChangeRate;

    [Header("Aileron Controls")]
    public float aileronForce;
    private float aileronCurrent;
    private float aileronTarget;
    private float aileronVelocity;

    [Header("Elevator Controls")]
    public float elevatorForce;
    private float elevatorCurrent;

    [Header("Rudder Controls")]
    public float rudderForce;
    public float rudderCurrent;

    public bool landingGearOut;

    [Header("Instruments")]
    public float airspeed;
    public float altitude;
    public float heading;
    public Vector3 velocity;
    private float speed;
    private float acceleration;
    public Vector3 angularVelocity;

    [Header("Destruction")]
    public bool crashed = false;
    private Timer deathTimer;
    private Timer forceDownTimer;
    private float previousHealth;

    public GameObject firePrefab;
    public GameObject firePrefabInstance;
    public Vector3 firePosition;
    public GameObject burningPrefab;
    public GameObject scrapsPrefab;

    // The notion is not that it's a simulation, but an interactive
    // system. This is where we prove out the resources the player
    // has to juggle while pushing the aircraft to the limit.

    // Feel free to pare these gameplay elements if they don't work out.
    [Header("Optionals")]
    public float engineTemperature;
    public float engineTemperatureRate;

    public int enginesRunning;
    public int enginesMax;

    [Header("Sound Effects")]
    public AudioSource engineSource;
    public AudioClip engineSound;
    public AudioSource hitSource;
    public AudioClip hitSound;

    // Temporary line-drawing vars so I can see the plane's flight path
    private Timer lineTimer;
    private Vector3 prevPosition;

    private bool isPlayer;
    private bool destroyed;

    void Start(){
        isPlayer = tag == "Player";

        if(engineSound){
            engineSource.clip = engineSound;
            engineSource.Play();
        }

        if(isPlayer){
            lineTimer = new Timer(5.0f);
            lineTimer.Start();

            deathTimer = new Timer(10.0f);
        }

        forceDownTimer = new Timer(5.0f);

        landingGearOut = true;
    }

    void FixedUpdate(){
        if(!destroyed){
            if(isPlayer){
                HandlePlayerInput();
            } else {
                switch(behavior){
                case AirplaneBehavior.Simple:
                    // Nothing, just apply velocity
                break;
                }
            }

            ApplyDamage();
        } else {
            aileronCurrent = -1.0f * forceDownTimer.Parameterized();
            elevatorCurrent = 0.5f * forceDownTimer.Parameterized();
            rudderCurrent = 0.5f * forceDownTimer.Parameterized();
        }

        if(transform.position.y < 0.0f && !crashed){
            Instantiate(burningPrefab, transform.position, Quaternion.Euler(-90.0f, 0, 0));
            Instantiate(scrapsPrefab, transform.position, Quaternion.Euler(-90.0f, 0, 0));

            if(firePrefabInstance){
                Destroy(firePrefabInstance);
            }

            crashed = true;

            engineSource.Stop();

            if(!isPlayer){
                Destroy(gameObject);
            } else {
                GameObject.FindWithTag("MainCamera").GetComponent<VehicleCameraComponent>().deathMode = true;
                GameObject.FindWithTag("MainCamera").transform.rotation = Quaternion.Euler(70.0f, 0.0f, 0.0f);
                deathTimer.Start();

                GameObject.FindWithTag("Level Script").GetComponent<LevelScriptBase>().StopProgression();
            }
        }

        if(!crashed){
            ApplyVelocity();
        } else if(isPlayer) {
            HandleCrashCamera();

            if(deathTimer.Finished()){
                GameObject.FindWithTag("Level Script").GetComponent<LevelScriptBase>().LoadLastSave_();
                // GameObject.FindWithTag("MainCamera").transform.position = new Vector3(0.0f, 0.0f, 0.0f);
                // GameObject.FindWithTag("MainCamera").transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                crashed = false;
                destroyed = false;
            }
        }
    }

    void HandleCrashCamera(){
        GameObject.FindWithTag("MainCamera").transform.position = new Vector3(transform.position.x, deathTimer.Parameterized() * 100.0f + 100.0f, transform.position.z - 50.0f);
    }

    void HandlePlayerInput(){
        if(lineTimer.Finished()){
            Debug.DrawLine(transform.position, prevPosition, Color.red, 10000.0f);
            prevPosition = transform.position;
            lineTimer.Start();
        }

        // Throttle input
        if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey("w")){
            throttle += throttleChangeRate * Time.deltaTime;
        } else if(Input.GetKey(KeyCode.LeftControl) || Input.GetKey("s")){
            throttle -= throttleChangeRate * Time.deltaTime;
        }

        throttle = Mathf.Min(Mathf.Max(0.0f, throttle), 1.0f);

        // Rudder input
        if(Input.GetKey("q")){
            aileronTarget = -1.0f;
        } else if(Input.GetKey("e")){
            aileronTarget = 1.0f;
        } else {
            aileronTarget = 0.0f;
        }

        aileronCurrent = Mathf.SmoothDamp(aileronCurrent, aileronTarget, ref aileronVelocity, 0.2f);

        // Elevator input
        elevatorCurrent = 1.0f - ((Input.mousePosition.y / Screen.height) * 2.0f);

        // Rudder input
        rudderCurrent = 1.0f - ((Input.mousePosition.x / Screen.width) * 2.0f);

        elevatorCurrent = Mathf.Clamp(elevatorCurrent, -1.0f, 1.0f);
        rudderCurrent = Mathf.Clamp(rudderCurrent, -1.0f, 1.0f);

        // Landing gear input
        if(Input.GetKeyDown("g")){
            landingGearOut = !landingGearOut;
        }
    }

    void ApplyVelocity(){
        // Takeoff and landing
        float minSpeedWithGear = minSpeed;
        if(landingGearOut && transform.position.y < 3.0f && !destroyed){
             minSpeedWithGear = 0.0f;
             aileronCurrent = 0.0f;

             elevatorCurrent *= throttle;
             elevatorCurrent = Mathf.Min(0.0f, elevatorCurrent);

             rudderCurrent *= throttle;

             if(transform.position.y < 0.5f){
                 transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
             }
        }

        // Overheating
        engineTemperature += engineTemperatureRate * (throttle - 0.5f) * Time.deltaTime;
        engineTemperature = Mathf.Max(0.0f, engineTemperature);

        // Position calculations
        if(throttleEnabled){
            speed = Mathf.SmoothDamp(speed, (throttle * maxSpeed) + minSpeedWithGear, ref acceleration, 5.0f);
            velocity = transform.forward * speed;
        }

        if(destroyed && !crashed){
            velocity.y = -1000.0f * Time.deltaTime * forceDownTimer.Parameterized();
        }

        transform.position += velocity * Time.deltaTime;

        // Instruments
        airspeed = velocity.magnitude;
        altitude = transform.position.y;
        heading = transform.rotation.eulerAngles.y;

        // Rotation calculations
        float yaw = rudderCurrent * rudderForce * -1.0f;
        float pitch = elevatorCurrent * elevatorForce;
        float roll = aileronCurrent * aileronForce * -1.0f;
        Vector3 eulerAngles = new Vector3(pitch, yaw, roll);
        eulerAngles *= Time.deltaTime;

        transform.rotation *= Quaternion.Euler(eulerAngles);

        // throttle Sound
        engineSource.volume = 0.5f + (throttle / 2.0f);
        engineSource.pitch = 0.5f + (throttle / 2.0f);
    }

    void ApplyDamage(){
        bool prevdestroyed = destroyed;

        float currentHealth = GetComponent<DamageableComponent>().health;
        destroyed = currentHealth <= 0.0f;

        if(currentHealth < previousHealth && isPlayer && currentHealth > 0.0f){
            hitSource.clip = hitSound;
            hitSource.pitch = 0.95f + (Random.value * 0.1f);
            hitSource.Play();
        }

        if(!prevdestroyed && destroyed){
            firePrefabInstance = Instantiate(firePrefab);
            firePrefabInstance.transform.parent = transform;
            firePrefabInstance.transform.localPosition = firePosition;

            forceDownTimer.Start();

            if(team == AirplaneTeam.Enemy){
                LevelScriptBase levelscript = GameObject.FindWithTag("Level Script").GetComponent<LevelScriptBase>();

                switch(type){
                case AirplaneType.Interceptor:
                    levelscript.interceptorkills++;
                break;
                case AirplaneType.Bomber:
                    levelscript.bomberkills++;
                break;
                case AirplaneType.Boss:
                    levelscript.bosskills++;
                break;
                }
            }
        }

        previousHealth = currentHealth;
    }

}
