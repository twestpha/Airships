using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GunComponent : NetworkBehaviour {

    public GunData currentGun;

    public GunData gunSlot1;
    public GunData gunSlot2;

    private Timer fireTimer;
    private Timer reloadTimer;

    public AudioClip reloadSoundA;
    public AudioClip reloadSoundB;
    public AudioClip emptyGunSound;

    private AudioSource audioSource;

    public GameObject GenericGunHitSprite;

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
        animationTimer = new Timer();

        gunSlot1.currentAmmo = gunSlot1.magazineAmmo;
        gunSlot2.currentAmmo = gunSlot2.magazineAmmo;

        SetCurrentGun(currentGun);

        audioSource = GetComponent<AudioSource>();
    }

    /*void Update(){
        if(Input.GetKeyDown("1")){
            SetCurrentGun(gunSlot1);
        } else if(Input.GetKeyDown("2")){
            SetCurrentGun(gunSlot2);
        }

        if(state == GunState.Idle){
            if(Input.GetMouseButton(0) && currentGun.currentAmmo > 0){
                fireTimer.Start();
                state = GunState.Firing;

                audioSource.clip = currentGun.fireSound;
                audioSource.Play();

                animstate = AnimationState.Firing;
                animationTimer.Start();

                GetComponent<FPSComponent>().CmdFireGun();

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

        if(state != GunState.Reloading && state != GunState.Firing && Input.GetKey("r") && currentGun.currentAmmo < currentGun.magazineAmmo){
            state = GunState.Reloading;
            reloadTimer.Start();

            audioSource.clip = reloadSoundA;
            audioSource.Play();
        } else if(state == GunState.Reloading && reloadTimer.Finished()){
            state = GunState.Idle;
            currentGun.currentAmmo = currentGun.magazineAmmo;

            audioSource.clip = reloadSoundB;
            audioSource.Play();
        }

        if(animstate == AnimationState.Firing && animationTimer.Finished()){
            animstate = AnimationState.Cocking;
            animationTimer.Start();
        } else if(animstate == AnimationState.Cocking && animationTimer.Finished()){
            animstate = AnimationState.Idle;
        }
    }*/

    void SetCurrentGun(GunData newgun){
        currentGun = newgun;

        state = GunState.Idle;
        animstate = AnimationState.Idle;

        fireTimer.SetDuration(newgun.fireTime);
        reloadTimer.SetDuration(newgun.reloadTime);

        animationTimer.SetDuration(newgun.fireTime /2.0f);
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
