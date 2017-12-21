using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class AirplaneGunData : ScriptableObject {
    public bool automatic;

    public int currentAmmo;
    public int startingAmmo;

    public float damage;
    public float armorBonus; // can be positive or negative
    public float fireTime;

    // 16kbps/16000 samples
    public AudioClip fireSound;

    public bool useMachineGunSpawns;
    public GameObject projectilePrefab;
}
