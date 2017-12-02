﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Networking;

public class DamageableComponent : NetworkBehaviour {

    public const int DamageableLayer = 1 << 8;

    [SyncVar]
    public float health;
    public bool armored;

    public Material destroyedMaterial;

    void Start(){
        Assert.IsTrue(gameObject.layer == 8, "Damageable component on gameobject " + gameObject + " is layer " + gameObject.layer + " when it should be " + DamageableLayer);
    }

    void Update(){

    }

    public void Damage(float damage, float armorBonus){
        if(armored){
            health -= (damage + armorBonus);
        } else {
            health -= damage;
        }

        Debug.Log("HIT FOR " + damage + " DAMAGE!");

        if(destroyedMaterial && health < 0.0f){
            GetComponent<MeshRenderer>().material = destroyedMaterial;
        }
    }
}
