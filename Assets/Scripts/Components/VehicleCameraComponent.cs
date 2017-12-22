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

    // screen printing vars
    public const int MaxStringLength = 32;
    public const float FontWidth = 20.0f;
    public const float FontHeight = 30.0f;
    public FontTextureData font;
    public Texture[] screenStringTextures;
    public Timer screenTimer;

    void Start() {
        texelsToScreenX = (float) Screen.width / (float) renderTexture.width;
        texelsToScreenY = (float) Screen.height / (float) renderTexture.height;

        vehicleCamera = GetComponent<Camera>();

        gunComponent = airplaneGunObject.GetComponent<AirplaneGunComponent>();

        screenStringTextures = new Texture[MaxStringLength];
        screenTimer = new Timer(7.0f);
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

            if(!screenTimer.Finished()){
                for(int i = 0; i < MaxStringLength; ++i){
                    if(screenStringTextures[i]){
                        GUI.DrawTexture(new Rect(i * FontWidth * texelsToScreenX, 5 * texelsToScreenY, FontWidth * texelsToScreenX, FontHeight * texelsToScreenY), screenStringTextures[i]);
                    }
                }
            }
        }
    }

    public void PrintStringToScreen(string message){
        for(int i = 0; i < MaxStringLength; ++i){
            if(i < message.Length){
                screenStringTextures[i] = font.fontsheet[(int)message[i]];
            } else {
                screenStringTextures[i] = null;
            }
        }

        if(message.Length > MaxStringLength){
            Debug.LogWarning("Message '" + message + "' contains more than " + MaxStringLength + " characters and will be truncated");
        }

        screenTimer.Start();
    }
}
