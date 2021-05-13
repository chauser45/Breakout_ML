using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    //"Screens"
    public Canvas mainMenu;
    public Canvas optionsMenu;
    public Canvas aboutMenu;
    public Canvas gameLost;
    public Canvas gameWon;
    public Canvas highscores;

    //Option selections
    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject testOn;
    public GameObject testOff;
    public GameObject easyDifficulty;
    public GameObject mediumDifficulty;
    public GameObject hardDifficulty;

    void Awake()
    {   
        mainMenu.enabled = true;    //Show only main menu on game start
        optionsMenu.enabled = false;
        aboutMenu.enabled = false;
        gameLost.enabled = false;
        gameWon.enabled = false;
        highscores.enabled = false;

        //Set options to default states
        soundOn.SetActive(false);
        soundOff.SetActive(false);
        testOn.SetActive(false);
        testOff.SetActive(false);
        easyDifficulty.SetActive(true);
        mediumDifficulty.SetActive(false);
        hardDifficulty.SetActive(false);
     
        //Only set options to default if game is just starting
        //  Afterwards maintain settings until changed
        if (GameState.startOfGame)
        {
            GameState.easy = true;
            GameState.medium = false;
            GameState.hard = false;
            SoundManagerScript.SoundOn = true;
            GameState.testMode = false;

            //Change flag
            GameState.startOfGame = false;
        }       
    }

    private void FixedUpdate() {
        //Player and Bot ball hit bottom at same time
        if (GameState.hasWon && GameState.hasLost)
        {   
        mainMenu.enabled = false;
        optionsMenu.enabled = false;
        aboutMenu.enabled = false;
        gameLost.enabled = true;        //Player loses ties
        gameWon.enabled = false;
        highscores.enabled = false;
        }
        
        //Player win
        else if (GameState.hasWon && !gameWon.enabled)
        {   
        mainMenu.enabled = false;
        optionsMenu.enabled = false;
        aboutMenu.enabled = false;
        gameLost.enabled = false;
        gameWon.enabled = true;
        highscores.enabled = false;
        }

        //Player loss
        else if (GameState.hasLost && !gameLost.enabled)
        {   
        mainMenu.enabled = false;
        optionsMenu.enabled = false;
        aboutMenu.enabled = false;
        gameWon.enabled = false;
        gameLost.enabled = true;
        highscores.enabled = false;
        }
    }

    public void OptionOn()
    {
        //Switch which menu is visible
        mainMenu.enabled = false;
        optionsMenu.enabled = true;     //Show options menu
        aboutMenu.enabled = false;
        gameLost.enabled = false;
        gameWon.enabled = false;
        highscores.enabled = false;

        //Show current option settings
        DisplaySound();
        DisplayTest();
        DisplayDifficulty();
    }

    // Mutes game globally and updates options menu to display correct setting
    public void ToggleSound(){
        SoundManagerScript.SoundOn = !SoundManagerScript.SoundOn;
        DisplaySound();
    }

    public void DisplaySound(){
        if (SoundManagerScript.SoundOn){
            soundOn.SetActive(true);
            soundOff.SetActive(false);
        }else{
            soundOn.SetActive(false);
            soundOff.SetActive(true);
        }
    }

    public void ToggleTest(){
        GameState.testMode = !GameState.testMode;
        DisplayTest();
    }

    public void DisplayTest(){
        if (GameState.testMode){
            testOn.SetActive(true);
            testOff.SetActive(false);
        }else{
            testOn.SetActive(false);
            testOff.SetActive(true);
        }
    }

    public void ToggleDifficulty()
    {
        //Easy->Medium->High->Easy etc.
        if (GameState.easy)
        {
            GameState.easy = false;
            GameState.medium = true;
            GameState.hard = false;
        } else if (GameState.medium)
        {
            GameState.easy = false;
            GameState.medium = false;
            GameState.hard = true;
        } else
        {
            GameState.easy = true;
            GameState.medium = true;
            GameState.hard = false;
        }
        DisplayDifficulty();
    }


    public void DisplayDifficulty()
    {
        if(GameState.easy)
        {
            easyDifficulty.SetActive(true);
            mediumDifficulty.SetActive(false);
            hardDifficulty.SetActive(false);
        } else if (GameState.medium)
        {
            easyDifficulty.SetActive(false);
            mediumDifficulty.SetActive(true);
            hardDifficulty.SetActive(false);
        } else
        {
            easyDifficulty.SetActive(false);
            mediumDifficulty.SetActive(false);
            hardDifficulty.SetActive(true);
        }
    }
    
    public void AboutOn(){
        //Switch which menu is visible
        mainMenu.enabled = false;
        optionsMenu.enabled = false;
        aboutMenu.enabled = true;       //Show About screen
        gameLost.enabled = false;
        gameWon.enabled = false;
        highscores.enabled = false;
    }

    public void Return()
    {
        //Switch which menu is visible
        mainMenu.enabled = true;        //Back to main menu
        optionsMenu.enabled = false;
        aboutMenu.enabled = false;
        gameLost.enabled = false;
        gameWon.enabled = false;
        highscores.enabled = false;
        soundOn.SetActive(false);
        soundOff.SetActive(false);

        //Reset between games
        GameState.hasWon = false;
        GameState.hasLost = false;
    }

    public void HighscoresOn()
    {
        //Switch which menu is visible
        mainMenu.enabled = false;
        optionsMenu.enabled = false;
        aboutMenu.enabled = false;
        gameLost.enabled = false;
        gameWon.enabled = false;
        highscores.enabled = true;      //Show highscore screen
        soundOn.SetActive(false);
        soundOff.SetActive(false);

        //Reset between games
        GameState.hasWon = false;
        GameState.hasLost = false;

    }

    public void PlayGame()      //This is Player Vs AI
    {
        // reset the game state
        GameState.hasWon = false;
        GameState.hasLost = false;
        //Show game
        SceneManager.LoadScene("MainScene");
    }

    public void WatchAi()
    {
        // reset the game state
        GameState.hasWon = false;
        GameState.hasLost = false;
        //Show game
        SceneManager.LoadScene("WatchAI");
    }

    public void ExitProgram()   //Exits game but only when running. No effect in editor
    {
        Application.Quit();
    }
}
