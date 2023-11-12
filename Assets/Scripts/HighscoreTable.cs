using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    [SerializeField]
    TextMeshProUGUI noHighScoreText;

    private void Start()
    {
        HighScoreManager highScoreManager = HighScoreManager.Instance;
        Debug.Log("got highscoremanager");
        highScoreManager.LoadHighScores();
        List<HighScoreManager.HighScore> scores = highScoreManager.getHighScores();
        
        entryContainer = transform.Find("HighscoreEntryContainer");
        Debug.Log(scores.Count);
        entryTemplate = transform.Find("HighscoreEntryContainer/HighscoreEntryTemplate");
        //Debug.Log(entryTemplate);
        entryTemplate.gameObject.SetActive(false);
        //todo remove htis - for debug only
        highScoreManager.SaveHighScores();

        float templateHeight = 45f;

        if (scores.Count == 0)
        {
            noHighScoreText.gameObject.SetActive(true);
        }

        for (int i = 0; i < scores.Count; i++)
        {
            Transform entryTransform = Instantiate(entryTemplate, entryContainer);
            RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
            entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * i - templateHeight*1.5f);
            entryTransform.gameObject.SetActive(true);

            entryTransform.Find("ScoreText").GetComponent<TMP_Text>().text = scores[i].score.ToString();
            entryTransform.Find("PosText").GetComponent<TMP_Text>().text = (i+1).ToString();
            entryTransform.Find("NameText").GetComponent<TMP_Text>().text = scores[i].name;


        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
