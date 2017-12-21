using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneGunComponent : MonoBehaviour {

    public AirplaneGunData currentGun;

    private Timer fireTimer;
    private Timer overlayTimer;

    public AudioSource audioSource;

    private LevelScriptBase levelScript;

    public bool gunsEnabled = true;
    public int bulletsfired;
    public bool overlayActive;

    void Start(){
        fireTimer = new Timer(currentGun.fireTime);
        overlayTimer = new Timer(0.05f);

        currentGun.currentAmmo = currentGun.startingAmmo;

        // audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        if(gunsEnabled && ((Input.GetMouseButton(0) && currentGun.automatic) || Input.GetMouseButtonDown(0)) && fireTimer.Finished() && currentGun.currentAmmo > 0){
            overlayTimer.Start();
            fireTimer.Start();

            overlayActive = true;

            audioSource.clip = currentGun.fireSound;
            audioSource.Play();

            // two spawn points, on both sides of airplane
            if(currentGun.useMachineGunSpawns){
                GameObject bullet1 = Object.Instantiate(currentGun.projectilePrefab, transform);
                GameObject bullet2 = Object.Instantiate(currentGun.projectilePrefab, transform);

                bullet1.transform.localPosition = new Vector3(1.0f, -0.25f, 0.0f);
                bullet2.transform.localPosition = new Vector3(-1.0f, -0.25f, 0.0f);

                bullet1.transform.parent = null;
                bullet2.transform.parent = null;

                float airspeed = GetComponent<AirplaneComponent>().airspeed;
                bullet1.GetComponent<BulletComponent>().speed += airspeed;
                bullet2.GetComponent<BulletComponent>().speed += airspeed;
            } else {

            }

            currentGun.currentAmmo--;
            bulletsfired++;
        }

        if(overlayTimer.FinishedThisFrame()){
            overlayActive = false;
        }
    }

}
