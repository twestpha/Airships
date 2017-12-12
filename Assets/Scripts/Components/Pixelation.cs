using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

 public class Pixelation : MonoBehaviour {

     public float texelsToScreenX;
     public float texelsToScreenY;

     public FPSComponent fpsController;
     public GameObject gunObject;

     private GunComponent guncomponent;
     private Camera fpscamera;

     public RenderTexture renderTexture;
     public Texture overlay;

     public Texture compassFrame;
     public Texture[] compassNeedleSprites;
     private float degreesPerSprite;

     public float unscopedFOV;
     public float scopedFOV;
     public float scopeTime;
     public Texture scopeTexture;
     private Timer scopeTimer;
     public bool scoped;

     public int offsetX;
     public int offsetY;

     public Texture testTexture;

     void Start() {
         fpsController = gunObject.GetComponent<FPSComponent>();

         if (!fpsController.isLocalPlayer){
             return;
         }

         texelsToScreenX = (float) Screen.width / (float) renderTexture.width;
         texelsToScreenY = (float) Screen.height / (float) renderTexture.height;

         degreesPerSprite = 360.0f / (float) (compassNeedleSprites.Length);

         guncomponent = gunObject.GetComponent<GunComponent>();

         fpscamera = GetComponent<Camera>();

         scopeTimer = new Timer(scopeTime);
     }

     void OnGUI() {
         if (!fpsController.isLocalPlayer){
             return;
         }

         GUI.depth = 20;
         GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), renderTexture);
         GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), overlay);

         // Compass drawing
         GUI.DrawTexture(new Rect(0,0, compassFrame.width * texelsToScreenX, compassFrame.height * texelsToScreenY), compassFrame);

         float angle = 360.0f - transform.rotation.eulerAngles.y;

         angle += degreesPerSprite * 0.5f;
         int index = (int)(angle / degreesPerSprite);

         if(index > compassNeedleSprites.Length - 1){
             index = 0;
         }

         Texture compassNeedle = compassNeedleSprites[index];

         GUI.DrawTexture(new Rect(-25.0f * texelsToScreenX, -18.0f * texelsToScreenY, compassNeedle.width * texelsToScreenX, compassNeedle.height * texelsToScreenY), compassNeedle);

         // Gun Drawing
         if(Input.GetMouseButton(1) && guncomponent.CanScope()){
             if(!scoped){
                 scoped = true;
                 scopeTimer.Start();
             }

             fpscamera.fieldOfView = Mathf.Lerp(unscopedFOV, scopedFOV, scopeTimer.Parameterized());
             GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), scopeTexture);
         } else {
             if(scoped){
                 scoped = false;
                 scopeTimer.Start();
             }
             fpscamera.fieldOfView = Mathf.Lerp(scopedFOV, unscopedFOV, scopeTimer.Parameterized());
         }

         if(!scoped){
             Texture gunSprite = guncomponent.GetCurrentGunTexture();
             GUI.DrawTexture(new Rect(64.0f * texelsToScreenX, 104.0f * texelsToScreenY, gunSprite.width * texelsToScreenX, gunSprite.height * texelsToScreenY), gunSprite);
         }
     }
 }
