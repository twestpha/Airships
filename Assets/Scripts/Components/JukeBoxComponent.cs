using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;

[RequireComponent(typeof(AudioSource))]
public class JukeBoxComponent : MonoBehaviour {

    public AudioSource jukeBoxAudioSource;

    public float jukeBoxMaxVolume = 1.0f;

    public TrackListData[] trackLists;

    private AudioClip queuedClip;
    private Timer fadeTimer;

    public void Start(){
        jukeBoxAudioSource.volume = jukeBoxMaxVolume;
        fadeTimer = new Timer(1.0f);
    }

    void Update(){
        if(queuedClip && !fadeTimer.Finished()){
            jukeBoxAudioSource.volume = (1.0f - fadeTimer.Parameterized()) * jukeBoxMaxVolume;
        } else if(queuedClip && fadeTimer.Finished()){
            jukeBoxAudioSource.clip = queuedClip;
            jukeBoxAudioSource.Play();
            queuedClip = null;
            fadeTimer.Start();
        } else if(!queuedClip && !fadeTimer.Finished()){
            jukeBoxAudioSource.volume = fadeTimer.Parameterized() * jukeBoxMaxVolume;
        }
    }

    public void PlaySong(string trackListName, int trackNumber){
        for(int i = 0; i < trackLists.Length; ++i){
            if(trackLists[i].name == trackListName){
                if(trackNumber < 0 || trackNumber > trackLists[i].tracks.Length - 1){
                    Debug.LogError("Track Number " + trackNumber + " is invalid for tracklist " + trackListName);
                    return;
                }

                PlayAudioClip(trackLists[i].tracks[trackNumber]);

                return;
            }
        }

        Debug.LogError("Track List " + trackListName + " is invalid");
    }

    // playtracklist(tracklist)

    private void PlayAudioClip(AudioClip clip){
        if(jukeBoxAudioSource.isPlaying){
            queuedClip = clip;
            fadeTimer.Start();
        } else {
            jukeBoxAudioSource.clip = clip;
            jukeBoxAudioSource.Play();
        }
    }
}
