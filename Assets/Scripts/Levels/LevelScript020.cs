using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelScript020 : LevelScriptBase {

    [Header("Audio Clips")]
    public AudioClip tower_joke;
    public AudioClip captain_takeoff;
    public AudioClip tower_cleared;
    public AudioClip tower_goodluck;
    public AudioClip angus_request;
    public AudioClip captain_copy;
    public AudioClip angus_intro;
    public AudioClip rory_but;
    public AudioClip hugo_intro;
    public AudioClip captain_charmed;
    public AudioClip captain_ordercodes;
    public AudioClip tower_ordercodes;
    public AudioClip captain_rogercodes;
    public AudioClip captain_radiotower;
    public AudioClip angus_sneaky;
    public AudioClip captain_theresmore;
    public AudioClip rory_dress;
    public AudioClip angus_lookbetter;
    public AudioClip captain_goodwork;
    public AudioClip hugo_niceshot;
    public AudioClip angus_oncemore;
    public AudioClip rory_lorriesspotted;
    public AudioClip captain_letthemhaveit;
    public AudioClip angus_blow;
    public AudioClip captain_settledown;
    public AudioClip tower_reportin;
    public AudioClip captain_reporting;
    public AudioClip tower_contacts;
    public AudioClip captin_willdo;
    public AudioClip captain_formup;
    public AudioClip angus_freshmeat;
    public AudioClip captain_alright;
    public AudioClip hugo_roger;
    public AudioClip angus_bombers;
    public AudioClip rory_makesure;
    public AudioClip angus_cornereddogs;
    public AudioClip hugo_twiceshy;
    public AudioClip tower_airship;
    public AudioClip rory_eyes;
    public AudioClip angus_boltcastle;
    public AudioClip hugo_impressive;
    public AudioClip captain_steady;
    public AudioClip angus_artillery;
    public AudioClip captain_downslowly;
    public AudioClip rory_sad;
    public AudioClip angus_leave;
    public AudioClip hugo_niceshooting;
    public AudioClip captain_supper;

    [Header("GameObject Prefabs")]
    public GameObject balloonPrefab;
    public GameObject cargoPlanePrefab;

    protected override void Progression(){
        EnableAirplaneThrottle(player, false);
        EnableAirplaneGuns(player, false);

        EnableBriefingMode(maincamera, true);
        WaitForMouseClick();
        EnableBriefingMode(maincamera, false);

        Delay(1.0f);
        Transmission(tower_joke, wait, useradio);
        Delay(0.5f);
        Transmission(captain_takeoff, wait, dontuseradio);
        Delay(0.5f);
        EnableAirplaneThrottle(player, true);
        EnableAirplaneGuns(player, true);
        Transmission(tower_cleared, wait, useradio);
        Delay(5.0f);
        Transmission(tower_goodluck, wait, useradio);

        // wait until near flight group

        


        // LoadLevel("020_blitz");
    }
}
