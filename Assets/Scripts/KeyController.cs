using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private Material Default, Highlighted, Deactivated;

    [SerializeField]
    private Renderer meshRenderer;

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(Material Default, Material Highlighted, Material Deactivated, Color color) {
        this.Default = new Material(Default);
        this.Highlighted = new Material(Highlighted);
        this.Deactivated = new Material(Deactivated);

        this.Default.color = color;
        this.Highlighted.color = color;
        this.Highlighted.SetColor("_EmissionColor", color);
        
        SetDefault();
    }

    public void SetHighlighted(bool highlighted) {
        if (highlighted) {
            meshRenderer.material = Highlighted;
        } else {
            SetDefault();
        } 
    }

    public void SetDefault() {
        meshRenderer.material = Default; 
    }

    public void SetDeactivated() {
        meshRenderer.material = Deactivated; 
    }
}
