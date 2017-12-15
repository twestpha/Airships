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

    protected GameObject player;

    private Timer delayTimer;

    Dictionary<string, float> floatdict;
    Dictionary<string, int> intdict;

    // #########################################################################
    // Level Stats to track
    // #########################################################################
    [Header("Level Stats")]
    public int balloonkills = 0;
    public int interceptorkills = 0;
    public int bomberkills = 0;
    public int deaths = 0;

    void Start(){
        player = GameObject.FindWithTag("Player");

        floatdict = new Dictionary<string, float>();
        intdict = new Dictionary<string, int>();

        delayTimer = new Timer();
        delayTimer.FinishedThisFrame();

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

    // Print - prints out debug information
    protected void Print(string s){
        functionlist.Add(new Func<int>(() => {return Print_(s);    }));
    }
    protected static int Print_(string s){
        Debug.Log(s);
        return NextCmd;
    }

    // Transmission - plays audioclip
    protected void Transmission(AudioClip clip, bool wait){
        functionlist.Add(new Func<int>(() => {return Transmission_(clip, wait);   }));
    }
    protected int Transmission_(AudioClip clip, bool wait){
        AudioSource source = GetComponent<AudioSource>();

        if(!source.isPlaying && source.clip != clip){
            source.clip = clip;
            source.Play();
        }

        return wait && source.isPlaying ? ThisCmd : NextCmd;
    }

    // CreateObjectAtPosition - creates a given gameobject at position
    protected void CreateObjectAtPosition(GameObject gameobject, Vector3 position){
        functionlist.Add(new Func<int>(() => {return CreateObjectAtPosition_(gameobject, position);   }));
    }
    protected int CreateObjectAtPosition_(GameObject gameobject, Vector3 position){
        GameObject.Instantiate(gameobject, position, new Quaternion());
        return NextCmd;
    }

    // EnableAirplaneThrottle - sets the gameobject's airplane component throttle enabled or not
    protected void EnableAirplaneThrottle(GameObject gameobject, bool enabled){
        functionlist.Add(new Func<int>(() => {return EnableAirplaneThrottle_(gameobject, enabled);    }));
    }
    protected int EnableAirplaneThrottle_(GameObject gameobject, bool enabled){
        gameobject.GetComponent<AirplaneComponent>().throttleEnabled = enabled;
        return NextCmd;
    }

    // EnableAirplaneGuns - sets the gameobject's airplane gun component gunsenabled enabled or not
    protected void EnableAirplaneGuns(GameObject gameobject, bool enabled){
        functionlist.Add(new Func<int>(() => {return EnableAirplaneGuns_(gameobject, enabled);    }));
    }
    protected int EnableAirplaneGuns_(GameObject gameobject, bool enabled){
        gameobject.GetComponent<AirplaneGunComponent>().gunsEnabled = enabled;
        return NextCmd;
    }

    // #########################################################################
    // Variable Methods
    // #########################################################################
    protected void SetVarToGameTime(string varname){
        functionlist.Add(new Func<int>(() => {return SetVarToGameTime_(varname);    }));
    }
    protected int SetVarToGameTime_(string varname){
        floatdict[varname] = Time.time;
        return NextCmd;
    }

    protected void SetVarToThrottle(GameObject gameobject, string varname){
        functionlist.Add(new Func<int>(() => {return SetVarToThrottle_(gameobject, varname);    }));
    }
    protected int SetVarToThrottle_(GameObject gameobject, string varname){
        floatdict[varname] = gameobject.GetComponent<AirplaneComponent>().throttle;
        return NextCmd;
    }

    protected void SetVarToAirspeed(GameObject gameobject, string varname){
        functionlist.Add(new Func<int>(() => {return SetVarToAirspeed_(gameobject, varname);    }));
    }
    protected int SetVarToAirspeed_(GameObject gameobject, string varname){
        floatdict[varname] = gameobject.GetComponent<AirplaneComponent>().airspeed;
        return NextCmd;
    }

    protected void SetVarToBalloonKills(string varname){
        functionlist.Add(new Func<int>(() => {return SetVarToBalloonKills_(varname);    }));
    }
    protected int SetVarToBalloonKills_(string varname){
        intdict[varname] = balloonkills;
        return NextCmd;
    }

    protected void SetVarToHeading(GameObject gameobject, string varname){
        functionlist.Add(new Func<int>(() => {return SetVarToHeading_(gameobject, varname);    }));
    }
    protected int SetVarToHeading_(GameObject gameobject, string varname){
        floatdict[varname] = gameobject.GetComponent<AirplaneComponent>().heading;
        return NextCmd;
    }

    // #########################################################################
    // Delay Methods
    // #########################################################################

    // Delay - waits for a duration of time
    protected void Delay(float time){
        functionlist.Add(new Func<int>(() => {return Delay_(time);    }));
    }
    protected int Delay_(float time){
        if(delayTimer.FinishedThisFrame()){
            return NextCmd;
        } else if(delayTimer.Finished()){
            delayTimer.SetDuration(time);
            delayTimer.Start();
        }

        return ThisCmd;
    }

    protected void WaitForGameTime(float gt){
        functionlist.Add(new Func<int>(() => {return WaitForGameTime_(gt);  }));
    }
    protected static int WaitForGameTime_(float gametime){
        return Time.time > gametime ? NextCmd : ThisCmd;
    }

    // Int methods
    protected void WaitEqual(int a, int b){
        functionlist.Add(new Func<int>(() => {return a == b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitNotEqual(int a, int b){
        functionlist.Add(new Func<int>(() => {return a != b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitGreaterThan(int a, int b){
        functionlist.Add(new Func<int>(() => {return a > b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitLessThan(int a, int b){
        functionlist.Add(new Func<int>(() => {return a < b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitGreaterThanEqual(int a, int b){
        functionlist.Add(new Func<int>(() => {return a >= b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitLessThanEqual(int a, int b){
        functionlist.Add(new Func<int>(() => {return a <= b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitInRange(int x, int a, int b){
        functionlist.Add(new Func<int>(() => {return a <= x && x <= b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitOutOfRange(int x, int a, int b){
        functionlist.Add(new Func<int>(() => {return x <= a && b <= x ? NextCmd : PrevCmd;  }));
    }

    // Int variable methods
    protected void WaitEqual(string varname, int b){
        functionlist.Add(new Func<int>(() => {return intdict[varname] == b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitNotEqual(string varname, int b){
        functionlist.Add(new Func<int>(() => {return intdict[varname] != b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitGreaterThan(string varname, int b){
        functionlist.Add(new Func<int>(() => {return intdict[varname] > b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitLessThan(string varname, int b){
        functionlist.Add(new Func<int>(() => {return intdict[varname] < b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitGreaterThanEqual(string varname, int b){
        functionlist.Add(new Func<int>(() => {return intdict[varname] >= b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitLessThanEqual(string varname, int b){
        functionlist.Add(new Func<int>(() => {return intdict[varname] <= b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitInRange(string varname, int a, int b){
        functionlist.Add(new Func<int>(() => {return a <= intdict[varname] && intdict[varname] <= b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitOutOfRange(string varname, int a, int b){
        functionlist.Add(new Func<int>(() => {return intdict[varname] <= a && b <= intdict[varname] ? NextCmd : PrevCmd;  }));
    }

    // Float methods
    protected void WaitEqual(float a, float b){
        functionlist.Add(new Func<int>(() => {return a == b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitNotEqual(float a, float b){
        functionlist.Add(new Func<int>(() => {return a != b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitGreaterThan(float a, float b){
        functionlist.Add(new Func<int>(() => {return a > b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitLessThan(float a, float b){
        functionlist.Add(new Func<int>(() => {return a < b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitGreaterThanEqual(float a, float b){
        functionlist.Add(new Func<int>(() => {return a >= b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitLessThanEqual(float a, float b){
        functionlist.Add(new Func<int>(() => {return a <= b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitInRange(float x, float a, float b){
        functionlist.Add(new Func<int>(() => {return a <= x && x <= b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitOutOfRange(float x, float a, float b){
        functionlist.Add(new Func<int>(() => {return x <= a && b <= x ? NextCmd : PrevCmd;  }));
    }

    // Float variable methods
    protected void WaitEqual(string varname, float b){
        functionlist.Add(new Func<int>(() => {return floatdict[varname] == b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitNotEqual(string varname, float b){
        functionlist.Add(new Func<int>(() => {return floatdict[varname] != b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitGreaterThan(string varname, float b){
        functionlist.Add(new Func<int>(() => {return floatdict[varname] > b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitLessThan(string varname, float b){
        functionlist.Add(new Func<int>(() => {return floatdict[varname] < b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitGreaterThanEqual(string varname, float b){
        functionlist.Add(new Func<int>(() => {return floatdict[varname] >= b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitLessThanEqual(string varname, float b){
        functionlist.Add(new Func<int>(() => {return floatdict[varname] <= b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitInRange(string varname, float a, float b){
        functionlist.Add(new Func<int>(() => {return a <= floatdict[varname] && floatdict[varname] <= b ? NextCmd : PrevCmd;  }));
    }
    protected void WaitOutOfRange(string varname, float a, float b){
        functionlist.Add(new Func<int>(() => {return floatdict[varname] <= a && b <= floatdict[varname] ? NextCmd : PrevCmd;  }));
    }
}
