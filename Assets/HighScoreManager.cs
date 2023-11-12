using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreManager : MonoBehaviour
{
    public List<HighScore> scores = new List<HighScore>();//TODO this doesn't need to be a singleton, maybe this can be a static class? or some kind of global?
    public const int MAX_SIZE = 5;

    public static HighScoreManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public int Size()
    {
        return scores.Count;
    }

    public void LoadHighScores()
    {
        //load high score from disk
       // Debug.Log("loading high score");
        string scoreTable = PlayerPrefs.GetString("highScoreTable", "");
        //string scoreTable = "";
        //Debug.Log("scoretable: " + scoreTable);
        if (scoreTable.Length == 0)
        {
            /*for (int i = 0; i < MAX_SIZE; i++)
            {
                scores.Add(new HighScore { name = "a", score = Random.Range(50, 5000) });
            }
            SortHighScores();*/
            scores = new List<HighScore>();

        }
        else
        {
            HighScoreList list = JsonUtility.FromJson<HighScoreList>(scoreTable);
            scores = list.scores;
        }
    }

    public void AddHighScore(string name, int score)
    {

       // Debug.Log("adding high score");
        scores.Add(new HighScore { name = name, score = score });
        SortHighScores();
        if (scores.Count > MAX_SIZE)
        {
            scores.RemoveAt(MAX_SIZE);
        }

    }

    private void SortHighScores()
    {
        scores.Sort((HighScore x, HighScore y) => y.score.CompareTo(x.score));
    }

    public List<HighScore> getHighScores()
    {
        //Debug.Log("getting high score");
        return scores;//#todo  should return a copy
    }

    public void SaveHighScores()
    {
        //Debug.Log("saving high score of length " + scores.Count);
        HighScoreList list = new HighScoreList { scores = scores};
        string scoreString = JsonUtility.ToJson(list);
        //Debug.Log("saving scoretable: " + scoreString);
        PlayerPrefs.SetString("highScoreTable", scoreString);
        PlayerPrefs.Save();
    }

    public void ClearHighScores()
    {
        scores.Clear();
        SaveHighScores();
    }

    public int GetLowestHighScore()
    {
        if (scores.Count == MAX_SIZE) { 
        return scores[scores.Count - 1].score;
        }
        else
        {
            return 0;
        }
    }

    private class HighScoreList//need this for saving and serializing
    {
        public List<HighScore> scores;
    }

    [System.Serializable]
    public class HighScore
    {
        public string name;
        public int score;

    }
}
