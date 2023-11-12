using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class StartMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;


    public void GoToHighscores()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void showSettings()
    {
        pauseMenuUI.SetActive(true);
    }

    public void hideSettings()
    {
        pauseMenuUI.SetActive(false);
    }
}
