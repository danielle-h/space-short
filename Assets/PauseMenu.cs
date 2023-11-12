using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool isGamePaused = false;
    public GameObject pauseMenuUI;

    [SerializeField] AudioMixer myMixer;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume(0);
        }

        if (PlayerPrefs.HasKey("sfxVolume"))
        {
            LoadSFXVolume();
        }
        else
        {
            SetSFXVolume(0);
        }
    }



    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.P))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }*/ //FIXME
        
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;

    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;

    }

    public void SetMusicVolume(float volume)
    {
        
        myMixer.SetFloat("music", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {

        myMixer.SetFloat("sfx", volume);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }

    private void LoadVolume()
    {
        float vol = PlayerPrefs.GetFloat("musicVolume");
        musicSlider.value = vol;
        SetMusicVolume(vol);
    }

    private void LoadSFXVolume()
    {
        float vol = PlayerPrefs.GetFloat("sfxVolume");
        sfxSlider.value = vol;
        SetSFXVolume(vol);
    }

    public void GoToStart()
    {
        Resume();
        //TODO need to save before quitting, or at least ask user
        SceneManager.LoadScene(GlobalConstants.START_SCENE_INDEX);
    }

    public void Settings()
    {
        Debug.Log("Go to settings menu");
    }

    public void BuyCoffee()
    {
        Application.OpenURL("https://www.buymeacoffee.com/369WkrTTU6");
    }

    public void ShowCredits()
    {
        Application.OpenURL("https://danielle-honig.com/space-short-credits");
    }
}
