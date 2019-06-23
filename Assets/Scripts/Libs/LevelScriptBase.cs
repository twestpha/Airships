using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
[RequireComponent(typeof(AudioHighPassFilter))]
public class LevelScriptBase : MonoBehaviour {

    private SaveGameData saveGameData;
    private const int saveversion = 2;
    private string lastsave;
    private string lastlevel;

    protected List<Func<int>> functionlist;

    protected const int NextCmd = 1;
    protected const int ThisCmd = 0;
    protected const int PrevCmd = -1;

    // #########################################################################
    // Transmission Settings
    // #########################################################################
    protected const bool useradio = true;
    protected const bool dontuseradio = false;

    protected const bool wait = true;
    protected const bool dontwait = false;

    private const float radioLowPassFilterCutoffFrequency = 2300.0f;
    private const float radioHighPassFilterCutoffFrequency = 600.0f;

    // #########################################################################

    public int command = 0;
    private const int maxCommandsPerFrame = 4;
    private bool finished = false;

    protected GameObject player;
    protected GameObject maincamera;

    private Timer delayTimer;

    Dictionary<string, float> floatdict;
    Dictionary<string, int> intdict;

    public AudioSource radioAudioSource;

    private bool transmissionPlaying;

    // #########################################################################
    // Level Stats to track
    // #########################################################################
    [Header("Level Stats")]
    public int balloonkills = 0;
    public int interceptorkills = 0;
    public int vehiclekills = 0;
    public int bomberkills = 0;
    public int bosskills = 0;
    public int environmentkills = 0;
    public int deaths = 0;

    void Start(){
        player = GameObject.FindWithTag("Player");
        maincamera = GameObject.FindWithTag("MainCamera");

        saveGameData = new SaveGameData();

        floatdict = new Dictionary<string, float>();
        intdict = new Dictionary<string, int>();

        delayTimer = new Timer();
        delayTimer.FinishedThisFrame();

        GetComponent<AudioLowPassFilter>().cutoffFrequency = radioLowPassFilterCutoffFrequency;
        GetComponent<AudioHighPassFilter>().cutoffFrequency = radioHighPassFilterCutoffFrequency;

        functionlist = new List<Func<int>>();
        Progression();
    }

    void Update(){
        if(!finished){
            for(int i = 0; i < maxCommandsPerFrame; ++i){
                int previousCommand = command;

                command += functionlist[command]();
                finished = command > functionlist.Count - 1;

                if(command == previousCommand || finished){
                    break;
                }
            }
        }

        // Debug save/load testing
        if(Input.GetKeyDown(KeyCode.F5)){
            SaveGame_("quicksave");
        } else if(Input.GetKeyDown(KeyCode.F6)){
            LoadLastSave_();
        }
    }

    protected int SaveGame_(string name){
        saveGameData.saveversion = saveversion;

        saveGameData.levelScriptCommand = command;
        saveGameData.levelBalloonkills = balloonkills;
        saveGameData.levelInterceptorkills = interceptorkills;
        saveGameData.levelBomberkills = bomberkills;
        saveGameData.levelBosskills = bosskills;
        saveGameData.levelDeaths = deaths;

        saveGameData.playerPositionX = player.transform.position.x;
        saveGameData.playerPositionY = player.transform.position.y;
        saveGameData.playerPositionZ = player.transform.position.z;

        saveGameData.playerOrientationW = player.transform.rotation.w;
        saveGameData.playerOrientationX = player.transform.rotation.x;
        saveGameData.playerOrientationY = player.transform.rotation.y;
        saveGameData.playerOrientationZ = player.transform.rotation.z;

        AirplaneComponent playerAirplaneComponent = player.GetComponent<AirplaneComponent>();
        saveGameData.playerThrottleEnabled = playerAirplaneComponent.throttleEnabled;

        saveGameData.playerVelocityX = playerAirplaneComponent.velocity.x;
        saveGameData.playerVelocityY = playerAirplaneComponent.velocity.y;
        saveGameData.playerVelocityZ = playerAirplaneComponent.velocity.z;

        saveGameData.playerThrottle = playerAirplaneComponent.throttle;

        AirplaneGunComponent playerAirplaneGunComponent = player.GetComponent<AirplaneGunComponent>();
        saveGameData.playerGunsEnabled = playerAirplaneGunComponent.gunsEnabled;

        saveGameData.playerBriefingMode = maincamera.GetComponent<VehicleCameraComponent>().briefingMode;

        saveGameData.savelevel = gameObject.scene.name;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + name + ".gd");
        bf.Serialize(file, saveGameData);
        file.Close();

