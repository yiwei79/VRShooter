# VR Shooter Implementation Plan (Unity 6.1 + XR Interaction Toolkit 3.1.2)

## Overview

This document outlines the complete implementation plan for a VR Shooter prototype using Unity 6.1 and XR Interaction Toolkit 3.1.2. The game features hands-only interaction, weapon handling, enemy neck grabbing, and a holster system in a single room scenario.

---

## 1. Project Setup and XR Configuration

### 1.1. Unity Project Setup
- Create new Unity 6.1 project using **XR Template** (recommended)
- Install packages via Package Manager:
  - **XR Interaction Toolkit 3.1.2** (should be pre-installed with template)
  - **XR Plugin Management**
  - **OpenXR Plugin** or platform-specific XR plugin

### 1.2. XR Origin Setup
- Use the **XR Origin (VR)** prefab from XR Interaction Toolkit
- Ensure XR Origin contains:
  - **XR Camera** (Main Camera child)
  - **LeftHand Controller** with XR Controller component
  - **RightHand Controller** with XR Controller component
  - **Locomotion System** component on XR Origin

---

## 2. Player Hands System

### 2.1. Hand Models and Setup
- Import hand models or use XR Interaction Toolkit sample hands
- Attach hand models as children to Left/Right Hand Controllers
- Add **XR Direct Interactor** components to both hand objects
- Configure interaction layers for hands

### 2.2. Hand Animation (Using XR Toolkit Built-ins)
- Use **XR Hand Tracking Subsystem** if available
- For controller-based animation:
  - Create Animator Controller with "Grip" and "Trigger" float parameters
  - Use **XR Controller Recorder** for input mapping
  - Implement simple open/close hand animations

### 2.3. Hand Visibility Management
- **Existing Script**: `DisableGrabbingHandModel.cs` ✓
- Handles hiding hands when grabbing weapons
- Uses XR Interaction Toolkit events (selectEntered/selectExited)

---

## 3. Movement System

### 3.1. Locomotion Setup (Using XR Toolkit Components)
- **Locomotion System** (already on XR Origin)
- Add **Continuous Move Provider**:
  - Assign left controller for movement input
  - Configure move speed and strafe
- Add **Teleportation Provider**:
  - Use **XR Ray Interactor** on right controller
  - Add **Teleportation Area** components to valid surfaces
- Add **Snap Turn Provider** for comfort (optional)

### 3.2. Movement Configuration
```csharp
// No custom script needed - use XR Toolkit components:
// - Continuous Move Provider (smooth locomotion)
// - Teleportation Provider (point-and-teleport)
// - Snap Turn Provider (comfort turning)
```

---

## 4. Weapon System

### 4.1. Weapon Prefab Setup- **Existing Script**: `HandGun.cs` ✓ (IMPLEMENTED with force application)- Add **XR Grab Interactable** component to weapon- Configure **Attach Transform** for proper hand positioning- Add **Rigidbody** and **Collider** components### 4.2. Enhanced Weapon Script ✓ (COMPLETED)The `HandGun.cs` script now includes:- **Force Application**: Recoil force on weapon and impact force on hit objects- **Ammo System**: 12-round magazine with auto-reload- **XR Integration**: Full XR Interaction Toolkit integration- **Damage System**: Works with both EnemyAI and generic Health components- **Effects**: Muzzle flash, sounds, and impact effects- **Debug Visualization**: Gizmos for firing direction and range

### 4.3. Weapon Interaction
- Use **XR Grab Interactable** (built-in) for pickup/drop
- Configure **Movement Type** to "Velocity Tracking" for realistic physics
- Set **Throw on Detach** for weapon throwing mechanics

---

## 5. Holster System

### 5.1. Holster Points Setup (Using XR Toolkit)
- Create empty GameObjects for holster positions (hip, chest, back)
- Add **XR Socket Interactor** components to holster points
- Configure **Socket Active** and **Hover Socket Snapping**
- Set **Select Filters** to accept only weapons

### 5.2. Holster Script (Optional Enhancement)
```csharp
public class WeaponHolster : MonoBehaviour
{
    private XRSocketInteractor socketInteractor;
    
    void Start()
    {
        socketInteractor = GetComponent<XRSocketInteractor>();
        socketInteractor.selectEntered.AddListener(OnWeaponHolstered);
        socketInteractor.selectExited.AddListener(OnWeaponTaken);
    }
    
    void OnWeaponHolstered(SelectEnterEventArgs args)
    {
        // Play holster sound/haptics
    }
    
    void OnWeaponTaken(SelectExitEventArgs args)
    {
        // Play unholster sound/haptics
    }
}
```

