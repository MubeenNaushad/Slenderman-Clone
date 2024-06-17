using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public GameObject menu, options, credits, loading;
    public string gameSceneName;
    
    public void openOptions()
    {
        menu.SetActive(false);
        options.SetActive(true);
    }
    public void openCredits()
    {
        menu.SetActive(false);
        credits.SetActive(true);
    }
    public void goBack()
    {
        options.SetActive(false);
        credits.SetActive(false);
        menu.SetActive(true);
    }
    public void quitGame()
    {
        Debug.Log("Quit Game called."); // Confirm method is called

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop playing the game in the editor
        #else
        Application.Quit(); // Quit the game in a built application
        #endif
    }
    public void playGame()
    {
        menu.SetActive(false);
        loading.SetActive(true);
        SceneManager.LoadScene(gameSceneName);
    }
}
