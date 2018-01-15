using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
[System.Serializable]
public class SaveGameData : ScriptableObject {
    public int saveversion;
    // Level Script
    public int levelScriptCommand;
    public int levelBalloonkills;
    public int levelInterceptorkills;
    public int levelBomberkills;
    public int levelBosskills;
    public int levelDeaths;

    // Player transform
    public Vector3 playerPosition;
    public Quaternion playerOrientation;
    // Player Airplane Component
    public bool playerThrottleEnabled;
    public Vector3 playerVelocity;
    public float playerThrottle;
    // Player Airplane Gun Component
    public bool playerGunsEnabled;
    // Player Vehicle Camera component
    public bool playerBriefingMode;
}