---

## 6. Enemy System

### 6.1. Enhanced Enemy AI
- **Existing Script**: `EnemyAI` in `zombiemanager.cs` ✓
- Add health and damage system to existing AI
- Integrate with weapon damage system

### 6.2. Enemy Health System
```csharp
// Add to existing EnemyAI class:
[Header("Health System")]
public float maxHealth = 100f;
private float currentHealth;
private bool isDead = false;

void Start()
{
    agent = GetComponent<NavMeshAgent>();
    currentHealth = maxHealth;
}

public void TakeDamage(float damage)
{
    if (isDead) return;
    
    currentHealth -= damage;
    
    if (currentHealth <= 0)
    {
        Die();
    }
}

void Die()
{
    isDead = true;
    agent.isStopped = true;
    // Add death animation/effects
    Destroy(gameObject, 2f);
}
```

### 6.3. Neck Grabbing System
- Add child GameObject to enemy neck with **XR Grab Interactable**
- Create **EnemyNeckGrabPoint** script:

```csharp
public class EnemyNeckGrabPoint : MonoBehaviour
{
    public EnemyAI enemyParent;
    public float grabDamage = 15f;
    public float stunDuration = 3f;
    
    private XRGrabInteractable grabInteractable;
    
    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(OnNeckGrabbed);
        grabInteractable.selectExited.AddListener(OnNeckReleased);
        
        if (!enemyParent) enemyParent = GetComponentInParent<EnemyAI>();
    }
    
    void OnNeckGrabbed(SelectEnterEventArgs args)
    {
        enemyParent.OnGrabbed(stunDuration);
        enemyParent.TakeDamage(grabDamage);
    }
    
    void OnNeckReleased(SelectExitEventArgs args)
    {
        enemyParent.OnReleased();
    }
}
```

### 6.4. Enemy Stun System
```csharp
// Add to EnemyAI class:
private bool isStunned = false;
private float stunEndTime = 0f;

void Update()
{
    // Check stun status
    if (isStunned && Time.time > stunEndTime)
    {
        isStunned = false;
        agent.isStopped = false;
    }
    
    // Existing AI logic only if not stunned
    if (!isStunned && !isDead && IsPlayerInView())
    {
        agent.SetDestination(player.position);
    }
}

public void OnGrabbed(float stunDuration)
{
    isStunned = true;
    stunEndTime = Time.time + stunDuration;
    agent.isStopped = true;
}

public void OnReleased()
{
    // Could reduce remaining stun time or add other effects
}
```

---

## 7. Interactable Objects

### 7.1. Radio Interactable
- Add **XR Simple Interactable** component to radio
- Create **RadioController** script for activation logic

### 7.2. Door System
- Use **XR Simple Interactable** for door interaction
- Implement door opening animation/logic

---

## 8. Scene Setup Checklist

### 8.1. Essential GameObjects
- [ ] **XR Origin (VR)** with locomotion system
- [ ] **Hand models** attached to controllers
- [ ] **Weapon prefab** with XR Grab Interactable
- [ ] **Enemy prefab** with AI and neck grab point
- [ ] **Holster points** with XR Socket Interactor
- [ ] **Radio** with XR Simple Interactable
- [ ] **NavMesh** baked for enemy movement

### 8.2. XR Interaction Layers
- Configure interaction layers for:
  - Weapons (grabbable)
  - Enemies (neck grabbable)
  - Environment (radio, doors)
  - Holsters (socket interactors)

---

## 9. Testing and Validation

### 9.1. Core Functionality Tests
- [ ] Hand tracking and visibility
- [ ] Weapon pickup, firing, and holstering
- [ ] Enemy AI, damage, and death
- [ ] Neck grabbing and stunning
- [ ] Movement (joystick and teleport)
- [ ] Radio and door interaction

### 9.2. VR Comfort
- [ ] Proper player height calibration
- [ ] Comfortable movement speeds
- [ ] Snap turn for motion sensitivity
- [ ] Haptic feedback for interactions

---

## 10. File Structure

```
Assets/Scripts/
├── Player/
│   ├── DisableGrabbingHandModel.cs ✓
│   └── PlayerHealth.cs (optional)
├── Weapons/
│   ├── HandGun.cs ✓ (needs enhancement)
│   └── WeaponHolster.cs (new)
├── Enemy management/
│   ├── zombiemanager.cs ✓ (enhance existing EnemyAI)
│   ├── EnemyNeckGrabPoint.cs (new)
│   └── BlackBoard.cs ✓
└── Interactables/
    ├── RadioController.cs (new)
    └── DoorController.cs (new)
```

