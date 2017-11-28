using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHitEffects : MonoBehaviour {

    private Timer timer;
    private int state = 0;

    private MeshRenderer meshrenderer;

    public Material materialA;
    public Material materialB;

	void Start(){
        timer = new Timer(0.07f);
        timer.Start();

        meshrenderer = GetComponent<MeshRenderer>();
        meshrenderer.material = materialA;
	}

	void Update(){
        if(state == 0 && timer.Finished()){
            state++;
            timer.Start();
            meshrenderer.material = materialB;
        } else if(state == 1 && timer.Finished()){
            Destroy(gameObject);
        }
	}
}
