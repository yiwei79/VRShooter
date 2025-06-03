using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.XR;

public class UIController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject panel;
public void GoToMainMenu()
    {
        // Load the main menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void GoToGame()
    {
        // Load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("ForIntergrationTest");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void QuitGame()
    {
        // Quit the application
        Application.Quit();
    }


}
