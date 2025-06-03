using UnityEngine;

public class HPBar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private UnityEngine.UI.Image hpBarImage;
    [SerializeField] private PlayerHealth playerHealth;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpBarImage.fillAmount = playerHealth.currentHealth / playerHealth.maxHealth;
    }
}
