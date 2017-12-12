using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashComponent : MonoBehaviour {

    private int frame;
    public Material[] frames;
    private Timer frameTimer;

    void Start(){
        frameTimer = new Timer(0.1f);
        frameTimer.Start();

        frame = 0;
        GetComponent<MeshRenderer>().material = frames[0];
    }

    void Update(){
        if(frameTimer.Finished()){
            frameTimer.Start();
            if(frame > frames.Length - 2){
                Destroy(gameObject);
                return;
            }

            frame++;
            GetComponent<MeshRenderer>().material = frames[frame];
        }
    }
}
