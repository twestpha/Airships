using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelScriptBase : MonoBehaviour {

    protected List<Func<int>> functionlist;

    protected const int NextCmd = 1;
    protected const int ThisCmd = 0;
    protected const int PrevCmd = -1;

    public int command = 0;
    private bool finished = false;

    private GameObject player;

    // #########################################################################
    // Level Stats to track
    // #########################################################################
    public int kills = 0;
    public int deaths = 0;
    public int bulletsfired = 0;

    void Start(){
        player = GameObject.FindWithTag("Player");

        functionlist = new List<Func<int>>();
        Progression();
    }

    void Update(){
        if(!finished){
            command += functionlist[command]();
            finished = command > functionlist.Count - 1;
        }
    }

    protected virtual void Progression(){}

    // #########################################################################
    // Action Methods
    // #########################################################################

    // Nop - no operation, does nothing
    protected void Nop(){
        functionlist.Add(new Func<int>(() => {return Nop_();    }));
    }
    protected static int Nop_(){
        return NextCmd;
    }

    // Transmission - displays text box and plays audioclip
    protected void Transmission(string a, string b){
        functionlist.Add(new Func<int>(() => {return Transmission_(a, b);   }));
    }
    protected int Transmission_(string a, string b){
        Debug.Log(a + b);
        return NextCmd;
    }

    // EnablePlayerThrottle - sets the player throttle enabled or not
    protected void EnablePlayerThrottle(bool enabled){
        functionlist.Add(new Func<int>(() => {return EnablePlayerThrottle_(enabled);    }));
    }
    protected int EnablePlayerThrottle_(bool enabled){
        player.GetComponent<AirplaneComponent>().throttleEnabled = enabled;
        return NextCmd;
    }


    // #########################################################################
    // Delay Methods
    // #########################################################################
    protected void WaitForGameTime(float gt){
        functionlist.Add(new Func<int>(() => {return WaitForGameTime_(gt);  }));
    }
    protected static int WaitForGameTime_(float gametime){
        return Time.time > gametime ? NextCmd : ThisCmd;
    }

    // Int methods
    protected static int WaitEquals(int a, int b){
        return a == b ? NextCmd : PrevCmd;
    }

    protected static int WaitGreaterThan(int a, int b){
        return a > b ? NextCmd : PrevCmd;
    }

    protected static int WaitLessThan(int a, int b){
        return WaitGreaterThan(b, a);
    }

    protected static int WaitGreaterThanEqual(int a, int b){
        return a >= b ? NextCmd : PrevCmd;
    }

    protected static int WaitLessThanEqual(int a, int b){
        return WaitGreaterThan(b, a);
    }

    // Float methods
    protected static int WaitEquals(float a, float b){
        return a == b ? NextCmd : PrevCmd;
    }

    protected static int WaitGreaterThan(float a, float b){
        return a > b ? NextCmd : PrevCmd;
    }

    protected static int WaitLessThan(float a, float b){
        return WaitGreaterThan(b, a);
    }

    protected static int WaitGreaterThanEqual(float a, float b){
        return a >= b ? NextCmd : PrevCmd;
    }

    protected static int WaitLessThanEqual(float a, float b){
        return WaitGreaterThan(b, a);
    }
}
