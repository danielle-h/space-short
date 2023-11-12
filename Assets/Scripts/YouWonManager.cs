using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YouWonManager : MonoBehaviour
{
    int sceneTime = 5;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(NextScreen());
    }



    private IEnumerator NextScreen()
    {
        yield return new WaitForSeconds(sceneTime);

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
