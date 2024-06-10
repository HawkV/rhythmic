using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {
    [SerializeField]
    private ParticleSystem ps;

    public void Init(Color color) {
        var main = ps.main;

        color.a = 1;
        main.startColor = color;
    }

    public void Play() {
        ps.Clear();
        ps.Play();
    }
}