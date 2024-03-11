using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;



public class StartMenu : MonoBehaviour
{
    GameObject pauseMenuUI;

    void Start(){
        Input.backButtonLeavesApp = true;
    }

    // void Update(){
        // bool backPressed = Keyboard.current.escapeKey.wasPressedThisFrame;
        // if (backPressed){
        //     Application.Quit();
        // }
    // }

    void OnBack(){
        //AndroidToast.ShowToast("pressed back");
        Application.Quit();
    }


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