        lastsave = name;
        lastlevel = gameObject.scene.name;

        Debug.Log("Game Saved");

        return NextCmd;
    }

    public void LoadLastSave_(){
        LoadGame_(lastsave);
    }

    public void LoadGame_(string name){
        if(File.Exists(Application.persistentDataPath + "/" + name + ".gd")) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + name + ".gd", FileMode.Open);
            saveGameData = bf.Deserialize(file) as SaveGameData;
            file.Close();
        } else {
            Debug.LogError("File " + Application.persistentDataPath + "/" + name + ".gd" + " does not exist. Load failed.");
            return;
        }

        if(saveGameData.saveversion != saveversion){
            Debug.LogError("Save file version " + saveGameData.saveversion + " does not match expected version " + saveversion + ". Load failed.");
            return;
        }

        // stomp currently playing audio
        if(radioAudioSource.isPlaying){
            radioAudioSource.Stop();
        }

        if(saveGameData.savelevel != gameObject.scene.name){
            LoadLevel_(saveGameData.savelevel, LoadSceneMode.Additive);
            // Message them to load this saveload
            // unload this level
            // return
        }

        command = saveGameData.levelScriptCommand;
        balloonkills = saveGameData.levelBalloonkills;
        interceptorkills = saveGameData.levelInterceptorkills;
        bomberkills = saveGameData.levelBomberkills;
        bosskills = saveGameData.levelBosskills;
        deaths = saveGameData.levelDeaths;

        player.transform.position = new Vector3(saveGameData.playerPositionX, saveGameData.playerPositionY, saveGameData.playerPositionZ);
        player.transform.rotation = new Quaternion(saveGameData.playerOrientationX, saveGameData.playerOrientationY, saveGameData.playerOrientationZ, saveGameData.playerOrientationW);

        AirplaneComponent playerAirplaneComponent = player.GetComponent<AirplaneComponent>();
        playerAirplaneComponent.throttleEnabled = saveGameData.playerThrottleEnabled;

        playerAirplaneComponent.velocity = new Vector3(saveGameData.playerVelocityX, saveGameData.playerVelocityY, saveGameData.playerVelocityZ);

        playerAirplaneComponent.throttle = saveGameData.playerThrottle;

        AirplaneGunComponent playerAirplaneGunComponent = player.GetComponent<AirplaneGunComponent>();
        playerAirplaneGunComponent.gunsEnabled = saveGameData.playerGunsEnabled;

        VehicleCameraComponent playerCamera = maincamera.GetComponent<VehicleCameraComponent>();
        playerCamera.briefingMode = saveGameData.playerBriefingMode;
        playerCamera.deathMode = false;

        Debug.Log("Game Loaded");
    }

    void OnApplicationQuit(){
        // Commit save game data
        // EditorUtility.SetDirty(saveGameData);
        // AssetDatabase.SaveAssets();
    }

    public void StopProgression(){
        finished = true;
        radioAudioSource.Stop();
    }

    protected GameObject Actor(string name){
        return GameObject.Find(name);
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

    // Print - prints out to screen
    protected void Print(string s){
        functionlist.Add(new Func<int>(() => {return Print_(s);    }));
    }
    protected static int Print_(string s){
        Debug.Log(s);
        Camera.main.gameObject.GetComponent<VehicleCameraComponent>().PrintStringToScreen(s);
        return NextCmd;
    }

    // Transmission - plays audioclip
    protected void Transmission(AudioClip clip, bool wait, bool useradio){
        functionlist.Add(new Func<int>(() => {return Transmission_(clip, wait, useradio);   }));
    }
    protected int Transmission_(AudioClip clip, bool wait, bool useradio){
        if(!clip){
            Debug.LogError("Clip is null");
        }

        if(wait){
            if(!transmissionPlaying && !radioAudioSource.isPlaying){
                radioAudioSource.bypassEffects = !useradio;
                radioAudioSource.clip = clip;
                radioAudioSource.Play();
                transmissionPlaying = true;
                Debug.Log("[Dialogue] " + clip);
            }

            if(!radioAudioSource.isPlaying){
                transmissionPlaying = false;
            }

            return radioAudioSource.isPlaying ? ThisCmd : NextCmd;
        } else {
            radioAudioSource.bypassEffects = !useradio;
            radioAudioSource.clip = clip;
            radioAudioSource.Play();
            Debug.Log("[Dialogue] " + clip);

            return NextCmd;
        }
    }

    // PlaySong - plays music
    protected void PlaySong(string trackListName, int trackNumber){
        functionlist.Add(new Func<int>(() => {return PlaySong_(trackListName, trackNumber);   }));
    }
    protected int PlaySong_(string trackListName, int trackNumber){
        GetComponent<JukeBoxComponent>().PlaySong(trackListName, trackNumber);

        return NextCmd;
    }

    // CreateObjectAtPosition - creates a given gameobject at position
    protected void CreateObjectAtPosition(GameObject gameobject, Vector3 position){
        functionlist.Add(new Func<int>(() => {return CreateObjectAtPosition_(gameobject, position);   }));
    }
    protected int CreateObjectAtPosition_(GameObject gameobject, Vector3 position){
        GameObject.Instantiate(gameobject, position, new Quaternion());
        return NextCmd;
    }

    // CreateObjectAtPositionWithRotation - creates a given gameobject at position with a rotation
    protected void CreateObjectAtPositionWithRotation(GameObject gameobject, Vector3 position, Vector3 rotation){
        functionlist.Add(new Func<int>(() => {return CreateObjectAtPositionWithRotation_(gameobject, position, rotation);   }));
    }
    protected int CreateObjectAtPositionWithRotation_(GameObject gameobject, Vector3 position, Vector3 rotation){
        GameObject.Instantiate(gameobject, position, Quaternion.Euler(rotation));
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

    // EnableBriefingMode - sets the gameobject's ui to briefing mode enabled or not
    protected void EnableBriefingMode(GameObject gameobject, bool enabled){
        functionlist.Add(new Func<int>(() => {return EnableBriefingMode_(gameobject, enabled);    }));
    }
    protected int EnableBriefingMode_(GameObject gameobject, bool enabled){
        gameobject.GetComponent<VehicleCameraComponent>().briefingMode = enabled;
        return NextCmd;
    }

    // SetBriefingModeTexture - sets the gameobject's ui to briefing mode to texture
    protected void SetBriefingModeTexture(GameObject gameobject, Texture texture){
        functionlist.Add(new Func<int>(() => {return SetBriefingMode_Texture(gameobject, texture);    }));
    }
    protected int SetBriefingMode_Texture(GameObject gameobject, Texture texture){
        gameobject.GetComponent<VehicleCameraComponent>().briefingScreen = texture;
        return NextCmd;
    }

    // LoadLevel - loads the next named level
    protected void LoadLevel(string levelname){
        functionlist.Add(new Func<int>(() => {return LoadLevel_(levelname, LoadSceneMode.Single);    }));
    }
    protected void LoadLevel(string levelname, bool additive){
        functionlist.Add(new Func<int>(() => {return LoadLevel_(levelname, additive ? LoadSceneMode.Additive : LoadSceneMode.Single);    }));
    }
    protected int LoadLevel_(string levelname, LoadSceneMode mode){
        SceneManager.LoadScene(levelname, mode);
        return NextCmd;
    }

    // SaveGame - saves the current state of the game
    protected void SaveGame(string name){
        functionlist.Add(new Func<int>(() => {return SaveGame_(name);    }));
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

    protected void SetVarToInterceptorKills(string varname){
        functionlist.Add(new Func<int>(() => {return SetVarToInterceptorKills_(varname);    }));
    }
    protected int SetVarToInterceptorKills_(string varname){
        intdict[varname] = interceptorkills;
        return NextCmd;
    }

    protected void SetVarToHeading(GameObject gameobject, string varname){
        functionlist.Add(new Func<int>(() => {return SetVarToHeading_(gameobject, varname);    }));
    }
    protected int SetVarToHeading_(GameObject gameobject, string varname){
        floatdict[varname] = gameobject.GetComponent<AirplaneComponent>().heading;
        return NextCmd;
    }

    protected void SetVarToPlayerInVolume(GameObject gameobject, string varname){
        functionlist.Add(new Func<int>(() => {return SetVarToPlayerInVolume_(gameobject, varname);    }));
    }
    protected int SetVarToPlayerInVolume_(GameObject gameobject, string varname){
        intdict[varname] = gameobject.GetComponent<PlayerVolumeComponent>().playerInVolume ? 1 : 0;
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

    protected void WaitForMouseClick(){
        functionlist.Add(new Func<int>(() => {return WaitForMouseClick_();  }));
    }
    protected static int WaitForMouseClick_(){
        return Input.GetMouseButtonDown(0) ? NextCmd : ThisCmd;
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
        functionlist.Add(new Func<int>(() => {return intdict.ContainsKey(varname) ? (intdict[varname] == b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitNotEqual(string varname, int b){
        functionlist.Add(new Func<int>(() => {return intdict.ContainsKey(varname) ? (intdict[varname] != b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitGreaterThan(string varname, int b){
        functionlist.Add(new Func<int>(() => {return intdict.ContainsKey(varname) ? (intdict[varname] > b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitLessThan(string varname, int b){
        functionlist.Add(new Func<int>(() => {return intdict.ContainsKey(varname) ? (intdict[varname] < b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitGreaterThanEqual(string varname, int b){
        functionlist.Add(new Func<int>(() => {return intdict.ContainsKey(varname) ? (intdict[varname] >= b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitLessThanEqual(string varname, int b){
        functionlist.Add(new Func<int>(() => {return intdict.ContainsKey(varname) ? (intdict[varname] <= b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitInRange(string varname, int a, int b){
        functionlist.Add(new Func<int>(() => {return intdict.ContainsKey(varname) ? (a <= intdict[varname] && intdict[varname] <= b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitOutOfRange(string varname, int a, int b){
        functionlist.Add(new Func<int>(() => {return intdict.ContainsKey(varname) ? (intdict[varname] <= a && b <= intdict[varname] ? NextCmd : PrevCmd) : NextCmd;  }));
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
        functionlist.Add(new Func<int>(() => {return floatdict.ContainsKey(varname) ? (floatdict[varname] == b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitNotEqual(string varname, float b){
        functionlist.Add(new Func<int>(() => {return floatdict.ContainsKey(varname) ? (floatdict[varname] != b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitGreaterThan(string varname, float b){
        functionlist.Add(new Func<int>(() => {return floatdict.ContainsKey(varname) ? (floatdict[varname] > b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitLessThan(string varname, float b){
        functionlist.Add(new Func<int>(() => {return floatdict.ContainsKey(varname) ? (floatdict[varname] < b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitGreaterThanEqual(string varname, float b){
        functionlist.Add(new Func<int>(() => {return floatdict.ContainsKey(varname) ? (floatdict[varname] >= b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitLessThanEqual(string varname, float b){
        functionlist.Add(new Func<int>(() => {return floatdict.ContainsKey(varname) ? (floatdict[varname] <= b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitInRange(string varname, float a, float b){
        functionlist.Add(new Func<int>(() => {return floatdict.ContainsKey(varname) ? (a <= floatdict[varname] && floatdict[varname] <= b ? NextCmd : PrevCmd) : NextCmd;  }));
    }
    protected void WaitOutOfRange(string varname, float a, float b){
        functionlist.Add(new Func<int>(() => {return floatdict.ContainsKey(varname) ? (floatdict[varname] <= a && b <= floatdict[varname] ? NextCmd : PrevCmd) : NextCmd;  }));
    }
}
