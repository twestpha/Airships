using UnityEngine;
 using System.Collections;

 public class Pixelation : MonoBehaviour {

     public float texelsToScreenX;
     public float texelsToScreenY;

     public RenderTexture renderTexture;
     public Texture overlay;

     public Texture compassFrame;
     public Texture[] compassNeedleSprites;
     private float degreesPerSprite;


     void Start() {
         texelsToScreenX = Screen.width / renderTexture.width;
         texelsToScreenY = Screen.height / renderTexture.height;

         degreesPerSprite = 360.0f / (float) (compassNeedleSprites.Length);
     }

     void OnGUI() {
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
     }
 }
