using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoreMenu : MonoBehaviour
{

    int highScore;
    HighScoreManager highScoreManager;
    

    // Start is called before the first frame update
    void Start()
    {
        //highScore = PlayerPrefs.GetInt(GlobalConstants.HIGH_SCORE_KEY, 0);//TODO change to use highscoremanager
        //UpdateGUI();
        highScoreManager = HighScoreManager.Instance;

    }

      public void GoBackToGame()
    {
        SceneManager.LoadScene(GlobalConstants.START_SCENE_INDEX);
    }

    public void ResetHighScore()
    {
        //PlayerPrefs.SetInt(GlobalConstants.HIGH_SCORE_KEY, 0);//TODO change to use highscoremanager
        // highScore = 0;
        highScoreManager.ClearHighScores();
        //UpdateGUI();
    }

   
}
