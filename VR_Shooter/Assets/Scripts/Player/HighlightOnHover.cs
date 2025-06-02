using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HighlightOnHover : MonoBehaviour
{
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer rend;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalMaterial = rend.material;
        
        // Try to auto-register to hover events if possible
        var interactable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRBaseInteractable>();
        if (interactable != null)
        {
            interactable.hoverEntered.AddListener(OnHoverEntered);
            interactable.hoverExited.AddListener(OnHoverExited);
        }
    }

    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (rend != null && highlightMaterial != null)
            rend.material = highlightMaterial;
    }

    public void OnHoverExited(HoverExitEventArgs args)
    {
        if (rend != null && originalMaterial != null)
            rend.material = originalMaterial;
    }
} 