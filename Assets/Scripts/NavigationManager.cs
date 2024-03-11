using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{

    /*  public static int START_SCENE_INDEX = 0;
    public static int NEW_HIGHSCORE_SCENE_INDEX = 3;
    public static int HIGHSCORE_SCENE_INDEX = 2;
    public static int YOU_WON_INDEX = 4;
    public static int GAME_SCENE_INDEX = 1;
    */
    void OnBack(){
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        if (currentScene == GlobalConstants.START_SCENE_INDEX){
            Application.Quit();
        }
        else if (currentScene >= 2){
            SceneManager.LoadScene(GlobalConstants.START_SCENE_INDEX);
        }
        //no case for game scene, taken care of in game - opens pause screen
    }
}
