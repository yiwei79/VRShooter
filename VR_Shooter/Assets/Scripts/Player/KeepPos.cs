using UnityEngine;

public class KeepPos : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = new Vector3(0.33f, 0, -0.28f);
        transform.rotation = Quaternion.Euler(0, 175, 0);
    }
}
