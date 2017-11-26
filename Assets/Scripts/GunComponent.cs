using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class GunData : ScriptableObject {
    public bool automatic;
    public bool hasScope;
    public bool projectile;

    public int damage;
    public int currentAmmo;
    public int magazineAmmo;

    public float fireTime;
    public float reloadTime;

    public Texture idleTexture;
    public Texture fireTexture1;
    public Texture fireTexture2;
    public Texture fireTexture3;
    // reuse fire texture 1 for this frame
    public Texture reloadTexture;
}

public class GunComponent : MonoBehaviour {

    public GunData currentGun;

    private Timer fireTimer;
    private Timer reloadTimer;

    public enum GunState {
        Idle,
        Firing,
        Cooldown,
        Waiting,
        Reloading,
    }

    public GunState state;

    void Start(){
        fireTimer = new Timer();
        reloadTimer = new Timer();

        currentGun.currentAmmo = currentGun.magazineAmmo;
        SetCurrentGun(currentGun);
    }

    void Update(){
        if(state == GunState.Idle){
            if(Input.GetMouseButton(0) && currentGun.currentAmmo > 0){
                fireTimer.Start();
                state = GunState.Firing;
                currentGun.currentAmmo--;
                Debug.Log("BANG!");
            }
        } else if(state == GunState.Firing){
            if(fireTimer.Finished()){
                if(currentGun.automatic){
                    state = GunState.Idle;
                } else {
                    state = GunState.Waiting;
                }
            }
        } else if(state == GunState.Waiting){
            if(!Input.GetMouseButton(0)){
                state = GunState.Idle;
            }
        }

        if(state != GunState.Reloading && Input.GetKey("r") && currentGun.currentAmmo < currentGun.magazineAmmo){
            state = GunState.Reloading;
            reloadTimer.Start();
        } else if(state == GunState.Reloading && reloadTimer.Finished()){
            state = GunState.Idle;
            currentGun.currentAmmo = currentGun.magazineAmmo;
        }
    }

    void SetCurrentGun(GunData newgun){
        currentGun = newgun;

        fireTimer.SetDuration(newgun.fireTime);
        reloadTimer.SetDuration(newgun.reloadTime);
    }

    public bool CanScope(){
        return currentGun.hasScope;
    }
}
