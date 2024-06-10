using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using Unity.Loading;
using System.IO;
using UnityEngine;

public class TrackController : MonoBehaviour
{
    [SerializeField]
    private AudioPlayer audioPlayer;

    [SerializeField]
    private float speed;

    private float spawnHeadstart;

    [SerializeField]
    private GameObject Spawner, Despawner, FretBar;

    private int chordToPlay, chordToSpawn;
    private List<TimedChord> chordList = new();
    private Dictionary<TimedChord, ChordController> spawnedChordList = new();

    [SerializeField]
    private NoteSpawnController noteSpawnController;

    void Awake()
    {
        AddChord(new Chord(new int[]{0, 1, 4}), 3f, 0.1f);
        AddChord(new Chord(new int[]{0, 2, 3}), 3.5f, 0.1f);
        AddChord(new Chord(new int[]{1, 2, 3}), 4f, 0.1f);
        AddChord(new Chord(new int[]{1, 2, 4}), 4.5f, 0.1f);
        AddChord(new Chord(new int[]{2, 3, 4}), 8f, 0.1f);
        AddChord(new Chord(new int[]{0, 2, 3}), 9f, 0.1f);
        AddChord(new Chord(new int[]{2, 3, 4}), 10f, 0.1f);
        AddChord(new Chord(new int[]{1, 2, 4}), 11f, 0.1f);

        // we need to spawn the chords <spawnHeadstart> time before the trigger time 
        // for them to be able to travel the entire distance from start to finish
        
        // for now "speed" is actually 1/(total travel time in seconds from spawner to despawner)
        // but what we need is total travel time from spawner to fret bar
        // we get that value by calculating the ratio of (distance from start to bar) to (total distnace) and multiplying our total time by it
        spawnHeadstart = (1/speed) * Vector3.Distance(Spawner.transform.position, FretBar.transform.position)/Vector3.Distance(Spawner.transform.position, Despawner.transform.position); 
        
        Debug.Log("spawn headstart, sec. : " + spawnHeadstart);

        EventManager.StartListening(EventManager.Event.ChordPlayed, PlayChord);
        EventManager.StartListening(EventManager.Event.LoadingStatusChanged, TrackStart);
    }
    
    public void TrackStart(LoadingStatus status) {
        if (status != LoadingStatus.Completed) {
            return;
        }

        chordToPlay = 0;
        chordToSpawn = 0;

        chordList.Sort((TimedChord a, TimedChord b) => {
            if (a.start < b.start) return -1;
            if (a.start == b.start) return 0;
            return 1;
        });

        audioPlayer.Play();
    }

    void EvictChord(float currentTime) {
        if (chordList.Count == 0 || chordToPlay >= chordList.Count) {
            return;
        }

        var chord = chordList[chordToPlay];

        if (currentTime >= chord.end) {
            chordToPlay++;

            spawnedChordList[chord].Deactivate();
        }
    }

    void PlayChord(Chord chord, bool isStarted) {
        if (!isStarted) {
            return;
        }
        
        if (chordList.Count == 0 || chordToPlay >= chordList.Count) {
            return;
        }

        var currentChord = chordList[chordToPlay];

        var time = audioPlayer.Time();

        if (time < currentChord.start) {
            return;
        }

        if (time <= currentChord.end) {
            var isHit = currentChord.chord.Equals(chord);

            if (isHit) {
                spawnedChordList[currentChord].Play();
                
                chordToPlay++;
            }

            EventManager.TriggerEvent(EventManager.Event.ChordHit, currentChord.chord, isHit);
        }
    }

    public void AddChord(Chord chord, float time, float threshold) {
        var c = new TimedChord(
            chord, 
            start: time - threshold,
            end: time + threshold,
            triggerTime: time
        );
        
        chordList.Add(c);
    }

    public void SpawnNextChord(float currentTime) {
        if (chordToSpawn >= chordList.Count) {
            return;
        }

        var timedChord = chordList[chordToSpawn];

        if (currentTime >= (timedChord.triggerTime - spawnHeadstart)) {
            spawnedChordList[timedChord] = noteSpawnController.SpawnChord(timedChord.chord);
            chordToSpawn++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var currentTime = audioPlayer.Time();

        SpawnNextChord(currentTime);
        EvictChord(currentTime);
    }
}
