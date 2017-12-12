using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelScript010 : LevelScriptBase {
    protected override void Progression(){
        EnablePlayerThrottle(false);
        WaitForGameTime(6.0f);
        Transmission("Start", "Going");
        EnablePlayerThrottle(true);
    }
}
