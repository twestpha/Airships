using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveGameData {
    public int saveversion;
    public string savelevel;

    // Level Script
    public int levelScriptCommand;
    public int levelBalloonkills;
    public int levelInterceptorkills;
    public int levelBomberkills;
    public int levelBosskills;
    public int levelDeaths;

    // Player transform
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;

    public float playerOrientationW;
    public float playerOrientationX;
    public float playerOrientationY;
    public float playerOrientationZ;
    // Player Airplane Component
    public bool playerThrottleEnabled;

    public float playerVelocityX;
    public float playerVelocityY;
    public float playerVelocityZ;

    public float playerThrottle;
    // Player Airplane Gun Component
    public bool playerGunsEnabled;
    // Player Vehicle Camera component
    public bool playerBriefingMode;
}
