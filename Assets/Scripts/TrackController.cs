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

    private ConcurrentQueue<TimedChord> chords = new();
    private List<TimedChord> chordList = new(); // duplicating collection for display purposes

    [SerializeField]
    private NoteSpawnController noteSpawnController;

    void Awake()
    {
        var A = new Note("A");
        var B = new Note("B");
        var C = new Note("C");
        var D = new Note("D");
        var E = new Note("E");

        AddChord(new Chord(new int[]{1, 2, 5}), 5f, 1f);

    

        EventManager.StartListening(EventManager.Event.ChordPlayed, PlayChord);
        EventManager.StartListening(EventManager.Event.LoadingStatusChanged, TrackStart);
    }
    
    public void TrackStart(LoadingStatus status) {
        if (status != LoadingStatus.Completed) {
            return;
        }

        chordList.Sort((TimedChord a, TimedChord b) => {
            if (a.start < b.start) return 1;
            if (a.start == b.start) return 0;
            return -1;
        });
        
        chordList.ForEach((TimedChord c) => {
            chords.Enqueue(c);
        });

        audioPlayer.Play();

        StartCoroutine(AutoEvict());
    }

    IEnumerator AutoEvict() {
        if (chords.IsEmpty) {
            yield break;
        }

        chords.TryPeek(out var chord);

        if (audioPlayer.Time() >= chord.end) {
            chords.TryDequeue(out _);
            Debug.Log("miss");
        }

        chords.TryPeek(out chord);

        yield return new WaitForSeconds(chord.end - audioPlayer.Time());
    }

    void PlayChord(Chord chord) {
        if (chords.IsEmpty) {
            return;
        }

        chords.TryPeek(out var currentChord);

        var time = audioPlayer.Time();
        if (time < currentChord.start) {
            return;
        }

        if (time <= currentChord.end) {
            if (currentChord.chord.Equals(chord)) {
                Debug.Log("hit");
            } else {
                Debug.Log("miss");
            }

            chords.TryDequeue(out _);
        }
    }

    // 
    public void AddChord(Chord chord, float time, float threshold) {
        var c = new TimedChord(
            chord, 
            start: time - threshold,
            end: time + threshold
        );
        
        chordList.Add(c);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
