using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonComponent : MonoBehaviour {
    // rise slowly until killed, then fall slowly
    public float riseRate;
    public float fallRate;
    private bool alive = true;
    private DamageableComponent damageable;

    void Start(){
        damageable = GetComponent<DamageableComponent>();
    }

    void Update(){
        bool prevalive = alive;
        alive = damageable.health > 0.0f;

        if(alive){
            transform.position += new Vector3(0.0f, riseRate * Time.deltaTime, 0.0f);
        }
        else {
            transform.position -= new Vector3(0.0f, fallRate * Time.deltaTime, 0.0f);
            if(transform.position.y < 0.0f){
                Destroy(gameObject);
            }
        }

        if(!alive && prevalive){
            GameObject.FindWithTag("Level Script").GetComponent<LevelScriptBase>().balloonkills++;
        }
    }
}
