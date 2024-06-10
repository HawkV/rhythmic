using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FretsController : MonoBehaviour
{
    private List<FretController> frets;

    void Start()
    {
        EventManager.StartListening(EventManager.Event.NotePressed, HightlightFret);
        EventManager.StartListening(EventManager.Event.ChordHit, AnimateFrets);    
        EventManager.StartListening(EventManager.Event.ChordPlayed, HighlightPressedFrets);    
    }

    public void Init(List<FretController> frets) {
        this.frets = frets;
    }

    void HightlightFret(int index, bool isHighlighted) {
        if (isHighlighted) {
            frets[index].HighlightOuter();
        } else {
            frets[index].DefaultOuter();
        }
    }

    void AnimateFrets(Chord chord, bool isHit) {
        if (isHit) {
            var pressedNotes = chord.GetPressedNotes();
            var pressedFrets = pressedNotes.Select(note => frets[note]).ToList();
            Debug.Log("effect?");
            
            pressedFrets.ForEach(fret => fret.PlayEffects());
        }
    }

    void HighlightPressedFrets(Chord chord, bool isStarted) {
        // we may depress the fret key with another configuration of pressed notes
        // so just in case we un-highlight every fret
        if (!isStarted) { 
            frets.ForEach(fret => {
                fret.DefaultInner();
            });
            return;
        }

        var pressedNotes = chord.GetPressedNotes();
        var pressedFrets = pressedNotes.Select(note => frets[note]).ToList();

        pressedFrets.ForEach(fret => {
            fret.HighlightInner();
            fret.HighlightOuter();
        });
    }

    void Update()
    {
        
    }
}
