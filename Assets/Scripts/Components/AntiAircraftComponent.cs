using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiAircraftComponent : MonoBehaviour {

    public DamageableComponent damageable;

    public GameObject burningPrefab;
    public GameObject scrapsPrefab;

	void Start(){

	}

	void Update(){
        if(damageable.health < 0){
            if(burningPrefab){
                Instantiate(burningPrefab, transform.position, Quaternion.Euler(-90.0f, 0, 0));
            }
            if(scrapsPrefab){
                Instantiate(scrapsPrefab, transform.position, Quaternion.Euler(-90.0f, 0, 0));
            }

            GameObject.FindWithTag("Level Script").GetComponent<LevelScriptBase>().vehiclekills++;

            Destroy(gameObject);
        }
	}
}
