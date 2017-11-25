using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour {

    public bool open = false;

    public Material openMaterial;
    public Material closedMaterial;

    private BoxCollider doorCollider;

    public GameObject sprite1;
    private MeshRenderer sprite1Renderer;
    public GameObject sprite2;
    private MeshRenderer sprite2Renderer;

    private GameObject player;

	void Start(){
        doorCollider = GetComponent<BoxCollider>();
        sprite1Renderer = sprite1.GetComponent<MeshRenderer>();
        sprite2Renderer = sprite2.GetComponent<MeshRenderer>();

        player = GameObject.FindWithTag("Player");
	}

	void Update(){

        if((transform.position - player.transform.position).magnitude < 1.5f && (Input.GetKeyDown("e") || Input.GetKeyDown("q"))){
            open = !open;
        }

        doorCollider.enabled = !open;

        if(open){
            sprite1Renderer.material = openMaterial;
            sprite2Renderer.material = openMaterial;
        } else {
            sprite1Renderer.material = closedMaterial;
            sprite2Renderer.material = closedMaterial;
        }
	}
}
