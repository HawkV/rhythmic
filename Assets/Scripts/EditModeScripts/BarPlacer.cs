using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class BarPlacer : MonoBehaviour
{   
    [SerializeField]
    private NoteInfo noteInfo;

    [SerializeField]
    private float Margin;

    [SerializeField]
    private FretsController fretController;

    [SerializeField]
    private GameObject Fret, Separator;

    [SerializeField]
    private Transform Spawner, Despawner;

    private List<GameObject> _spawners, _despawners, _bars, _separators, _frets;
    private List<GameObject> spawners { 
        get {
            if (_spawners == null || _spawners.Count == 0) {
                _spawners = gameObject.GetDescendantsWithPrefix(spawnerPrefix);
                // after cutting off the prefix all that remains is an index
                // we convert it into integer and sort by it
                _spawners = _spawners.OrderBy(x => int.Parse(x.name[spawnerPrefix.Length..])).ToList();
            }
    
            return _spawners;
        }
        set {
            _spawners = value;
        }
    } 
    private List<GameObject> despawners {
        get {
            if (_despawners == null || _despawners.Count == 0) {
                _despawners = gameObject.GetDescendantsWithPrefix(despawnerPrefix);
                _despawners = _despawners.OrderBy(x => int.Parse(x.name[despawnerPrefix.Length..])).ToList();
            }

            return _despawners;
        } 
        set {
            _despawners = value;
        }
    }
    private List<GameObject> frets {
        get {
            if (_frets == null || _frets.Count == 0) {
                _frets = gameObject.GetDescendantsWithPrefix(fretPrefix);
                _frets = _frets.OrderBy(x => int.Parse(x.name[fretPrefix.Length..])).ToList();
            }

            return _frets;
        } 
        set {
            _frets = value;
        }
    }
    private List<GameObject> bars { 
        get {
            if (_bars == null || _bars.Count == 0) {
                _bars = gameObject.GetDescendantsWithPrefix(barPrefix);
            }

            return _bars;
        }
        set {
            _bars = value;
        }
    } 
    private List<GameObject> separators {
        get {        
            if (_separators == null || _separators.Count == 0) { 
                _separators = gameObject.GetDescendantsWithPrefix(separatorPrefix);
            }
            
            return _separators;
        }    
        set {
            _separators = value;
        }
    }

    const string spawnedPrefix = "(spawned)";
    readonly string barPrefix = string.Format("{0} bar ", spawnedPrefix);
    readonly string fretPrefix = string.Format("{0} fret ", spawnedPrefix);
    readonly string separatorPrefix = string.Format("{0} separator ", spawnedPrefix);
    readonly string spawnerPrefix = string.Format("{0} spawner ", spawnedPrefix);
    readonly string despawnerPrefix = string.Format("{0} despawner ", spawnedPrefix);

    private void Start() {
        if (!Application.isPlaying) {
            return;
        }

        var fretControllers = frets.Select(f => f.GetComponent<FretController>()).ToList();
        for (int i = 0; i < fretControllers.Count; i++) {
            Debug.Log(fretControllers[i]);
            Debug.Log(noteInfo.noteDescription[i].color);
            fretControllers[i].Init(noteInfo.noteDescription[i].color);
        }    

        fretController.Init(fretControllers);

        Spawner.GetComponent<NoteSpawnController>().Init(spawners, despawners, noteInfo.noteDescription);
    }

    private void OnValidate()
    {
        PlaceItems();
    }

    public void Init() {
        CleanUp();

        if (!noteInfo) {
            Debug.Log("noteInfo is not set");
            return;
        }

        // instantiate individual bars
        for(int i = 0; i < noteInfo.noteDescription.Length; i++) {
            // instantiate parent bar object
            var bar = InstantiateEmpty(transform.position, transform.rotation, transform);
            bar.name = string.Format("{0}{1}", barPrefix, i);

            // instantiate fret
            var fretObj = Instantiate(Fret, transform.position, transform.rotation, bar.transform);
            fretObj.name = string.Format("{0}{1}", fretPrefix, i);

            // instantiate spawner
            var spawner = InstantiateEmpty(Spawner.position, transform.rotation, bar.transform);
            spawner.name = string.Format("{0}{1}", spawnerPrefix, i);

            // instantiate despawner
            var despawner = InstantiateEmpty(Despawner.position, transform.rotation, bar.transform);
            despawner.name = string.Format("{0}{1}", despawnerPrefix, i);
        }
  
        // instantiate bar separators
        for(int i = 0; i < noteInfo.noteDescription.Length - 1; i++) {
            var separator = Instantiate(Separator, transform);
            separator.name = string.Format("{0}{1}", separatorPrefix, i);
        }

        PlaceItems();
    }  

    void DeleteSpawnedObjects() {
        gameObject.GetDescendantsWithPrefix(spawnedPrefix).ForEach(item => {
            if (Application.isPlaying) {
                Destroy(item);
            } else {
                DestroyImmediate(item);
            }
        });
    }

    void CleanUp() {
        DeleteSpawnedObjects();

        bars = new();
        spawners = new();
        despawners = new();
        separators = new();
    }

    void PlaceItems() {
        PlaceBars();
        PlaceSeparators();
    }

    void PlaceBars() {
        var center = transform.position;
        
        var len = bars.Count;
        var segmentLength = (len - 1) * Margin;
        var leftBound = - segmentLength / 2;

        for (int i = 0; i < len; i++) {
            bars[i].transform.position = new Vector3(leftBound + Margin * i, 0, 0) + center;
        }   
    }

    void PlaceSeparators() {
        var center = transform.position;

        var len = separators.Count;
        var segmentLength = len * Margin;
        var leftBound = (Margin - segmentLength) / 2;

        for (int i = 0; i < len; i++) {
            separators[i].transform.position = new Vector3(leftBound + Margin * i, 0, 0) + center;
        }   
    }

    GameObject InstantiateEmpty(Vector3 position, Quaternion rotation, Transform parent) {
        var obj = new GameObject();

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.transform.parent = parent;

        return obj;
    }

    void OnDestroy() {
        CleanUp();
    }
}
