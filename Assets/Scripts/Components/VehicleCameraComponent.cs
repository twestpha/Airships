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
    public Texture firingUnderlights;

    public Texture[] compassTextures;
    private float compassDegreesPerSection;
    public Texture warninglightTexture;

    public Texture briefingScreen;

    public bool briefingMode = false;
    public bool deathMode = false;

    public GameObject airplaneObject;
    private AirplaneGunComponent gunComponent;
    private AirplaneComponent airplaneComponent;

    // screen printing vars
    public const int MaxStringLength = 32;
    public const float FontWidth = 20.0f;
    public const float FontHeight = 30.0f;
    public FontTextureData font;
    private Texture[] screenStringTextures;
    public Timer screenPrintTimer;

    void Start() {
        Cursor.visible = false;

        texelsToScreenX = (float) Screen.width / (float) renderTexture.width;
        texelsToScreenY = (float) Screen.height / (float) renderTexture.height;

        vehicleCamera = GetComponent<Camera>();

        gunComponent = airplaneObject.GetComponent<AirplaneGunComponent>();
        airplaneComponent = airplaneObject.GetComponent<AirplaneComponent>();

        screenStringTextures = new Texture[MaxStringLength];
        screenPrintTimer = new Timer(7.0f);

        compassDegreesPerSection = 360.0f / compassTextures.Length;
    }

    void OnGUI() {
        GUI.depth = 20;

        if(briefingMode){
            GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), briefingScreen);
        } else if(!deathMode) {
            GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), renderTexture);

            if(gunComponent.overlayActive){
                GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), firingUnderlights);
            }

            GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), vehicleBase);

            int index = (int)((360.0f - (airplaneComponent.heading - (0.5f * compassDegreesPerSection))) / compassDegreesPerSection);
            index = Mathf.Min(Mathf.Max(0, index), compassTextures.Length - 1);
            GUI.DrawTexture(new Rect(155.0f * texelsToScreenX, 303.0f * texelsToScreenY, compassTextures[index].width * texelsToScreenX, compassTextures[index].height * texelsToScreenY), compassTextures[index]);

            if(gunComponent.overlayActive){
                GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), firingHighlights);
            }

            if(!screenPrintTimer.Finished()){
                for(int i = 0; i < MaxStringLength; ++i){
                    if(screenStringTextures[i]){
                        GUI.DrawTexture(new Rect(i * FontWidth * texelsToScreenX, 5 * texelsToScreenY, FontWidth * texelsToScreenX, FontHeight * texelsToScreenY), screenStringTextures[i]);
                    }
                }
            }
        } else {
            GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), renderTexture);
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

        screenPrintTimer.Start();
    }
}
