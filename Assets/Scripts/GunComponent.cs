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
    public int armorBonus; // can be positive or negative
    public int currentAmmo;
    public int magazineAmmo;

    public float fireTime;
    public float reloadTime;
    public float maxRange;

    public Texture idleTexture;
    public Texture fireTexture1;
    public Texture fireTexture2;
    public Texture reloadTexture;

    // 16kbps/16000 samples
    public AudioClip fireSound;
}

public class GunComponent : MonoBehaviour {

    public GunData currentGun;

    private Timer fireTimer;
    private Timer reloadTimer;

    public AudioClip reloadSound;
    public AudioClip emptyGunSound;

    private AudioSource audioSource;

    public enum GunState {
        Idle,
        Firing,
        Cooldown,
        Waiting,
        Reloading,
    }

    private Timer animationTimer;
    public enum AnimationState {
        Idle,
        Firing,
        Cocking,
    }

    public GunState state;
    public AnimationState animstate;

    void Start(){
        fireTimer = new Timer();
        reloadTimer = new Timer();
        animationTimer = new Timer(0.12f);

        currentGun.currentAmmo = currentGun.magazineAmmo;
        SetCurrentGun(currentGun);

        audioSource = GetComponent<AudioSource>();
    }

    void Update(){
        if(state == GunState.Idle){
            if(Input.GetMouseButton(0) && currentGun.currentAmmo > 0){
                fireTimer.Start();
                state = GunState.Firing;

                audioSource.clip = currentGun.fireSound;
                audioSource.Play();

                animstate = AnimationState.Firing;
                animationTimer.Start();

                currentGun.currentAmmo--;
            }

            if(Input.GetMouseButtonDown(0) && currentGun.currentAmmo == 0 && !audioSource.isPlaying){
                audioSource.clip = emptyGunSound;
                audioSource.Play();
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

            audioSource.clip = reloadSound;
            audioSource.Play();
        } else if(state == GunState.Reloading && reloadTimer.Finished()){
            state = GunState.Idle;
            currentGun.currentAmmo = currentGun.magazineAmmo;
        }

        if(animstate == AnimationState.Firing && animationTimer.Finished()){
            animstate = AnimationState.Cocking;
            animationTimer.Start();
        } else if(animstate == AnimationState.Cocking && animationTimer.Finished()){
            animstate = AnimationState.Idle;
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

    public Texture GetCurrentGunTexture(){
        if(state == GunState.Reloading){
                return currentGun.reloadTexture;
        } else if(animstate == AnimationState.Idle){
            return currentGun.idleTexture;
        } else if(animstate == AnimationState.Firing){
            return currentGun.fireTexture1;
        } else {
            return currentGun.fireTexture2;
        }
    }
}
