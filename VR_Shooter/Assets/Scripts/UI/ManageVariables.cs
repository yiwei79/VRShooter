using TMPro;
using UnityEngine;

public class ManageVariables : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private TextMeshProUGUI Ammo;
    [SerializeField] private HandGun handGun;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ammo.text = "Ammo: " + handGun.currentAmmo.ToString() + "/" + handGun.maxAmmo.ToString();
    }
}
