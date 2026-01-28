using System.Collections;
using Tobo.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PM_QuitButton : MonoBehaviour
{
    // Anybody can modify the name of the main menu scene just in case if its name has updated
    [Scene, SerializeField] private string mainMenuSceneName;
    
    public void QuitToMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
