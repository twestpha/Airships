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
    public AudioClip tower_headeast;
    public AudioClip captain_ballons;
    public AudioClip tower_nice;
    public AudioClip captain_handles;
    public AudioClip bogey_intro;
    public AudioClip tower_warning;
    public AudioClip tower_headwest;
    public AudioClip tower_warning2;
    public AudioClip tower_openfire;
    public AudioClip captain_confirmkill;
    public AudioClip tower_allclear;
    public AudioClip captain_landing;

    protected override void Progression(){
        EnableAirplaneThrottle(player, false);
        EnableAirplaneGuns(player, false);
        Delay(3.0f);
        Transmission(tower_intro, true);
        Delay(0.5f);
        Transmission(captain_intro, true);
        Delay(0.7f);
        Transmission(tower_starttesting, true);

        SetVarToThrottle(player, "throttle");
        WaitGreaterThan("throttle", 0.8f);

        Transmission(tower_allgreen, true);
        Delay(0.5f);
        EnableAirplaneThrottle(player, true);

        SetVarToAirspeed(player, "airspeed");
        WaitGreaterThan("airspeed", 85.0f);

        Transmission(tower_instruments, true);
        Delay(0.3f);
        Transmission(captain_confirmspeed, true);
        Delay(1.2f);
        Transmission(tower_controlstest, true);
        Delay(0.6f);
        Transmission(captain_confirmcontrols, true);
        Delay(1.3f);
        Transmission(tower_headsouth, true);

        // wait for heading south

        Transmission(captain_headsouth, true);
        Delay(5.3f);
        Transmission(tower_interjection, true);
        Delay(1.2f);
        Transmission(tower_barrageballoons, true);
        EnableAirplaneGuns(player, true);
        Delay(0.7f);
        Transmission(tower_headeast, true);

        // wait for balloons destroyed

        Transmission(captain_ballons, true);
        Delay(0.8f);
        Transmission(tower_nice, true);
        Delay(0.3f);
        Transmission(captain_handles, true);
        Transmission(bogey_intro, true);
        Delay(0.5f);
        Transmission(tower_warning, true);
        Delay(0.4f);
        Transmission(tower_headwest, true);
        Delay(0.1f);
        Transmission(tower_warning2, true);

        // wait for heading west

        Transmission(tower_openfire, true);

        // wait for interceptors destroyed

        Transmission(captain_confirmkill, true);
        Delay(0.4f);
        Transmission(tower_allclear, true);
        Delay(0.7f);
        Transmission(captain_landing, true);

        // wait until landed

        // end level
    }
}
