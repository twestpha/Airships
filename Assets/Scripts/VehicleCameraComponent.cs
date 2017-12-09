using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class VehicleCameraComponent : MonoBehaviour {

    public float texelsToScreenX;
    public float texelsToScreenY;

    private Camera vehicleCamera;

    public RenderTexture renderTexture;
    public Texture overlay;

    void Start() {
        texelsToScreenX = (float) Screen.width / (float) renderTexture.width;
        texelsToScreenY = (float) Screen.height / (float) renderTexture.height;

        vehicleCamera = GetComponent<Camera>();
    }

    void OnGUI() {
        GUI.depth = 20;
        GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), renderTexture);
        GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), overlay);
    }
}
