using UnityEngine;


public class GazeXRRayInteractor : UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor
{
    void Start()
    {
        // Set the ray origin to the main camera
        if (Camera.main != null)
        {
            this.rayOriginTransform = Camera.main.transform;
        }
    }
} 