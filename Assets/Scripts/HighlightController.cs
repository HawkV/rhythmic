using UnityEngine;

public class HighlightController : MonoBehaviour
{
    [SerializeField]
    private Material Default, Highlighted, Deactivated;
    private Material _default, _highlighted, _deactivated;

    [SerializeField]
    private Renderer meshRenderer;

    public void Init(Color color) {
        // copying base materials into new variables to avoid changing properties of the original ones  
        _default = new Material(Default);
        _highlighted = new Material(Highlighted);
        _deactivated = new Material(Deactivated);

        // TODO: different colors for different materials?
        _default.color = color;
        _highlighted.color = color;
        _highlighted.SetColor("_EmissionColor", color);
        
        SetDefault();
    }

    public void SetHighlighted() {
        meshRenderer.material = _highlighted;
    }

    public void SetDefault() {
        meshRenderer.material = _default; 
    }

    public void SetDeactivated() {
        meshRenderer.material = _deactivated; 
    }
}
