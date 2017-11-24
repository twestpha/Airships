using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelInteract : MonoBehaviour {

    public int state;
    // 0 = Port
    // 1 = Fore
    // 2 = Starboard

    private GameObject player;

    public Material[] portsheet;
    public Material[] foresheet;
    public Material[] starboardsheet;

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
            // TODO notify airship of change of heading

            if(state == 0 /* Port */){
                rotatable.spriteSheet = starboardsheet;
            } else if(state == 1 /* Fore */){
                rotatable.spriteSheet = foresheet;
            } else if(state == 2 /* Starboard */){
                rotatable.spriteSheet = portsheet;
            }
        }
    }
}
