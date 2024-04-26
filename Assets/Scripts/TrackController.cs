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

    private Dictionary<Note, NoteSpawner> spawners;

    void Awake()
    {
        // AddChord(new Chord(new BitVector32(0b00001)), 10f, 10f);
        audioPlayer.LoadingStatusChanged += TrackStart;
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

    public void PlayChord(Chord chord) {
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
