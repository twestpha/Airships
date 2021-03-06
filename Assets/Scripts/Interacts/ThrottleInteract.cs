﻿using System.Collections;
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
        if(!player){
            player = GameObject.FindWithTag("Player");
            return;
        }
        
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
            if(state == 0 /* Stop */){
                rotatable.SetSpriteSheet(stopsheet, true);
            } else if(state == 1 /* Slow Ahead */){
                rotatable.SetSpriteSheet(slowsheet, true);
            } else if(state == 2 /* Full Ahead */){
                rotatable.SetSpriteSheet(fullsheet, true);
            }
        }
    }
}
