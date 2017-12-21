using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelScript010 : LevelScriptBase {
    [Header("Audio Clips")]
    public AudioClip tower_intro;
    public AudioClip captain_intro;
    public AudioClip tower_starttesting;
    public AudioClip tower_allgreen;
    public AudioClip tower_instruments;
    public AudioClip captain_confirmspeed;
    public AudioClip tower_controlstest;
    public AudioClip captain_confirmcontrols;
    public AudioClip tower_headsouth;
    public AudioClip captain_headsouth;
    public AudioClip tower_interjection;
    public AudioClip tower_barrageballoons;
    public AudioClip tower_headwest;
    public AudioClip captain_ballons;
    public AudioClip tower_nice;
    public AudioClip captain_handles;
    public AudioClip bogey_intro;
    public AudioClip tower_warning;
    public AudioClip tower_headeast;
    public AudioClip tower_warning2;
    public AudioClip bogey_intro2;
    public AudioClip tower_openfire;
    public AudioClip captain_confirmkill;
    public AudioClip tower_allclear;
    public AudioClip captain_landing;

    [Header("GameObject Prefabs")]
    public GameObject balloonPrefab;
    public GameObject cargoPlanePrefab;

    protected override void Progression(){

        // fullscreen ui briefing

        // fade in

        EnableAirplaneThrottle(player, false);
        EnableAirplaneGuns(player, false);
        Delay(3.0f);
        Transmission(tower_intro, true);
        Delay(0.5f);
        Transmission(captain_intro, true);
        Delay(0.7f);
        Transmission(tower_starttesting, true);

        Print("[A] and [S] to change throttle");

        SetVarToThrottle(player, "throttle");
        WaitGreaterThan("throttle", 0.8f);

        Delay(0.3f);
        Transmission(tower_allgreen, true);
        Delay(0.5f);
        EnableAirplaneThrottle(player, true);

        Print("[Up] and [Down] to pitch plane");

        SetVarToAirspeed(player, "airspeed");
        WaitGreaterThan("airspeed", 85.0f);

        Transmission(tower_instruments, true);
        Delay(0.3f);
        Transmission(captain_confirmspeed, true);
        Delay(1.2f);
        Print("[Left] and [Right] to yaw plane");
        Transmission(tower_controlstest, true);
        Delay(0.6f);
        Transmission(captain_confirmcontrols, true);
        Delay(1.3f);
        Print("[Q] and [E] to roll plane");
        Transmission(tower_headsouth, true);

        Print("Instruments are for heading");

        SetVarToHeading(player, "heading");
        WaitInRange("heading", 135.0f, 225.0f); // South

        Delay(0.6f);
        Transmission(captain_headsouth, true);

        CreateObjectAtPosition(balloonPrefab, new Vector3(-834.0f, 0.0f, 162.0f));
        Delay(0.6f);
        CreateObjectAtPosition(balloonPrefab, new Vector3(-721.0f, 0.0f, 9.0f));
        Delay(0.3f);
        CreateObjectAtPosition(balloonPrefab, new Vector3(-866.0f, 0.0f, -45.0f));
        CreateObjectAtPosition(balloonPrefab, new Vector3(-1066.0f, 0.0f, 125.0f));
        Delay(0.8f);
        CreateObjectAtPosition(balloonPrefab, new Vector3(-1012.0f, 0.0f, -111.0f));
        Delay(0.7f);
        CreateObjectAtPosition(balloonPrefab, new Vector3(-735.0f, 0.0f, -55.0f));

        Transmission(tower_interjection, true);
        Delay(1.2f);
        Transmission(tower_barrageballoons, true);
        Delay(0.7f);
        Transmission(tower_headwest, true);
        EnableAirplaneGuns(player, true);
        Print("[Left Mouse] to fire main weapon");

        SetVarToBalloonKills("balloonkills");
        WaitGreaterThanEqual("balloonkills", 1);
        Print("Balloons Deflated 1/6");

        SetVarToBalloonKills("balloonkills");
        WaitGreaterThanEqual("balloonkills", 2);
        Print("Balloons Deflated 2/6");

        SetVarToBalloonKills("balloonkills");
        WaitGreaterThanEqual("balloonkills", 3);
        Print("Balloons Deflated 3/6");

        SetVarToBalloonKills("balloonkills");
        WaitGreaterThanEqual("balloonkills", 4);
        Print("Balloons Deflated 4/6");

        SetVarToBalloonKills("balloonkills");
        WaitGreaterThanEqual("balloonkills", 5);
        Print("Balloons Deflated 5/6");

        SetVarToBalloonKills("balloonkills");
        WaitGreaterThanEqual("balloonkills", 6);
        Print("Balloons Deflated 6/6");

        Delay(1.3f);
        Transmission(captain_ballons, true);
        Delay(0.8f);
        Transmission(tower_nice, true);
        Delay(0.3f);
        CreateObjectAtPosition(cargoPlanePrefab, new Vector3(1058.0f, 335.0f, -131.0f));
        Transmission(captain_handles, true);
        Transmission(bogey_intro, true);
        Delay(0.5f);
        Transmission(tower_warning, true);
        Delay(0.4f);
        Transmission(tower_headeast, true);
        Delay(0.1f);
        Transmission(tower_warning2, true);
        Delay(0.7f);
        Transmission(bogey_intro2, true);

        SetVarToHeading(player, "heading");
        WaitInRange("heading", 45.0f, 315.0f); // East

        Delay(0.8f);
        Transmission(tower_openfire, true);
        Print("Destroy Hostile Aircraft");


        // wait for interceptors destroyed

        Delay(0.4f);
        Transmission(captain_confirmkill, true);
        Delay(0.4f);
        Transmission(tower_allclear, true);
        Delay(0.7f);
        Transmission(captain_landing, true);

        Print("Land the plane");

        // wait until landed

        LoadLevel("020_blitz");
    }
}
