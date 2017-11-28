using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunComponent : MonoBehaviour {

    public GunData currentGun;

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

                RaycastHit hit;
                if(Physics.Raycast(
                    transform.position,
                    transform.forward,
                    out hit,
                    currentGun.maxRange)
                ){
                    DamageableComponent damageable = hit.collider.gameObject.GetComponent<DamageableComponent>();
                    if(damageable){
                        damageable.Damage(currentGun.damage, currentGun.armorBonus);

                        // TODO make a different sprite based on tag of what we hit

                    }

                    GameObject.Instantiate(GenericGunHitSprite).transform.position = Vector3.Lerp(transform.position, hit.point, 0.95f);
                }

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
    }

    void SetCurrentGun(GunData newgun){
        currentGun = newgun;

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
