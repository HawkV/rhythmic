using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public record class Chord {
    int noteFlags;

    public Chord() {}

    public Chord(IEnumerable<int> noteIDs) {
        foreach (var note in noteIDs) {
            // for each note we set a corresponding bit to 1 in noteFlags variable
            // at the start noteFlags is 0 so flip(0) = 1
            FlipNote(note); 
        }
    }

    // Invert isPressed value for a specified index
    public void FlipNote(int index) {
        // x xor 0 = x; x xor 1 = !x 
        noteFlags ^= 1 << index;
    }

    public void SetNote(int index, bool value) {
        var offset = 1 << index;
        if (value) {
            noteFlags |= offset;
        } else {
            noteFlags &= ~offset;
        }
    }

    public bool GetNote(int index) {
        return (noteFlags & (1 << index)) != 0;
    }

    public List<int> GetPressedNotes() {
        var notes = new List<int>();
        var currentIndex = 0;
        var tmp = noteFlags;

        while(tmp != 0) {
            if ((tmp & 1) != 0) {
                notes.Add(currentIndex);
            }

            currentIndex++;
            tmp >>= 1;
        }

        return notes;
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