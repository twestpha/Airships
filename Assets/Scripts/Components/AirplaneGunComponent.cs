using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneGunComponent : MonoBehaviour {

    public AirplaneGunData currentGun;

    private Timer fireTimer;
    private Timer overlayTimer;

    private AudioSource audioSource;
    private Texture defaultOverlay;

    public GameObject playerCameraObject;
    private VehicleCameraComponent airplaneCamera;

    private LevelScriptBase levelScript;

    public int bulletsfired;

    void Start(){
        fireTimer = new Timer(currentGun.fireTime);
        overlayTimer = new Timer(0.05f);

        currentGun.currentAmmo = currentGun.startingAmmo;

        airplaneCamera = playerCameraObject.GetComponent<VehicleCameraComponent>();
        defaultOverlay = airplaneCamera.overlay;

        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        if(((Input.GetMouseButton(0) && currentGun.automatic) || Input.GetMouseButtonDown(0)) && fireTimer.Finished() && currentGun.currentAmmo > 0){
            overlayTimer.Start();
            fireTimer.Start();

            audioSource.clip = currentGun.fireSound;
            audioSource.Play();

            airplaneCamera.overlay = currentGun.firingOverlay;

            // two spawn points, on both sides of airplane
            if(currentGun.useMachineGunSpawns){
                GameObject bullet1 = Object.Instantiate(currentGun.projectilePrefab, transform);
                GameObject bullet2 = Object.Instantiate(currentGun.projectilePrefab, transform);

                bullet1.transform.localPosition = new Vector3(1.0f, -0.25f, 0.0f);
                bullet2.transform.localPosition = new Vector3(-1.0f, -0.25f, 0.0f);

                bullet1.transform.parent = null;
                bullet2.transform.parent = null;

                float airspeed = GetComponent<AirplaneComponent>().airspeed;
                bullet1.GetComponent<BazookaComponent>().speed += airspeed;
                bullet2.GetComponent<BazookaComponent>().speed += airspeed;
            } else {

            }

            currentGun.currentAmmo--;
            bulletsfired++;
        }

        if(overlayTimer.FinishedThisFrame()){
            airplaneCamera.overlay = defaultOverlay;
        }
    }

}
