# Player Implementation Plan (VR Shooter)

## Overview

This document outlines the step-by-step process to set up the player character for the VR Shooter prototype, using Unity 6.1, XR Interaction Toolkit 3.1.2, and the default VR template. The goal is to create a simple, functional VR player with visible hands, basic movement, and the ability to interact with weapons and the environment.

---

## 1. XR Rig and Player Setup

### 1.1. XR Origin (VR) Setup
- Use the XR Origin (VR) prefab from the XR Interaction Toolkit.
- Place the XR Origin in the center of your play area (the room).
- Ensure the XR Origin has:
  - XR Camera (for player view)
  - Left and Right Hand Controllers (children of XR Origin)

### 1.2. Hand Models
- Use the default hand models provided by the XR Toolkit or import simple hand prefabs.
- Attach the hand models to the Left and Right Hand Controller objects.
- Add XR Direct Interactor components to both hand objects to enable grabbing and interaction.

---

## 2. Player Movement

### 2.1. Locomotion System
- Add a Locomotion System component to the XR Origin.
- Add and configure:
  - Continuous Move Provider (for joystick movement)
  - Teleportation Provider (for teleport movement)
- Assign the correct input actions for movement and teleportation (check XR Interaction Toolkit input settings).

### 2.2. Testing Movement
- Enter Play mode and verify:
  - Joystick movement works (smooth locomotion)
  - Teleportation works (point and click to teleport)
  - Player height and speed feel comfortable

---

## 3. Hand Interactions

### 3.1. Grabbing and Interacting
- Ensure XR Direct Interactor is active on both hands.
- Test grabbing interactable objects (e.g., weapon, radio).
- Adjust hand colliders if necessary for reliable grabbing.

### 3.2. Hand Animation (Optional)
- If using animated hand models, set up animator controllers to respond to grip/trigger input for basic open/close animations.

---

## 4. Player and Weapon Communication

### 4.1. Weapon Pickup
- Ensure weapons have XR Grab Interactable components.
- Test picking up and dropping weapons with both hands.

### 4.2. Firing Mechanism
- When holding a weapon, pressing the trigger should call the weapon's Fire() method.
- Use Unity Events or direct script references to communicate between the hand/controller and the weapon.

---

## 5. Player Health and Damage (Optional for Prototype)

### 5.1. Health System
- Add a simple PlayerHealth script to the XR Origin.
- Implement basic health, damage, and death logic (e.g., respawn or game over).

---

## 6. VR Comfort and Usability

### 6.1. Player Height Adjustment
- Allow for player height calibration if needed (optional for prototype).

### 6.2. Comfort Settings
- Consider adding snap turn or vignette effects for comfort (optional for prototype).

---

## 7. Testing Checklist

- [ ] Player can see and move hands in VR.
- [ ] Player can move using joystick and teleport.
- [ ] Player can pick up and drop weapons.
- [ ] Player can interact with environment objects (e.g., radio, door).
- [ ] Weapon can be fired while held.
- [ ] (Optional) Player health system works.

---

## 8. References

- Unity XR Interaction Toolkit Documentation: https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.1/manual/index.html
- Unity VR Template Documentation

---

**This plan is designed for beginners: focus on getting each step working before moving to the next. Use placeholder models and simple scripts to keep things manageable.** 