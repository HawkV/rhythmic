using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FretController : MonoBehaviour
{
    private List<GameObject> frets;

    void Start()
    {
        EventManager.StartListening(EventManager.Event.NotePressed, HightlightFret);    
    }

    public void Init(List<GameObject> frets) {
        this.frets = frets;
    }

    void HightlightFret(int index, bool isHighlighted) {
        var fretHighlighter = frets[index].GetComponent<HighlightController>();

        if (isHighlighted) {
            fretHighlighter.SetHighlighted();
        } else {
            fretHighlighter.SetDefault();
        }
    }

    void Update()
    {
        
    }
}
