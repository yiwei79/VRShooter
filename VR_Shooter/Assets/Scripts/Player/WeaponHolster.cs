using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WeaponHolster : MonoBehaviour
{
    private UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor socketInteractor;
    public AudioClip holsterSound;
    public AudioClip unholsterSound;
    private AudioSource audioSource;

    void Start()
    {
        socketInteractor = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();
        audioSource = GetComponent<AudioSource>();
        socketInteractor.selectEntered.AddListener(OnWeaponHolstered);
        socketInteractor.selectExited.AddListener(OnWeaponUnholstered);
    }

    void OnWeaponHolstered(SelectEnterEventArgs args)
    {
        Debug.Log($"Weapon holstered in {gameObject.name}");
        if (audioSource != null && holsterSound != null)
            audioSource.PlayOneShot(holsterSound);
    }

    void OnWeaponUnholstered(SelectExitEventArgs args)
    {
        Debug.Log($"Weapon unholstered from {gameObject.name}");
        if (audioSource != null && unholsterSound != null)
            audioSource.PlayOneShot(unholsterSound);
    }
} 