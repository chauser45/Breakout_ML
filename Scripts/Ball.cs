using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Ball : MonoBehaviour
{
    public float speed = 20;
    public GameObject PaddleBot;
    // Start is called before the first frame update
    void Start()
    {   
        // assign an initial velocity to the ball
        GetComponent<Rigidbody2D>().velocity = new Vector2(1,2).normalized * speed;
        if (SceneManager.GetActiveScene().name != "Training"){
            PaddleBot = GameObject.FindGameObjectWithTag("PaddleBot");
        }  
    }
    
    float ReflectAngle(Vector2 ballPos, Vector2 paddlePos, float paddleLength){
        // determine where the ball contactPosed the paddle in relation to the paddle center. A value of
        // 1 is the right edge, a value of -1 is the left edge, 0 in middle
        float contactPos = ((ballPos.x -paddlePos.x) / paddleLength*2) -1;
        if (contactPos < -1 ){
            contactPos = -1;
        } else if (contactPos > 1){
            contactPos = 1;
        }
    
        // convert the contactPos spot to an angle to reflect off at. Max angle is 63 deg, or 1.1 radians.
        float angle = (float) (contactPos * 1.1 + 3.14/2);
        return angle;
    }


    void OnCollisionEnter2D(Collision2D col) {
        // Play bounce sound upon player balls collision with anything but a brick.
        if (gameObject.name == "BallPlayer" && col.gameObject.name != "brick"){
            SoundManagerScript.PlaySound("Bounce");
        }

        // collisions between ball and paddle
        if ((gameObject.name == "BallPlayer" && col.gameObject.name == "PaddlePlayer") || (gameObject.name == "BallBot" && col.gameObject.name == "PaddleBot") ){
            Vector2 dir;

            // detect if hitting the ball with the side of the paddle, and spike it downwards at a 45 degree angle if the ball 
            // is lower in the y axis than the paddle.
            float yDelta = transform.position.y - col.transform.position.y;
            
            if (yDelta < .1){
                if (transform.position.x - col.transform.position.x < 0){
                    dir = new Vector2(-1,-1).normalized;
                    GetComponent<Rigidbody2D>().velocity = dir *speed;
                } 
                else {
                    dir = new Vector2(1,-1).normalized;
                    GetComponent<Rigidbody2D>().velocity = dir *speed;
                }
            }
            
            else{
                // calculate angle at which to send the ball flying away at.
                float angle = ReflectAngle(transform.position, col.transform.position, col.collider.bounds.size.x);

                // Use cos and sin along with normalized to create a unit vector for the new reflection vector of the appropriate angle.
                if (angle < 0){
                    dir = new Vector2((float)System.Math.Cos(angle),(float)System.Math.Sin(angle)*-1).normalized;   
                }
                else{
                    dir = new Vector2((float)System.Math.Cos(angle) * -1,(float)System.Math.Sin(angle)).normalized;
                }
            
                // Multiply unit vector by speed and assign to the ball.
                GetComponent<Rigidbody2D>().velocity = dir *speed;
            }
        }

        // This is the end condition for contactPos with the bottom ball. Checks which ball hit the bottom and loads GameWon or GameLost screens.
        else if (col.gameObject.name == "PlayerWallBottom" || col.gameObject.name == "BotWallBottom")
        {   
            // if in training mode, pass the botArena object to the dropped ball method in the agent script
            if (SceneManager.GetActiveScene().name == "Training" || SceneManager.GetActiveScene().name == "WatchAI") {
                PaddleBot.GetComponent<AgentTrainer>().droppedBall(gameObject.transform.parent.gameObject);
            }
            // if not in training mode, trigger game end according to which components collided
            else{
                // Do nothing if in test mode
                if (GameState.testMode) {}
                            
                //Player loses if ball hits their bottom
                else if (col.gameObject.name == "PlayerWallBottom")
                {
                    GameState.hasLost = true;
                    SceneManager.LoadScene("StartMenus");
                }
                //Player wins if ball hits Bot's bottom
                else
                {
                    GameState.hasWon = true;
                    SceneManager.LoadScene("StartMenus");
                }
            }
            //Do nothing if in test mode
        }
    }
}


