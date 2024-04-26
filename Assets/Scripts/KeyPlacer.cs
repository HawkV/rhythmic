using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class NotePlacer : MonoBehaviour
{   
    [SerializeField] 
    private bool ResetToggle;

    [SerializeField]
    private float Margin;

    [SerializeField]
    private GameObject Key, Separator, Spawner, Despawner;
    
    [SerializeField]
    private Material Default, Highlighted, Deactivated;

    [Serializable]
    public struct NamedColor {
        public string name;
        public Color color;
    }

    public NamedColor[] colors;

    [HideInInspector]
    public Dictionary<string, KeyController> notes = new();

    private List<GameObject> separators = new();

    private List<GameObject> spawners = new();
    private List<GameObject> despawners = new();

    private List<GameObject> bars = new();

    void Awake() {
        Reset();
    }

    void Init() {
        bars = new();  
        for(int i = 0; i < colors.Length; i++) {
            var bar = new GameObject("Bar" + i);
            bar.transform.parent = this.transform;
            bars.Add(bar);
        }

        notes = new();  

        for(int i = 0; i < colors.Length; i++) {
            var noteObj = Instantiate(Key, transform, worldPositionStays: false);
            
            var note = noteObj.GetComponent<KeyController>();
            note.Init(Default, Highlighted, Deactivated, colors[i].color);
            
            notes[colors[i].name] = note; 
            noteObj.transform.parent = bars[i].transform;
        }

        
        separators = new();  
        for(int i = 0; i < colors.Length - 1; i++) {
            separators.Add(Instantiate(Separator, transform, worldPositionStays: false));
        }

        despawners = new();  
        for(int i = 0; i < colors.Length; i++) {
            despawners.Add(Instantiate(Despawner, transform, worldPositionStays: false));
            despawners[i].transform.parent = bars[i].transform;
        }

        spawners = new();  
        for(int i = 0; i < colors.Length; i++) {
            spawners.Add(Instantiate(Spawner, transform, worldPositionStays: false));
            spawners[i].transform.parent = bars[i].transform;
            spawners[i].GetComponent<NoteSpawner>().Init(despawners[i]);
        }

    }

    void CleanUp() {
        foreach (var item in GetComponentsInChildren<KeyController>()) {
            DestroyImmediate(item.gameObject);
        }

        foreach (var item in separators) {
            DestroyImmediate(item);
        }

        foreach (var item in spawners) {
            DestroyImmediate(item);
        }

        foreach (var item in despawners) {
            DestroyImmediate(item);
        }

        foreach (var item in bars) {
            DestroyImmediate(item);
        }
    }

    void Reset() {
        CleanUp();
        Init();
    }

    void Update() {
        if (notes == null || ResetToggle) {
            Reset();

            ResetToggle = false;
        }

        PlaceKeys();
        PlaceSeparators();
        PlaceSpawners();
        PlaceDespawners();
    }

    void PlaceKeys() {
        var center = transform.position;

        var len = notes.Count;
        var segmentLength = (len - 1) * Margin;
        var leftBound = - segmentLength / 2;

        for (int i = 0; i < len; i++) {
            var noteName = colors[i].name;

            var note = notes[noteName]; 
            
            if (note != null) {
                note.transform.position = new Vector3(leftBound + Margin * i, 0, 0) + center;
            }
        }   
    }

    void PlaceSeparators() {
        var center = transform.position;

        var len = notes.Count - 1;
        var segmentLength = len * Margin;
        var leftBound = (Margin - segmentLength) / 2;

        for (int i = 0; i < len; i++) {
            var separator = separators[i]; 
            
            if (separator != null) {
                separator.transform.position = new Vector3(leftBound + Margin * i, 0, 0) + center;
            }
        }   
    }

    void PlaceSpawners() {
        var center = Spawner.transform.position;

        var len = spawners.Count;
        var segmentLength = (len - 1) * Margin;
        var leftBound = - segmentLength / 2;

        for (int i = 0; i < len; i++) {
            var spawner = spawners[i]; 
            
            if (spawner != null) {
                spawner.transform.position = new Vector3(leftBound + Margin * i, 0, 0) + center;
            }
        }   
    }

    void PlaceDespawners() {
        var center = Despawner.transform.position;

        var len = spawners.Count;
        var segmentLength = (len - 1) * Margin;
        var leftBound = - segmentLength / 2;

        for (int i = 0; i < len; i++) {
            var despawner = despawners[i]; 
            
            if (despawner != null) {
                despawner.transform.position = new Vector3(leftBound + Margin * i, 0, 0) + center;
            }
        }   
    }
}
