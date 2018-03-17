using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesctructableEnvironmentComponent : MonoBehaviour {

    public Material[] animationFrames;
    public float timeBetweenFrames;
    private Timer timer;

    private bool destroyed;
    private int index;

	void Start(){
        destroyed = false;
        index = 0;

        timer = new Timer(timeBetweenFrames);
	}

	void Update(){
        bool prevdestroyed = destroyed;

        destroyed = GetComponent<DamageableComponent>().health < 0;

        if(!prevdestroyed && destroyed){
            timer.Start();
            GameObject.FindWithTag("Level Script").GetComponent<LevelScriptBase>().environmentkills++;
        }

        if(destroyed && timer.Finished()){
            GetComponent<Renderer>().material = animationFrames[index];
            index++;
            index = Mathf.Min(index, animationFrames.Length - 1);
            timer.Start();
        }
	}
}
