using UnityEngine;

public class FacePlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform playerCamera;
    Vector3 lookAtPosition;

    // Update is called once per frame
    void Update()
    {
        if(playerCamera != null)
        {
            lookAtPosition = playerCamera.position;
            lookAtPosition.y = transform.position.y; 
            transform.LookAt(lookAtPosition);

            transform.forward = -transform.forward; 
        }
    }
}
