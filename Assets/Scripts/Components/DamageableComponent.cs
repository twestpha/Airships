﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DamageableComponent : MonoBehaviour {

    public const int DamageableLayer = 1 << 8;
    public const int DamagingLayer = 1 << 10;

    public float health;
    public float startHealth;
    public bool armored;

    public Material destroyedMaterial;
    public bool hasTeam;
    public AirplaneComponent airplane;

    void Start(){
        Assert.IsTrue(gameObject.layer == 8, "Damageable component on gameobject " + gameObject + " is layer " + gameObject.layer + " when it should be " + DamageableLayer);

        airplane = GetComponent<AirplaneComponent>();
        hasTeam = airplane != null;

        startHealth = health;
    }

    void Update(){

    }

    public void Damage(float damage, float armorBonus){
        if(armored){
            health -= (damage + armorBonus);
        } else {
            health -= damage;
        }

        // Debug.Log("HIT FOR " + damage + " DAMAGE!");

        if(destroyedMaterial && health <= 0.0f){
            GetComponent<MeshRenderer>().material = destroyedMaterial;
        }
    }
}
