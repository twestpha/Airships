using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneComponent : MonoBehaviour {

    private Rigidbody body;

    public float maxSpeed;
    public float minSpeed;
    public float throttle;
    public float throttleChangeRate;

    public float stabilizationCoeffecient;

    public float aileronForce;
    private float aileronCurrent;
    private float aileronTarget;
    private float aileronVelocity;

    public float elevatorForce;
    private float elevatorCurrent;

    public float rudderForce;
    public float rudderCurrent;

    public bool landingGearOut;

    [Header("Debug Instruments")]
    public float airspeed;
    public float altitude;
    public Vector3 velocity;
    private float speed;
    private float acceleration;
    public Vector3 angularVelocity;

    // The notion is not that it's a simulation, but an interactive
    // system. This is where we prove out the resources the player
    // has to juggle while pushing the aircraft to the limit.

    // Feel free to pare these gameplay elements if they don't work out.
    [Header("Optionals")]
    public float engineTemperature;
    public float fuel;

    public int enginesRunning;
    public int enginesMax;

    // Temporary line-drawing vars so I can see the plane's flight path
    private Timer lineTimer;
    private Vector3 prevPosition;

    void Start(){
        body = GetComponent<Rigidbody>();
        lineTimer = new Timer(5.0f);
        lineTimer.Start();

        landingGearOut = true;
    }

    void FixedUpdate(){
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

        throttle = Mathf.Min(Mathf.Max(0.0f, throttle), 1.2f);

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

        // Takeoff and landing
        float minSpeedWithGear = minSpeed;
        if(landingGearOut && transform.position.y < 3.0f){
             minSpeedWithGear = 0.0f;
             aileronCurrent = 0.0f;

             elevatorCurrent *= throttle;
             elevatorCurrent = Mathf.Min(0.0f, elevatorCurrent);

             rudderCurrent *= throttle;

             if(transform.position.y < 0.5f){
                 transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
             }
        }

        // Position calculations
        speed = Mathf.SmoothDamp(speed, throttle * maxSpeed + minSpeedWithGear, ref acceleration, 5.0f);
        velocity = transform.forward * speed;

        // Instruments
        transform.position += velocity * Time.deltaTime;
        airspeed = velocity.magnitude;
        altitude = transform.position.y;

        // Rotation calculations
        float yaw = rudderCurrent * rudderForce * -1.0f;
        float pitch = elevatorCurrent * elevatorForce;
        float roll = aileronCurrent * aileronForce * -1.0f;
        Vector3 eulerAngles = new Vector3(pitch, yaw, roll);
        eulerAngles *= Time.deltaTime;

        transform.rotation *= Quaternion.Euler(eulerAngles);
    }
}
