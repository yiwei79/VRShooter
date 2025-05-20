Revised VR Player Setup with Hands Only
Based on your updated requirements, let's focus on implementing just the hands for the player controller instead of full arms. This is a more common approach in VR games and will be simpler to implement.
1. Setting Up Hand Models

Import Hand Models:

Find or create realistic hand models (or stylized, depending on your game's aesthetic)
Ensure they're properly rigged with finger bones if you want finger animations
Models should be in a neutral pose (slightly curved fingers work best)


Position in the Scene:

Add the hand models as children of your left and right controller objects
Position them so they extend naturally from the controllers
Rotate them to match the expected hand position when holding controllers



csharppublic class HandSetup : MonoBehaviour
{
    public Transform leftControllerTransform;
    public Transform rightControllerTransform;
    public GameObject leftHandPrefab;
    public GameObject rightHandPrefab;
    
    void Start()
    {
        // Instantiate hands
        if (leftHandPrefab && leftControllerTransform)
        {
            GameObject leftHand = Instantiate(leftHandPrefab, leftControllerTransform);
            // Position and rotate as needed
            leftHand.transform.localPosition = new Vector3(0, 0, 0);
            leftHand.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        
        if (rightHandPrefab && rightControllerTransform)
        {
            GameObject rightHand = Instantiate(rightHandPrefab, rightControllerTransform);
            // Position and rotate as needed
            rightHand.transform.localPosition = new Vector3(0, 0, 0);
            rightHand.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
2. Hand Animation System
For basic hand animations without full IK:

Create Simple Animations:

Open hand (default)
Closed fist (for grabbing)
Pointing (optional)
Trigger finger curled (for shooting)


Implement Animation Controller:

Create an Animator Controller with these states
Set up transitions between them based on button inputs


Connect Controller Inputs:

csharpusing UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class HandAnimator : MonoBehaviour
{
    public Animator handAnimator;
    public XRController controller;
    
    // Animation parameter names
    private const string GRIP_PARAM = "Grip";
    private const string TRIGGER_PARAM = "Trigger";
    
    void Update()
    {
        if (controller == null || handAnimator == null)
            return;
            
        // Get controller input values
        float gripValue = 0f;
        float triggerValue = 0f;
        
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.grip, out gripValue))
        {
            handAnimator.SetFloat(GRIP_PARAM, gripValue);
        }
        
        if (controller.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out triggerValue))
        {
            handAnimator.SetFloat(TRIGGER_PARAM, triggerValue);
        }
    }
}
3. Using Unity's XR Hands Package (Optional)
For more detailed finger tracking and animations:

Install XR Hands Package:

Open Package Manager and add "XR Hands"


Configure Hand Tracking:

Add XRHandSubsystem component to your XR Origin
Connect controller inputs to finger joints



csharpusing UnityEngine;
using UnityEngine.XR.Hands;

public class XRHandTracker : MonoBehaviour
{
    public XRHandSubsystem handSubsystem;
    public GameObject leftHandModel;
    public GameObject rightHandModel;
    
    // References to finger joints in your models
    public Transform[] leftFingerJoints;
    public Transform[] rightFingerJoints;
    
    void Start()
    {
        if (handSubsystem == null)
        {
            // Find the active hand subsystem
            var xrHandSubsystems = new List<XRHandSubsystem>();
            SubsystemManager.GetSubsystems(xrHandSubsystems);
            if (xrHandSubsystems.Count > 0)
                handSubsystem = xrHandSubsystems[0];
        }
    }
    
    void Update()
    {
        if (handSubsystem == null)
            return;
            
        // Update hand poses based on controller inputs
        UpdateHandPose(handSubsystem.leftHand, leftFingerJoints);
        UpdateHandPose(handSubsystem.rightHand, rightFingerJoints);
    }
    
    void UpdateHandPose(XRHand hand, Transform[] fingerJoints)
    {
        // Implementation depends on your hand model's joint setup
    }
}
4. Interaction System for Hands
For grabbing objects and weapons:

Add XR Interactors to Hands:

Use Unity's XR Interaction Toolkit
Add XR Direct Interactor components to your hand objects


Create Grab Points on Objects:

Add XR Grab Interactable to weapons and other items
Set up attach transforms to position items correctly in hand


Configure the Interactions:

Set up proper layering for interactive objects
Configure interaction filters as needed



5. Weapon Handling System
For weapons to work with your hands:

Create Weapon Prefabs:

Model of the gun
Attach points for hand positioning
Colliders and rigidbody


Implement Weapon Behavior:

csharppublic class Weapon : MonoBehaviour
{
    [Header("Weapon Properties")]
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 0.5f;
    
    [Header("References")]
    public Transform muzzle;
    public ParticleSystem muzzleFlash;
    public AudioSource audioSource;
    public AudioClip fireSound;
    
    // For hand positioning
    public Transform primaryHandGrip;
    public Transform secondaryHandGrip; // Optional
    
    private float nextFireTime = 0f;
    private XRGrabInteractable grabInteractable;
    
    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable != null)
        {
            grabInteractable.activated.AddListener(OnTriggerPulled);
        }
    }
    
    void OnTriggerPulled(ActivateEventArgs args)
    {
        Fire();
    }
    
    public void Fire()
    {
        if (Time.time < nextFireTime)
            return;
            
        nextFireTime = Time.time + 1f / fireRate;
        
        // Play effects
        if (muzzleFlash != null)
            muzzleFlash.Play();
            
        if (audioSource != null && fireSound != null)
            audioSource.PlayOneShot(fireSound);
        
        // Raycast for hit detection
        RaycastHit hit;
        if (Physics.Raycast(muzzle.position, muzzle.forward, out hit, range))
        {
            // Check if hit an enemy
            Enemy enemy = hit.transform.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            
            // Spawn impact effect at hit point
            // (You can add this functionality later)
        }
    }
    
    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.activated.RemoveListener(OnTriggerPulled);
        }
    }
}
6. Holster System
For a hand-based holster system:

