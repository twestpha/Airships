using UnityEngine;
 using System.Collections;

 public class Pixelation : MonoBehaviour {

     public RenderTexture renderTexture;
     public Texture overlay;

     void Start() {
     }

     void OnGUI() {
         GUI.depth = 20;
         GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), renderTexture);
         // GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), overlay);
     }
 }
