using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



public class YouWonManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    //int sceneTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        int score  = PlayerPrefs.GetInt(GlobalConstants.SCORE_KEY, 0);
        scoreText.SetText("Score: " + score);
        //StartCoroutine(NextScreen());
    }

    public void NextScreen()
    {
        //yield return new WaitForSeconds(sceneTime);

        int next = PlayerPrefs.GetInt(GlobalConstants.IS_HIGH_SCORE);
        if (next == 0)
        {
            SceneManager.LoadScene(GlobalConstants.START_SCENE_INDEX);

        }
        else
        {
            SceneManager.LoadScene(GlobalConstants.NEW_HIGHSCORE_SCENE_INDEX);
        }

    }
}
