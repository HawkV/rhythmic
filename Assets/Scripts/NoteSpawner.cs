using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    private GameObject note, despawner;

    private Color color;

    private float speed;

    public void Init(float speed, Color color, GameObject note, GameObject despawner) {
        this.speed = speed;
        this.color = color;
        this.despawner = despawner;
        this.note = note;
    }   

    public GameObject Spawn() {
        var obj = Instantiate(note, transform, worldPositionStays:false);
        obj.SetActive(true);
        obj.GetComponent<NoteController>().Init(transform.position, despawner.transform.position, speed);
        obj.GetComponent<HighlightController>().Init(color);

        return obj;
    }   
}
