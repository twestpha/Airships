using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableComponent : MonoBehaviour {

    public GameObject sprite;
    private MeshRenderer spriterenderer;
    public Material tex0;
    public Material tex1;
    public Material tex2;
    public Material tex3;
    public Material tex4;
    public Material tex5;
    public Material tex6;
    public Material tex7;
    public Material tex8;
    public Material tex9;
    public Material tex10;
    public Material tex11;
    public Material tex12;
    private Vector3 originalscale;

    void Start(){
        spriterenderer = sprite.GetComponent<MeshRenderer>();
        originalscale = sprite.transform.localScale;
    }

    void Update(){
        Vector3 camerapos = Camera.main.transform.position;
        Vector3 toCamera = camerapos - transform.position;

        toCamera.y = 0.0f;
        toCamera.Normalize();

        float cross = Vector3.Cross(toCamera, transform.forward).y;
        float angle = Mathf.Acos(Vector3.Dot(toCamera, transform.forward));
        angle *= Mathf.Rad2Deg;

        if(cross > 0.0f){
            // angle *= -1.0f;
            Vector3 newscale = originalscale;
            newscale.x *= -1.0f;
            sprite.transform.localScale = newscale;
        } else {
            sprite.transform.localScale = originalscale;
        }

        // I know this is bad... but it works...
        if(angle >= -7.5f && angle < 7.5f){
            spriterenderer.material = tex0;
        } else if (angle >= 7.5f && angle < 22.5f){
            spriterenderer.material = tex1;
        } else if (angle >= 22.5f && angle < 37.5f){
            spriterenderer.material = tex2;
        } else if (angle >= 37.5f && angle < 52.5f){
            spriterenderer.material = tex3;
        } else if (angle >= 52.5f && angle < 67.5f){
            spriterenderer.material = tex4;
        } else if (angle >= 67.5f && angle < 82.5){
            spriterenderer.material = tex5;
        } else if (angle >= 82.5 && angle < 97.5f){
            spriterenderer.material = tex6;
        } else if (angle >= 97.5f && angle < 112.5f){
            spriterenderer.material = tex7;
        } else if (angle >= 112.5f && angle < 127.5){
            spriterenderer.material = tex8;
        } else if (angle >= 127.5 && angle < 142.5){
            spriterenderer.material = tex9;
        } else if (angle >= 142.5f && angle < 157.5){
            spriterenderer.material = tex10;
        } else if (angle >= 157.5 && angle < 172.5){
            spriterenderer.material = tex11;
        } else {
            spriterenderer.material = tex12;
        }
    }

    public void SetSpriteSheet(Material[] newsheet){
        tex0 = newsheet[0];
        tex1 = newsheet[1];
        tex2 = newsheet[2];
        tex3 = newsheet[3];
        tex4 = newsheet[4];
        tex5 = newsheet[5];
        tex6 = newsheet[6];
        tex7 = newsheet[7];
        tex8 = newsheet[8];
        tex9 = newsheet[9];
        tex10 = newsheet[10];
        tex11 = newsheet[11];
        tex12 = newsheet[12];
    }
}
