using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;

public class ShareManager : MonoBehaviour
{

     [SerializeField] private Button whatsappButton;
    [SerializeField] private Button twitterButton;
    [SerializeField] private Button facebookButton;
      SocialShareComposer composerFacebook;
    SocialShareComposer composerTwitter;
    SocialShareComposer composerWhatsapp;
    // Start is called before the first frame update
    void Start()
    {
         composerWhatsapp = SocialShareComposer.CreateInstance(SocialShareComposerType.WhatsApp);
        composerTwitter = SocialShareComposer.CreateInstance(SocialShareComposerType.Twitter);
        composerFacebook = SocialShareComposer.CreateInstance(SocialShareComposerType.Facebook);

                // Sharing
        whatsappButton.onClick.AddListener(() => Share("whatsapp"));
        twitterButton.onClick.AddListener(() => Share("twitter"));
        facebookButton.onClick.AddListener(() => Share("facebook"));

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
             Debug.Log("sharing from " + callerType);
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
}
