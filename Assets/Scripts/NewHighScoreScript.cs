using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewHighScoreScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject highScoreScreen;
    [SerializeField] private TMP_InputField highScoreName;
    [SerializeField] private Button saveButton;


    HighScoreManager highScoreManager;
    string playerName;
    int highscore;
 


    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt(GlobalConstants.HIGH_SCORE_KEY, 0);
        highScoreText.SetText(highscore.ToString());
        highScoreName.onEndEdit.AddListener(GetHighScoreName);
        highScoreManager = HighScoreManager.Instance;
        highScoreManager.LoadHighScores();
        saveButton.onClick.AddListener(delegate { GetHighScoreName(highScoreName.text); });

    }


    public void GetHighScoreName(string text)
    {
        print(text);
        playerName = text;
        //sanitize text
        playerName = Regex.Replace(playerName, "[^\\w\\._]", "");
        print(playerName);

        //update high score
        highScoreManager.AddHighScore(playerName, highscore);
        print("added new score");

        highScoreManager.SaveHighScores();
        //BackToStart();
        SceneManager.LoadScene(GlobalConstants.HIGHSCORE_SCENE_INDEX);

        //TODO call scene from gameover in game scene
    }

    public void BackToStart()
    {
        //return to start scene
        //print("trying to load first scene");
        SceneManager.LoadScene(GlobalConstants.START_SCENE_INDEX);
    }

    



    // Update is called once per frame
    void Update()
    {
        
    }
}
