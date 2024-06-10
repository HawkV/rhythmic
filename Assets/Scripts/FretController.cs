using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FretController : MonoBehaviour
{
    [SerializeField]
    HighlightController outerHighlight, innerHighlight;

    [SerializeField]
    EffectController effectController;

    public void Init(Color color)
    {
        outerHighlight.Init(color);
        innerHighlight.Init(color);
        effectController.Init(color);
    }

    public void HighlightInner() {
        innerHighlight.SetHighlighted();
    }

    public void HighlightOuter() {
        outerHighlight.SetHighlighted();
    }

    public void DefaultInner() {
        innerHighlight.SetDefault();
    }

    public void DefaultOuter() {
        outerHighlight.SetDefault();
    }

    public void PlayEffects() {
        effectController.Play();
    }
}
