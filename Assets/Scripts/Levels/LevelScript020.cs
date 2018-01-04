using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelScript020 : LevelScriptBase {

    protected override void Progression(){
        Delay(5.0f);

        LoadLevel("020_blitz");
    }
}
