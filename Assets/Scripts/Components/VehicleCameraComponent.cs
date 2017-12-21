using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class VehicleCameraComponent : MonoBehaviour {

    public float texelsToScreenX;
    public float texelsToScreenY;

    private Camera vehicleCamera;

    public RenderTexture renderTexture;

    public Texture vehicleBase;
    public Texture firingHighlights;

    public Texture briefingScreen;

    public bool briefingMode = false;

    public GameObject airplaneGunObject;
    private AirplaneGunComponent gunComponent;

    void Start() {
        texelsToScreenX = (float) Screen.width / (float) renderTexture.width;
        texelsToScreenY = (float) Screen.height / (float) renderTexture.height;

        vehicleCamera = GetComponent<Camera>();

        gunComponent = airplaneGunObject.GetComponent<AirplaneGunComponent>();
    }

    void OnGUI() {
        GUI.depth = 20;

        if(briefingMode){
            GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), briefingScreen);
        } else {
            GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), renderTexture);

            GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), vehicleBase);

            if(gunComponent.overlayActive){
                GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), firingHighlights);
            }
        }
    }
}
