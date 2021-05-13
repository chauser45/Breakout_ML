using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.MLAgents;
using Unity.MLAgents.Policies;
using Unity.Barracuda;
using UnityEditor;
using UnityEngine.SceneManagement;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class GameManager : MonoBehaviour
{
    public Text gameScore;
    public static int score;
    public static int playerBricks;
    public GameObject agent;
    public NNModel model;
    public GameObject returnButton;

  //  public GameObject watchArena;


    // Start is called before the first frame update
    void Start()
    {
        score = 0;  //Reset score on start of game
        playerBricks = 136;

        if (GameState.testMode || SceneManager.GetActiveScene().name == "WatchAI")
        {
            returnButton.SetActive(true);       //Show button when needed to return to main menu
        } else
        {
            returnButton.SetActive(false);
        }
    }

    public void SetScore()
    {
        //Set text in the game
        gameScore.text = "Score: " + score.ToString();
    }
   
    // Update score text as game progresses
    public void Update()
    {
        SetScore();
    }

    //Return to main menu
    public void Return()
    {
        SceneManager.LoadScene("StartMenus");
    }
}
