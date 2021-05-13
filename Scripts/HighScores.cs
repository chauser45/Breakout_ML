using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//This class just holds a player's name and their score
public class PlayerInfo
{
    public string name;
    public int score;

    public PlayerInfo(string name, int score)   //Basic setter function
    {
        this.name = name;
        this.score = score;
    }
}

public class HighScores : MonoBehaviour
{
    List<PlayerInfo> scores;
    public InputField display;          //Shows score on Highscore canvas
    public InputField playerNameWin;    //Name entry on player win
    public InputField playerNameLoss;   //Name entry of player loss
    
    // Start is called before the first frame update
    void Start()
    {
        scores = new List<PlayerInfo>();
        LoadHighscores();
    }

     public void SubmitButtonWin()
     {
        //Create new name, score
        PlayerInfo stats = new PlayerInfo(playerNameWin.text, GameManager.score);

        if ( !string.IsNullOrEmpty(stats.name)) //Don't accept empty names
        {
            scores.Add(stats);                  //Add to list of scores
            playerNameWin.text = "Score saved"; //Show that score was aved

            SortScores();           
        }
     }

    //Same but for loss screen, needed seperate fields for the different screens
    public void SubmitButtonLoss()
    {
        PlayerInfo stats = new PlayerInfo(playerNameLoss.text, GameManager.score);

        if (!string.IsNullOrEmpty(stats.name))
        {
            scores.Add(stats);
            playerNameLoss.text = "Score saved";

            SortScores();
        }
    }

    void SortScores()
    {
        for(int i = scores.Count -1; i > 0; i--)    //For all saved scores
        {
            if(scores[i].score > scores[i - 1].score)   //If current score is higher swap them out
            {
                PlayerInfo temp = scores[i - 1];

                scores[i - 1] = scores[i];
                scores[i] = temp;
            }
        }
        UpdatePlayerPrefsString();
    }

    void UpdatePlayerPrefsString()
    {
        string prefs = "";  //Blank string to reset it

        for (int i = 0; i < scores.Count; i++){     //For all scores add to string seperated by commas ,
            prefs += scores[i].name + ",";
            prefs += scores[i].score + ",";
        }

        PlayerPrefs.SetString("Highscores", prefs); //Save string as PlayerPrefs, this saves scores across play sessions

        UpdateHighscoreVisual();
    }

    void UpdateHighscoreVisual()
    {
        display.text = "";     //Reset string

        for(int i = 0; i < scores.Count; i++)   //For all saved scores
        {
            string rank = (i+1).ToString();     //1., 2. , 3. etc
            display.text += rank + ". " + scores[i].name + ": " + scores[i].score + "\n";
            //1. Frank: 2300
            //2. Sally: 2000
            //3. Richard: 400 etc.
        }
    }

    void LoadHighscores()
    {
        string stats = PlayerPrefs.GetString("Highscores", ""); //Retreived saved scores

        string[] stats2 = stats.Split(','); //Separate using the commas

        //Create new PlayerInfos from separated string
        for( int i = 0; i < stats2.Length - 2; i+=2)
        {
            PlayerInfo temp = new PlayerInfo(stats2[i], int.Parse(stats2[i + 1]));

            scores.Add(temp);

            UpdateHighscoreVisual();
        }
    }

    public void ClearHighscores()
    {
        PlayerPrefs.DeleteAll();    //Clear PlayerPrefs

        display.text = "";          //Clear score display
    }
}
