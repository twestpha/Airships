using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class RotatableComponent : MonoBehaviour {

    public GameObject sprite;
    public bool mirror = true;
    public Material[] spriteSheet;

    private MeshRenderer spriteRenderer;
    private Vector3 originalScale;
    public float degreesPerSprite;

    public GameObject fpsObject;
    private FPSComponent fpscontroller;

    void Start(){
        fpscontroller = GetComponent<FPSComponent>();
        if(fpscontroller && fpscontroller.isLocalPlayer){
            return;
        }

        spriteRenderer = sprite.GetComponent<MeshRenderer>();
        originalScale = sprite.transform.localScale;

        if(mirror){
            degreesPerSprite = 180.0f / (float) (spriteSheet.Length - 1);
            //                                   ^ Because of extra sprite at end
        } else {
            degreesPerSprite = 360.0f / (float) (spriteSheet.Length);
        }
    }

    void Update(){
        if(fpscontroller && fpscontroller.isLocalPlayer){
            return;
        }

        Vector3 camerapos = Camera.main.transform.position;
        Vector3 toCamera = camerapos - transform.position;

        toCamera.y = 0.0f;
        toCamera.Normalize();

        float cross = Vector3.Cross(toCamera, transform.forward).y;
        float angle = Mathf.Acos(Vector3.Dot(toCamera, transform.forward));
        angle *= Mathf.Rad2Deg;

        if(cross > 0.0f && mirror){
            Vector3 newscale = originalScale;
            newscale.x *= -1.0f;
            sprite.transform.localScale = newscale;
        } else {
            sprite.transform.localScale = originalScale;
        }

        if(cross > 0.0f && !mirror){
            // transform into range (0, 360)
            angle = 180.0f + (180.0f - angle);
        }

        angle += degreesPerSprite * 0.5f;

        int index = (int)(angle / degreesPerSprite);

        if(index > spriteSheet.Length - 1 || index < 0){
            index = 0;
        }

        // Debug.Log(index + " " + angle);
        spriteRenderer.material = spriteSheet[index];
    }

    public void SetSpriteSheet(Material[] newSpriteSheet, bool mirror){
        spriteSheet = newSpriteSheet;

        if(mirror){
            degreesPerSprite = 180.0f / (float) (spriteSheet.Length - 1);
        } else {
            degreesPerSprite = 360.0f / (float) (spriteSheet.Length);
        }
    }
}
