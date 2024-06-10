using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChordController : MonoBehaviour
{
    private List<GameObject> notes;
    private List<HighlightController> noteHighlight;

    public void Init(List<GameObject> notes) {
        this.notes = notes;

        noteHighlight = notes.Select(note => note.GetComponent<HighlightController>()).ToList();
    }

    // Change chord display status to Deactivated
    public void Deactivate() {
        foreach (var highlight in noteHighlight) {
            highlight.SetDeactivated();
        }
    }

    public void Play() {
        Destroy();
    }

    public void Destroy() {
        foreach (var note in notes) {
            Destroy(note);
        }

        Destroy(this);
    }
}