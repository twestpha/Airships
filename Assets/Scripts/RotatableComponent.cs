using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableComponent : MonoBehaviour {

    public GameObject sprite;
    public bool mirror = true;
    public Material[] spriteSheet;

    private MeshRenderer spriteRenderer;
    private Vector3 originalScale;
    private float degreesPerSprite;

    void Start(){
        spriteRenderer = sprite.GetComponent<MeshRenderer>();
        originalScale = sprite.transform.localScale;

        if(mirror){
            degreesPerSprite = 180.0f / (float) (spriteSheet.Length - 1);
        } else {
            degreesPerSprite = 360.0f / (float) (spriteSheet.Length - 1);
        }
    }

    void Update(){
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

        int index = 0;

        angle += degreesPerSprite * 0.5f;
        index = (int)(angle / degreesPerSprite);

        spriteRenderer.material = spriteSheet[index];
    }
}
