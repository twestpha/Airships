using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotatable : MonoBehaviour {

    public GameObject sprite;
    private MeshRenderer renderer;
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
        renderer = sprite.GetComponent<MeshRenderer>();
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

        // fuck everything lol
        if(angle >= -7.5f && angle < 7.5f){
            renderer.material = tex0;
        } else if (angle >= 7.5f && angle < 22.5f){
            renderer.material = tex1;
        } else if (angle >= 22.5f && angle < 37.5f){
            renderer.material = tex2;
        } else if (angle >= 37.5f && angle < 52.5f){
            renderer.material = tex3;
        } else if (angle >= 52.5f && angle < 67.5f){
            renderer.material = tex4;
        } else if (angle >= 67.5f && angle < 82.5){
            renderer.material = tex5;
        } else if (angle >= 82.5 && angle < 97.5f){
            renderer.material = tex6;
        } else if (angle >= 97.5f && angle < 112.5f){
            renderer.material = tex7;
        } else if (angle >= 112.5f && angle < 127.5){
            renderer.material = tex8;
        } else if (angle >= 127.5 && angle < 142.5){
            renderer.material = tex9;
        } else if (angle >= 142.5f && angle < 157.5){
            renderer.material = tex10;
        } else if (angle >= 157.5 && angle < 172.5){
            renderer.material = tex11;
        } else {
            renderer.material = tex12;
        }
    }
}
