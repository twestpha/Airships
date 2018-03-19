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

    [Header("Briefing Data")]
    public AudioClip slide_change;
    public Texture briefing1;
    public Texture briefing2;
    public Texture briefing3;
    public Texture briefing4;

    protected override void Progression(){

        EnableAirplaneThrottle(player, false);
        EnableAirplaneGuns(player, false);

        EnableBriefingMode(maincamera, true);

        SetBriefingModeTexture(maincamera, briefing1);
        Transmission(slide_change, dontwait, dontuseradio);
        Delay(0.1f);
        WaitForMouseClick();

        SetBriefingModeTexture(maincamera, briefing2);
        Transmission(slide_change, dontwait, dontuseradio);
        Delay(0.1f);
        WaitForMouseClick();

        SetBriefingModeTexture(maincamera, briefing3);
        Transmission(slide_change, dontwait, dontuseradio);
        Delay(0.1f);
        WaitForMouseClick();

        SetBriefingModeTexture(maincamera, briefing4);
        Transmission(slide_change, dontwait, dontuseradio);
        Delay(0.1f);
        WaitForMouseClick();

        EnableBriefingMode(maincamera, false);

        Delay(3.0f);
        Transmission(tower_intro, wait, useradio);
        Delay(0.5f);
        Transmission(captain_intro, wait, dontuseradio);
        Delay(0.7f);
        Transmission(tower_starttesting, wait, useradio);

        Print("[W] and [S] to change throttle");

        SetVarToThrottle(player, "throttle");
        WaitGreaterThan("throttle", 0.8f);

        Delay(0.3f);
        Transmission(tower_allgreen, wait, useradio);
        Delay(0.5f);
        EnableAirplaneThrottle(player, true);

        Print("[Up] and [Down] to pitch plane");

        Delay(3.0f);
        PlaySong("Master", 0);

        SetVarToAirspeed(player, "airspeed");
        WaitGreaterThan("airspeed", 85.0f);

        Transmission(tower_instruments, wait, useradio);
        Delay(0.3f);
        Transmission(captain_confirmspeed, wait, dontuseradio);
        Delay(1.2f);
        Print("[Left] and [Right] to yaw plane");
        Transmission(tower_controlstest, wait, useradio);
        Delay(0.6f);
        Transmission(captain_confirmcontrols, wait, dontuseradio);
        Delay(1.3f);
        Print("[Q] and [E] to roll plane");
        Transmission(tower_headsouth, wait, useradio);

        Print("Use instruments for heading");

        SetVarToHeading(player, "heading");
        WaitInRange("heading", 135.0f, 225.0f); // South

        Delay(0.6f);
        Transmission(captain_headsouth, wait, dontuseradio);

        SaveGame("010_flight_check");

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

        Transmission(tower_interjection, wait, useradio);
        Delay(1.2f);
        Transmission(tower_barrageballoons, wait, useradio);
        Delay(0.7f);
        Transmission(tower_headwest, wait, useradio);
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

        SaveGame("011_balloons_destroyed");

        Delay(1.3f);
        Transmission(captain_ballons, wait, dontuseradio);
        Delay(0.8f);
        Transmission(tower_nice, wait, useradio);
        Delay(0.3f);
        CreateObjectAtPositionWithRotation(cargoPlanePrefab, new Vector3(3000.0f, 335.0f, -131.0f), new Vector3(0.0f, -90.0f, 0.0f));
        Transmission(captain_handles, wait, dontuseradio);
        Transmission(bogey_intro, wait, useradio);
        Delay(0.5f);
        Transmission(tower_warning, wait, useradio);
        Delay(0.4f);
        Transmission(tower_headeast, wait, useradio);
        Delay(0.1f);
        Transmission(tower_warning2, wait, useradio);
        Delay(0.7f);
        Transmission(bogey_intro2, wait, useradio);

        SetVarToHeading(player, "heading");
        WaitInRange("heading", 45.0f, 315.0f); // East

        Delay(0.8f);
        Transmission(tower_openfire, true, useradio);
        Print("Destroy Hostile Aircraft");

        PlaySong("Master", 1);

        SetVarToInterceptorKills("interceptorkills");
        WaitGreaterThanEqual("interceptorkills", 1);

        Delay(0.4f);
        Transmission(captain_confirmkill, wait, dontuseradio);
        Delay(0.4f);
        Transmission(tower_allclear, wait, useradio);
        Delay(0.7f);
        Transmission(captain_landing, wait, dontuseradio);

        Print("Mission Complete!");

        Delay(1.5f);

        // TODO fade to black?

        LoadLevel("020_blitz");
    }
}
