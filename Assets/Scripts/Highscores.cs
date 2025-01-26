using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class Highscores : MonoBehaviour
{
    [Serializable]
    class HighScoreData
    {
        public int[] Highscores = new int[5];
        public string[] Initials = new string[5];
    }

    private string scoresPath = "scores.json";

    public Text score1Text;
    public Text score2Text;
    public Text score3Text;
    public Text score4Text;
    public Text score5Text;

    public Text initials1Text;
    public Text initials2Text;
    public Text initials3Text;
    public Text initials4Text;
    public Text initials5Text;

    public int testScoreUpdate = 0;

    private HighScoreData highscoreData = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {      
        LoadHighscores();
        UpdateHighscores(testScoreUpdate, "ZZZ");
        UpdateUI();        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadHighscores()
    {
        if (File.Exists(scoresPath))
        {
            var readjson = File.ReadAllText(scoresPath);
            highscoreData = JsonUtility.FromJson<HighScoreData>(readjson);
        }
        else
        {
            Debug.Log("File does not exist!");
            highscoreData.Highscores[0] = 1000;
            highscoreData.Highscores[1] = 750;
            highscoreData.Highscores[2] = 500;
            highscoreData.Highscores[3] = 250;
            highscoreData.Highscores[4] = 100;

            highscoreData.Initials[0] = "JSA";
            highscoreData.Initials[1] = "PAW";
            highscoreData.Initials[2] = "GKO";
            highscoreData.Initials[3] = "CKO";
            highscoreData.Initials[4] = "BUB";
        }
    }

    private void UpdateHighscores(int score, string initials)
    {
        int newIndex = -1;

        if (score == highscoreData.Highscores[4])
        { 
            newIndex = 4;
        }
        else if(score > highscoreData.Highscores[4])
        {
            for(int i=4;i>=0;i--)
            {
                if(score < highscoreData.Highscores[i])
                {
                    newIndex = i+1;
                    break;
                }
            }
            if(newIndex == -1)
            {
                newIndex = 0;       // highest score
            }
        }

        Debug.Log("New index:" + newIndex);

        // highscore achieved?
     /*   if (newIndex != -1)
        {
            for (int k = newIndex; k<4;k++)
            {
                highscoreData.Highscores[k+1] = highscoreData.Highscores[k];
                highscoreData.Initials[k+1] = highscoreData.Initials[k];
            }
            highscoreData.Highscores[newIndex] = score;
            highscoreData.Initials[newIndex] = initials;
        }*/
        var json = JsonUtility.ToJson(highscoreData);
        File.WriteAllText(scoresPath, json);
    }

    private void UpdateUI()
    {
        score1Text.text = highscoreData.Highscores[0].ToString();
        score2Text.text = highscoreData.Highscores[1].ToString();
        score3Text.text = highscoreData.Highscores[2].ToString();
        score4Text.text = highscoreData.Highscores[3].ToString();
        score5Text.text = highscoreData.Highscores[4].ToString();
            
        initials1Text.text = highscoreData.Initials[0];
        initials2Text.text = highscoreData.Initials[1];
        initials3Text.text = highscoreData.Initials[2];
        initials4Text.text = highscoreData.Initials[3];
        initials5Text.text = highscoreData.Initials[4];
    }
}
