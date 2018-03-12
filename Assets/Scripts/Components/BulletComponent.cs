using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletComponent : MonoBehaviour {

    public float speed;
    private Vector3 startPoint;

    public float damage;
    public float armorBonus;

    public GameObject groundSplash;
    public GameObject waterSplash;

    private const int WaterLayer = 4;
    private const int DamagableLayer = 8;
    private const int GroundLayer = 10;

    public float DespawnRange = 800.0f;

    public AirplaneComponent firedFromAirplane;

    public GameObject despawnExplosion;

    public bool randomMode;

    void Start(){
        startPoint = transform.position;

        if(randomMode){
            DespawnRange -= Random.value * 500.0f;
        }
    }

    void Update(){
        transform.position += transform.forward * speed * Time.deltaTime;

        if((transform.position - startPoint).magnitude > DespawnRange){
            if(despawnExplosion){
                GameObject exp = GameObject.Instantiate(despawnExplosion);
                exp.transform.position = transform.position;
            }

            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other){
        if(other.gameObject.layer == DamagableLayer){
            // TODO add generic hit marker splash

            DamageableComponent damageable = other.gameObject.GetComponent<DamageableComponent>();

            // if(damageable && !damageable.hasTeam || damageable.airplane.team != firedFromAirplane.team){
            if(damageable){
                damageable.Damage(damage, armorBonus);
            }
            // }
        } else if(other.gameObject.layer == WaterLayer && groundSplash){
            Object.Instantiate(groundSplash, other.contacts[0].point + new Vector3(0.0f, 6.0f, 0.0f), new Quaternion());
        } else if(other.gameObject.layer == GroundLayer && waterSplash){
            Object.Instantiate(waterSplash, other.contacts[0].point + new Vector3(0.0f, 6.0f, 0.0f), new Quaternion());
        }

        Destroy(gameObject);
    }
}