Create Holster Points:

Empty GameObjects positioned around the player (hip, chest, back)


Implement Snap Zone Interactables:

Add XR Socket Interactor components to holster points
Configure to accept only weapon objects



csharppublic class Holster : MonoBehaviour
{
    public XRSocketInteractor socketInteractor;
    
    void Start()
    {
        if (socketInteractor == null)
            socketInteractor = GetComponent<XRSocketInteractor>();
    }
    
    // You can add custom logic for holster feedback
    public void OnWeaponHolstered(XRBaseInteractable interactable)
    {
        Debug.Log("Weapon holstered: " + interactable.name);
        // Play holster sound or animation
    }
    
    public void OnWeaponTaken(XRBaseInteractable interactable)
    {
        Debug.Log("Weapon taken: " + interactable.name);
        // Play unholster sound or animation
    }
}
7. Grabbing Enemies by the Neck
For the neck-grabbing mechanic:

Add Grab Points to Enemies:

Create a specific GameObject with collider on enemy necks
Add XR Simple Interactable or XR Grab Interactable component


Implement the Grab Behavior:

csharppublic class EnemyNeckGrabPoint : MonoBehaviour
{
    public Enemy enemyParent;
    public float damageOnGrab = 5f;
    public float stunDuration = 2f;
    
    private XRBaseInteractable interactable;
    private bool isGrabbed = false;
    
    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnGrabbed);
            interactable.selectExited.AddListener(OnReleased);
        }
        
        if (enemyParent == null)
            enemyParent = GetComponentInParent<Enemy>();
    }
    
    void OnGrabbed(SelectEnterEventArgs args)
    {
        if (enemyParent != null && !isGrabbed)
        {
            isGrabbed = true;
            enemyParent.OnGrabbed(stunDuration);
            enemyParent.TakeDamage(damageOnGrab);
        }
    }
    
    void OnReleased(SelectExitEventArgs args)
    {
        if (enemyParent != null && isGrabbed)
        {
            isGrabbed = false;
            enemyParent.OnReleased();
        }
    }
    
    void OnDestroy()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnGrabbed);
            interactable.selectExited.RemoveListener(OnReleased);
        }
    }
}

Update the Enemy Class:

csharppublic class Enemy : MonoBehaviour
{
    // Add these methods to your existing Enemy class
    private NavMeshAgent agent;
    private bool isStunned = false;
    private float stunEndTime = 0f;
    
    // Existing Start method
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Other initialization
    }
    
    // Existing Update method
    void Update()
    {
        if (isStunned && Time.time > stunEndTime)
        {
            isStunned = false;
            if (agent != null)
                agent.isStopped = false;
        }
        
        // Rest of your update logic
    }
    
    public void OnGrabbed(float stunDuration)
    {
        isStunned = true;
        stunEndTime = Time.time + stunDuration;
        
        if (agent != null)
            agent.isStopped = true;
            
        // Play stun animation or sound
    }
    
    public void OnReleased()
    {
        // Any logic when released before stun time ends
        // Could keep them stunned or have them recover faster
    }
}
Implementation Steps Summary

Start with the Unity VR Template:

Begin with the basic XR Origin setup


Add Hand Models:

Import hand models and attach to controllers
Set up basic hand animations based on controller input


Configure Weapon Interactions:

Create weapon prefabs with XR Grab Interactable
Implement firing mechanics


Set Up the Holster System:

Add socket interactors for weapon storage


Implement Enemy Neck Grabbing:

Create grab points on enemies
Set up interactable components
Add stun and damage effects


Add VFX and SFX:

Muzzle flash, impact effects
Gun sounds, hit sounds
(These can be added later for the full experience)



Getting Started Quickly
For a quick prototype:

Use the built-in hand models from the XR Interaction Toolkit sample packages
Create a simple gun prefab with basic shooting mechanics
Implement a basic enemy with health and death logic
Add simple holster points using XR Socket Interactors

These steps will give you the core functionality for your VR shooter with hands-only interaction.
Would you like me to focus on any specific element from this revised approach?