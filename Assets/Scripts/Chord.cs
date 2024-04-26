using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public record class Chord {
    int noteFlags;

    public Chord(List<Note> noteList, Dictionary<Note, int> noteIDs) {
        foreach (Note note in noteList) {
            noteFlags  |= 1 << noteIDs[note]; // for each note we flip a corresponding bit in noteFlags variable
        }
    }
}

public record class TimedChord {
    public float start; // earliest valid trigger time from track start in seconds
    public float end; // latest valid trigger time from track start in seconds

    public Chord chord;

    public TimedChord(Chord chord, float start, float end) {
        this.chord = chord;
        this.start = start;
        this.end = end;
    }
}