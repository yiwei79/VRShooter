using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerUIPanel : MonoBehaviour
{
    [SerializeField] private GameObject panelToShow;
    [SerializeField] private InputActionProperty leftTrigger;


    private void OnEnable()
    {
        leftTrigger.action.performed += OnTriggerPressed;
        leftTrigger.action.canceled += OnTriggerReleased;
        leftTrigger.action.Enable();
    }

    private void OnDisable()
    {
        leftTrigger.action.performed -= OnTriggerPressed;
        leftTrigger.action.canceled -= OnTriggerReleased;
        leftTrigger.action.Disable();
    }

    private void OnTriggerPressed(InputAction.CallbackContext context)
    {
        panelToShow.SetActive(true);
        Debug.Log("Trigger pressed, panel activated.");
    }

    private void OnTriggerReleased(InputAction.CallbackContext context)
    {
        panelToShow.SetActive(false);
        Debug.Log("Trigger released, panel deactivated.");
    }

}
