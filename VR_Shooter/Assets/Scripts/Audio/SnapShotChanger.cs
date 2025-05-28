using UnityEngine;
using UnityEngine.Audio;

public class SnapShotChanger : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot tunnelIn;
    [SerializeField] private AudioMixerSnapshot tunnelOut;
    private void OnTriggerEnter(Collider other)
    {
        switch(other.gameObject.tag)
        {
            case "TunnelInside":
                tunnelIn.TransitionTo(0.1f);
                break;
            case "TunnelOutside":
                tunnelOut.TransitionTo(0.1f);
                break;
        }
    }
}
