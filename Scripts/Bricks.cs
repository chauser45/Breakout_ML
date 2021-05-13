using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bricks : MonoBehaviour
{
    GameObject PaddleBot;
    private void Start() {
        PaddleBot = GameObject.FindGameObjectWithTag("PaddleBot");
    }
    void OnCollisionEnter2D(Collision2D col)     //Ball colliding with Bricks is the trigger
    {   
        Destroy(gameObject);    //gameObject = the individual brick
        
        if (SceneManager.GetActiveScene().name == "Training"){          
            PaddleBot.GetComponent<AgentTrainer>().breakBrick(gameObject.transform.parent);
        }
        
        //100 points for every brick that is broken
        if (col.gameObject.name == "BallPlayer"){
            GameManager.score = GameManager.score + 100;
            // decrement remaining playerBricks tracker.
            GameManager.playerBricks -= 1;
            // print("bricks left:" + GameManager.playerBricks);
        }
        
        // check if no playerBricks remain and trigger a game win if the case.
        if (GameManager.playerBricks == 0){
            GameState.hasWon=true;
            SceneManager.LoadScene("StartMenus");
        }
    }
}
