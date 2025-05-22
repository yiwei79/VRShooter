using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class DisableGrabbingHandModel : MonoBehaviour
{
    public GameObject leftHandModel;
    public GameObject rightHandModel;
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(HideGrabbingHand);
        grabInteractable.selectExited.AddListener(ShowGrabbingHand);
    }

    public void HideGrabbingHand(SelectEnterEventArgs args)
    {
        if(args.interactableObject.transform.tag == "LeftHand")
        {
            leftHandModel.SetActive(false);
        }
        else if(args.interactableObject.transform.tag == "RightHand")
        {
            rightHandModel.SetActive(false);
        }
    }

    public void ShowGrabbingHand(SelectExitEventArgs args)
    {
        if(args.interactableObject.transform.tag == "LeftHand")
        {
            leftHandModel.SetActive(true);
        }
        else if(args.interactableObject.transform.tag == "RightHand")
        {
            rightHandModel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
