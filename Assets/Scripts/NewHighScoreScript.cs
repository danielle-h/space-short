using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class NewHighScoreScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private GameObject highScoreScreen;
    [SerializeField] private TMP_InputField highScoreName;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button whatsappButton;
    [SerializeField] private Button twitterButton;
    [SerializeField] private Button facebookButton;


    HighScoreManager highScoreManager;
    string playerName;
    int highscore;
    SocialShareComposer composerFacebook;
    SocialShareComposer composerTwitter;
    SocialShareComposer composerWhatsapp;


    // Start is called before the first frame update
    void Start()
    {
        highscore = PlayerPrefs.GetInt(GlobalConstants.HIGH_SCORE_KEY, 0);
        highScoreText.SetText(highscore.ToString());
        highScoreName.onEndEdit.AddListener(GetHighScoreName);
        highScoreManager = HighScoreManager.Instance;
        highScoreManager.LoadHighScores();
        saveButton.onClick.AddListener(delegate { GetHighScoreName(highScoreName.text); });

        // Sharing
        whatsappButton.onClick.AddListener(() => Share("whatsapp"));
        twitterButton.onClick.AddListener(() => Share("twitter"));
        facebookButton.onClick.AddListener(() => Share("facebook"));

        composerWhatsapp = SocialShareComposer.CreateInstance(SocialShareComposerType.WhatsApp);
        composerTwitter = SocialShareComposer.CreateInstance(SocialShareComposerType.Twitter);
        composerFacebook = SocialShareComposer.CreateInstance(SocialShareComposerType.Facebook);

        StartCoroutine(TakeScreenshot(1.0f));

    }

    IEnumerator TakeScreenshot(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Perform the action here
        composerWhatsapp.AddScreenshot();
        composerTwitter.AddScreenshot();
        composerFacebook.AddScreenshot();
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

    public void Share(string callerType)
    {
        /*     //return to start scene
             bool canSendText = MessageComposer.CanSendText();
             bool canSendAttachments = MessageComposer.CanSendAttachments();
             bool canSendSubject = MessageComposer.CanSendSubject();
             highScoreText.SetText("Share " + canSendText + " " + canSendSubject + " " + canSendAttachments);
             //print("Share " + canSendText + " " + canSendSubject + " " + canSendAttachments);
             MessageComposer composer = MessageComposer.CreateInstance();
             composer.SetBody("Body");
             composer.SetCompletionCallback((result, error) => {
                 highScoreText.SetText("Result code: " + result.ResultCode);
             });
             composer.Show();*/
        bool isFacebookAvailable = SocialShareComposer.IsComposerAvailable(SocialShareComposerType.Facebook);
        bool isTwitterAvailable = SocialShareComposer.IsComposerAvailable(SocialShareComposerType.Twitter);
        bool isWhatsappAvailable = SocialShareComposer.IsComposerAvailable(SocialShareComposerType.WhatsApp);
        //highScoreText.SetText("Share " + isFacebookAvailable + " " + isTwitterAvailable + " " + isWhatsappAvailable);
        SocialShareComposer composer;
        if (callerType == "facebook")
        {
            composer = composerFacebook;
        }
        else if (callerType == "whatsapp")
        {
            composer = composerWhatsapp;
        }
        else
        {
            composer = composerTwitter;
        }
        composer.SetCompletionCallback((result, error) => {
            //highScoreText.SetText("error: " + result.ResultCode);
            AndroidToast.ShowToast(result.ResultCode + "");
        });
        if (callerType != "facebook")
        {
            composer.SetText("Can you beat my score?");
            composer.AddURL(URLString.URLWithPath("https://play.google.com/store/apps/details?id=com.HoneystoneGames.SpaceShort"));
        }
        composer.Show();


    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
