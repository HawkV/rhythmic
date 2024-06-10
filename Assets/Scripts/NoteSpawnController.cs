using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NoteInfo;

public class NoteSpawnController : MonoBehaviour
{
    [SerializeField]
    private GameObject Note;

    [SerializeField]
    private float Speed;

    private List<NoteSpawner> noteSpawners;

    public void Init(List<GameObject> spawnerLocations, List<GameObject> despawnerLocations, NoteDescription[] noteDescriptions) {
        noteSpawners = new List<NoteSpawner>(spawnerLocations.Count);

        for (int i = 0; i < spawnerLocations.Count; i++) {
            var spawner = spawnerLocations[i];
            var despawner = despawnerLocations[i];
            var ns = spawner.AddComponent<NoteSpawner>();

            var note = Instantiate(Note);
            note.SetActive(false);

            ns.Init(Speed, noteDescriptions[i].color, note, despawner);
            noteSpawners.Add(ns);
        }
    }   

    public ChordController SpawnChord(Chord chord) {
        var chordObj = new GameObject();
        var spawnedNotes = new List<GameObject>();
        var pressedNotes = chord.GetPressedNotes();

        foreach (var preessedNote in pressedNotes) {
            spawnedNotes.Add(noteSpawners[preessedNote].Spawn());
        }

        var chordController = chordObj.AddComponent<ChordController>();
        chordController.Init(spawnedNotes);

        return chordController;
    }   
}
