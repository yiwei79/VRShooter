using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class HandGun : MonoBehaviour
{
    [Header("Weapon Properties")]
    public float damage = 25f;
    public float range = 100f;
    public float fireRate = 0.5f;
    public float recoilForce = 5f;
    public float impactForce = 10f;
    
    [Header("References")]
    public Transform muzzle;
    public ParticleSystem muzzleFlash;
    public AudioSource audioSource;
    public AudioClip fireSound;
    public GameObject impactEffect;
    
    [Header("Ammo System")]
    public int maxAmmo = 12;
    public int currentAmmo;
    public AudioClip reloadSound;
    
    private float nextFireTime = 0f;
    private XRGrabInteractable grabInteractable;
    private Rigidbody weaponRigidbody;
    private bool isGrabbed = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize components
        grabInteractable = GetComponent<XRGrabInteractable>();
        weaponRigidbody = GetComponent<Rigidbody>();
        currentAmmo = maxAmmo;
        
        // Set up XR Interaction events
        if (grabInteractable != null)
        {
            grabInteractable.activated.AddListener(OnTriggerPulled);
            grabInteractable.selectEntered.AddListener(OnGrabbed);
            grabInteractable.selectExited.AddListener(OnReleased);
        }
        
        // Ensure muzzle is assigned
        if (muzzle == null)
        {
            // Create a default muzzle point if none assigned
            GameObject muzzlePoint = new GameObject("Muzzle");
            muzzlePoint.transform.SetParent(transform);
            muzzlePoint.transform.localPosition = new Vector3(0, 0, 0.5f); // Adjust as needed
            muzzle = muzzlePoint.transform;
        }
    }
    
    void OnTriggerPulled(ActivateEventArgs args)
    {
        Fire();
    }
    
    void OnGrabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }
    
    void OnReleased(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }
    
    public void Fire()
    {
        // Check fire rate and ammo
        if (Time.time < nextFireTime || currentAmmo <= 0)
            return;
            
        nextFireTime = Time.time + 1f / fireRate;
        currentAmmo--;
        
        // Play effects
        PlayFireEffects();
        
        // Apply recoil to weapon
        ApplyRecoil();
        
        // Perform raycast for hit detection
        PerformRaycast();
        
        // Auto-reload if empty
        if (currentAmmo <= 0)
        {
            Reload();
        }
    }
    
    void PlayFireEffects()
    {
        // Muzzle flash
        if (muzzleFlash != null)
            muzzleFlash.Play();
            
        // Fire sound
        if (audioSource != null && fireSound != null)
            audioSource.PlayOneShot(fireSound);
    }
    
    void ApplyRecoil()
    {
        if (weaponRigidbody != null && isGrabbed)
        {
            // Apply backward force to simulate recoil
            Vector3 recoilDirection = -muzzle.forward;
            weaponRigidbody.AddForce(recoilDirection * recoilForce, ForceMode.Impulse);
            
            // Add slight upward kick
            weaponRigidbody.AddForce(Vector3.up * (recoilForce * 0.3f), ForceMode.Impulse);
            
            // Add rotational recoil
            weaponRigidbody.AddTorque(Vector3.right * (recoilForce * 0.5f), ForceMode.Impulse);
        }
    }
    
    void PerformRaycast()
    {
        RaycastHit hit;
        Vector3 rayDirection = muzzle.forward;
        
        if (Physics.Raycast(muzzle.position, rayDirection, out hit, range))
        {
            // Apply force to hit object
            ApplyImpactForce(hit);
            
            // Log what we hit for debugging (no enemy system needed yet)
            LogHitInfo(hit);
            
            // Spawn impact effect
            SpawnImpactEffect(hit);
            
            // Debug ray in scene view
            Debug.DrawRay(muzzle.position, rayDirection * hit.distance, Color.red, 1f);
        }
        else
        {
            // Debug ray when no hit
            Debug.DrawRay(muzzle.position, rayDirection * range, Color.yellow, 1f);
        }
    }
    
    void ApplyImpactForce(RaycastHit hit)
    {
        Rigidbody hitRigidbody = hit.collider.GetComponent<Rigidbody>();
        if (hitRigidbody != null)
        {
            // Calculate force direction
            Vector3 forceDirection = hit.point - muzzle.position;
            forceDirection.Normalize();
            
            // Apply force at hit point
            hitRigidbody.AddForceAtPosition(forceDirection * impactForce, hit.point, ForceMode.Impulse);
            
            // Debug.Log($"Applied {impactForce} force to {hit.transform.name}"); // Commented out to prevent TLS allocator errors
        }
    }
    
    void LogHitInfo(RaycastHit hit)
    {
        // Simple logging for debugging - no enemy dependencies
        // Debug.Log($"Hit: {hit.transform.name} at {hit.point}"); // Commented out to prevent TLS allocator errors
        
        // TODO: Enemy damage system will be implemented later
        // This keeps the weapon functional without complex dependencies
    }
    
    void SpawnImpactEffect(RaycastHit hit)
    {
        if (impactEffect != null)
        {
            GameObject effect = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(effect, 2f); // Clean up after 2 seconds
        }
    }
    
    public void Reload()
    {
        if (currentAmmo < maxAmmo)
        {
            currentAmmo = maxAmmo;
            
            // Play reload sound
            if (audioSource != null && reloadSound != null)
                audioSource.PlayOneShot(reloadSound);
                
            // Debug.Log("Weapon reloaded!"); // Commented out to prevent TLS allocator errors
        }
    }
    
    // Public method to manually reload (can be called by other scripts)
    public void ManualReload()
    {
        Reload();
    }
    
    // Getter for UI or other systems
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
    
    public int GetMaxAmmo()
    {
        return maxAmmo;
    }
    
    public bool IsEmpty()
    {
        return currentAmmo <= 0;
    }
    
    void OnDestroy()
    {
        // Clean up event listeners
        if (grabInteractable != null)
        {
            grabInteractable.activated.RemoveListener(OnTriggerPulled);
            grabInteractable.selectEntered.RemoveListener(OnGrabbed);
            grabInteractable.selectExited.RemoveListener(OnReleased);
        }
    }
    
    // Debug information in inspector
    void OnDrawGizmosSelected()
    {
        if (muzzle != null)
        {
            // Draw firing direction
            Gizmos.color = Color.red;
            Gizmos.DrawRay(muzzle.position, muzzle.forward * range);
            
            // Draw muzzle position
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(muzzle.position, 0.05f);
        }
    }
}
