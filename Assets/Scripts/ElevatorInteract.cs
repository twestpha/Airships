using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorInteract : MonoBehaviour {

    public GameObject pistonDriver;

    public enum ElevatorState {
        Idle,
        Raising,
        Waiting,
        Lowering,
    }

    public ElevatorState state;
    private Timer raiseTimer;
    private Timer waitTimer;
    private Vector3 pistonRaised;
    private Vector3 pistonLowered;

    void Start(){
        raiseTimer = new Timer(2.0f);
        waitTimer = new Timer(2.0f);

        pistonRaised = new Vector3(0.0f, 0.0f, -1.0f);
        pistonLowered = new Vector3(0.0f, 0.0f, 1.0f);
    }

    void FixedUpdate(){
        if(state == ElevatorState.Idle){
            state = ElevatorState.Raising;
            raiseTimer.Start();
        } else if(state == ElevatorState.Raising){
            pistonDriver.transform.localPosition = Vector3.Lerp(pistonLowered, pistonRaised, raiseTimer.Parameterized());

            if(raiseTimer.Finished()){
                state = ElevatorState.Waiting;
                waitTimer.Start();
            }
        } else if(state == ElevatorState.Waiting){
            if(waitTimer.Finished()){
                state = ElevatorState.Lowering;
                raiseTimer.Start();
            }
        } else if(state == ElevatorState.Lowering){
            pistonDriver.transform.localPosition = Vector3.Lerp(pistonRaised, pistonLowered, raiseTimer.Parameterized());

            if(raiseTimer.Finished()){
                state = ElevatorState.Idle;
            }
        }
    }
}