---

## 11. Implementation Priority1. **Phase 1**: Basic XR setup and hand interaction2. **Phase 2**: Weapon system and holstering ✓ (HandGun COMPLETED)3. **Phase 3**: Enhanced enemy AI with health/damage ✓ (EnemyAI ENHANCED)4. **Phase 4**: Neck grabbing mechanics5. **Phase 5**: Radio and door interactions6. **Phase 6**: Polish, VFX, and SFX---## 12. COMPLETED IMPLEMENTATIONS ✓### HandGun.cs - Complete Weapon System- **Force Application**: Weapon recoil and impact forces on hit objects- **XR Integration**: Full XR Interaction Toolkit 3.1.2 support- **Ammo System**: 12-round magazine with auto-reload- **Damage System**: Compatible with EnemyAI and generic Health components- **Effects Support**: Muzzle flash, audio, and impact effects- **Debug Tools**: Gizmos for range and direction visualization### EnemyAI (zombiemanager.cs) - Enhanced Enemy System- **Health System**: Damage, death, and visual feedback- **Stun Mechanics**: Support for neck grabbing system- **Force Application**: Rigidbody integration for physics impacts- **Improved AI**: Better player detection and movement- **Death System**: Audio, effects, and cleanup### Health.cs - Generic Health Component- **Universal Health System**: Can be used on any object- **Event System**: UnityEvents for damage, death, and health changes- **Flexible Configuration**: Customizable death behavior and effects---**Key Advantage**: This plan leverages XR Interaction Toolkit 3.1.2's built-in components (XR Grab Interactable, XR Socket Interactor, XR Simple Interactable) to minimize custom scripting while maintaining full functionality.

using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float viewRadius = 10f;
    [Range(0, 360)]
    public float viewAngle = 120f;
    public Transform player;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    private NavMeshAgent agent;
    private bool playerInSight = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (IsPlayerInView())
        {
            agent.SetDestination(player.position);
        }
    }

    bool IsPlayerInView()
    {
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Angle check
        if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
        {
            // Raycast to check for obstacles
            if (!Physics.Raycast(transform.position, dirToPlayer, distanceToPlayer, obstacleMask))
            {
                return true;
            }
        }

        return false;
    }

    // Optional: Visualize FOV in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        Vector3 viewAngleA = DirFromAngle(-viewAngle / 2, false);
        Vector3 viewAngleB = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * viewRadius);
    }

    Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}

---

## 🎯 CURRENT FOCUS: SIMPLIFIED WEAPON SYSTEM

### ✅ HandGun.cs - Team-Friendly Implementation
**Status: COMPLETED - Ready for Testing**

**Core Features:**
- **Force Application**: Realistic weapon recoil and impact forces ✓
- **XR Integration**: Full XR Interaction Toolkit 3.1.2 support ✓  
- **Ammo System**: 12-round magazine with auto-reload ✓
- **Physics Interaction**: Applies force to any Rigidbody objects ✓
- **Effects Ready**: Supports muzzle flash, audio, and impact effects ✓
- **Debug Tools**: Visual gizmos for range and direction ✓

**Team Benefits:**
- **No Dependencies**: Works independently without enemy systems
- **Immediate Testing**: Can test force application on any physics objects
- **Clean Architecture**: Easy to extend later without breaking existing code
- **Collaborative**: Other team members can work on enemies separately

### 🧪 Testing the Weapon System

1. **Create a Weapon Prefab:**
   - Add HandGun.cs script to your gun model
   - Add XRGrabInteractable, Rigidbody, and Collider components
   - Create child GameObject for muzzle position

2. **Test Force Application:**
   - Place cubes/spheres with Rigidbodies in scene
   - Grab weapon and fire at objects
   - Observe realistic impact forces and weapon recoil

3. **Verify XR Integration:**
   - Test pickup/drop with VR controllers
   - Confirm trigger activation fires weapon
   - Check ammo system and auto-reload

### 📋 Next Development Phases

**Phase 3 (Later):** Enemy system integration
**Phase 4 (Later):** Neck grabbing mechanics  
**Phase 5 (Later):** Radio and door interactions

**Current Priority:** Get weapon mechanics feeling great before adding complexity!
```

</rewritten_file>