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
    public Texture[] dialTextures;

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
            // Rendered Camera View
            GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), renderTexture);

            // Firing underlay highlights
            if(gunComponent.overlayActive){
                GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), firingUnderlights);
            }

            // Main Vehicle Overlay
            GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), vehicleBase);

            // Compass Drawing
            int index = (int)((360.0f - (airplaneComponent.heading - (0.5f * compassDegreesPerSection))) / compassDegreesPerSection);
            index = Mathf.Min(Mathf.Max(0, index), compassTextures.Length - 1);
            GUI.DrawTexture(new Rect(155.0f * texelsToScreenX, 303.0f * texelsToScreenY, compassTextures[index].width * texelsToScreenX, compassTextures[index].height * texelsToScreenY), compassTextures[index]);

            // Airspeed Indicator Drawing
            int hundreds = (int)(airplaneComponent.airspeed / 100.0f);
            int tens = (int)((airplaneComponent.airspeed - (hundreds * 100.0f)) / 10.0f);
            int ones = (int)(airplaneComponent.airspeed - (hundreds * 100.0f) - (tens * 10.0f));
            int dec = (int)((airplaneComponent.airspeed - (int)(airplaneComponent.airspeed)) * 10.0f);

            GUI.DrawTexture(new Rect(162.0f * texelsToScreenX, 275.0f * texelsToScreenY, dialTextures[hundreds].width * texelsToScreenX, dialTextures[hundreds].height * texelsToScreenY), dialTextures[hundreds]);
            GUI.DrawTexture(new Rect(176.0f * texelsToScreenX, 275.0f * texelsToScreenY, dialTextures[tens].width * texelsToScreenX, dialTextures[tens].height * texelsToScreenY), dialTextures[tens]);
            GUI.DrawTexture(new Rect(190.0f * texelsToScreenX, 275.0f * texelsToScreenY, dialTextures[ones].width * texelsToScreenX, dialTextures[ones].height * texelsToScreenY), dialTextures[ones]);
            GUI.DrawTexture(new Rect(208.0f * texelsToScreenX, 275.0f * texelsToScreenY, dialTextures[dec].width * texelsToScreenX, dialTextures[dec].height * texelsToScreenY), dialTextures[dec]);

            // Firing Highlights
            if(gunComponent.overlayActive){
                GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), firingHighlights);
            }

            // Screen Text
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
