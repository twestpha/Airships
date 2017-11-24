using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrottleInteract : MonoBehaviour {

    public int state;
    // 0 = Stop
    // 1 = Slow Ahead
    // 2 = Full Ahead

    private GameObject player;

    public Material[] stopsheet;
    public Material[] slowsheet;
    public Material[] fullsheet;

    private RotatableComponent rotatable;

    void Start(){
        player = GameObject.FindWithTag("Player");
        rotatable = GetComponent<RotatableComponent>();
    }

    void Update(){
        int previousstate = state;

        if((transform.position - player.transform.position).magnitude < 1.5f){
            if(Input.GetKeyDown("e")){
                state++;
            } else if(Input.GetKeyDown("q")){
                state--;
            }

            state = Mathf.Min(Mathf.Max(state, 0), 2);
        }

        if(previousstate != state){
            // TODO notify airship of change of speed

            if(state == 0 /* Stop */){
                rotatable.spriteSheet = stopsheet;
            } else if(state == 1 /* Slow Ahead */){
                rotatable.spriteSheet = slowsheet;
            } else if(state == 2 /* Full Ahead */){
                rotatable.spriteSheet = fullsheet;
            }
        }
    }
}
