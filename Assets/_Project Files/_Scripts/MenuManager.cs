using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ChangeScene()
    {
        SceneManager.LoadScene(1);  
    }

    // Method to quit the game
    public void QuitGame()
    {
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // In a build, this will close the application
        Application.Quit();
#endif
    }
}
