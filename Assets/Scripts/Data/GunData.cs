using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class GunData : ScriptableObject {
    public bool automatic;
    public bool hasScope;
    public bool projectile;

    public int currentAmmo;
    public int magazineAmmo;

    public float damage;
    public float armorBonus; // can be positive or negative
    public float fireTime;
    public float reloadTime;
    public float maxRange;

    public Texture idleTexture;
    public Texture fireTexture1;
    public Texture fireTexture2;
    public Texture reloadTexture;

    // 16kbps/16000 samples
    public AudioClip fireSound;

    public GameObject projectilePrefab;
}
