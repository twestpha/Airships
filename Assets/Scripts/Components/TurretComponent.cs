using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretComponent : MonoBehaviour {

    public float engageRange;
    public float fireTime;
    public int reloadCount;
    private int bulletsFired;
    public float reloadTime;

    public GameObject barrel;
    public GameObject bulletPrefab;

    private GameObject player;
    private Timer fireTimer;
    private Timer reloadTimer;

    private AudioSource source;

    public bool leadingMode;

	void Start(){
        player = GameObject.FindWithTag("Player");

        fireTimer = new Timer(fireTime);
        reloadTimer = new Timer(reloadTime);

        bulletsFired = 0;

        source = GetComponent<AudioSource>();
	}

	void Update(){
        Vector3 target = player.transform.position;

        if(leadingMode){
            target += player.GetComponent<AirplaneComponent>().velocity * 0.3f;
        }

        Vector3 targetVector = target - transform.position;

        if(targetVector.magnitude < engageRange){
            transform.rotation = Quaternion.Euler(-90.0f, 180.0f, 180.0f - Mathf.Atan2(targetVector.z, targetVector.x) * Mathf.Rad2Deg);

            Vector2 distance2d = new Vector2(targetVector.z, targetVector.x);
            barrel.transform.localRotation = Quaternion.Euler(0.0f, -1.0f * Mathf.Atan2(targetVector.y, distance2d.magnitude) * Mathf.Rad2Deg, 0.0f);

            if(bulletsFired < reloadCount && fireTimer.Finished() && reloadTimer.Finished()){
                bulletsFired++;
                fireTimer.Start();

                source.Play();

                GameObject bullet = Object.Instantiate(bulletPrefab, barrel.transform.position, Quaternion.LookRotation(targetVector));
                bullet.transform.parent = null;
            } else if(bulletsFired >= reloadCount){
                reloadTimer.Start();
                bulletsFired = 0;
            }
        }
	}
}
