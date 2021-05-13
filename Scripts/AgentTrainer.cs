using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.Barracuda;
using UnityEditor;
using UnityEngine.SceneManagement;



public class AgentTrainer : Agent
{
    public GameObject botArena;
    Rigidbody2D paddleRB;
    public GameObject BallBot;
    Vector2 originalPaddlePos;
    Vector2 originalBallPos;
    Vector2 origBotArenaPos;
    Rigidbody2D ballRB;
    GameObject cam;
    Transform parentBotArena;
    NNModel model;
    void Start()
    {   
        paddleRB = GetComponent<Rigidbody2D>();
        ballRB = BallBot.GetComponent<Rigidbody2D>();

        parentBotArena = gameObject.transform.parent;  
        InvokeRepeating("checkWon", 30, 10);
        originalPaddlePos = new Vector2(gameObject.transform.localPosition.x, gameObject.transform.localPosition.y);  
        originalBallPos = new Vector2(BallBot.transform.localPosition.x, BallBot.transform.localPosition.y);
        
        cam = GameObject.FindGameObjectWithTag("MainCamera");

        //Easy difficulty
        if (GameState.easy)
        {
            model = Resources.Load<NNModel>("Brains/Easy"); //Grab the brain from Assets/Resources/Brain
            this.SetModel("PaddleBot", model, InferenceDevice.CPU);   //Set model for paddle agent
        }
        //Medium Difficulty
        else if (GameState.medium)
        {
            model = Resources.Load<NNModel>("Brains/Medium");
            this.SetModel("PaddleBot", model, InferenceDevice.CPU);
        }
        //Hard Difficulty
        else
        {
            model = Resources.Load<NNModel>("Brains/Hard");
            this.SetModel("PaddleBot", model, InferenceDevice.CPU);
        }
    }

    void checkWon(){
        // Get the brick bin child of the arena
        int children = parentBotArena.transform.GetChild(3).childCount;
        // if no bricks remain, trigger a win and end the episode
        if (children <1 ){
            print("There are " + children + "children in bin of" + parentBotArena.name);
            AddReward(1.0f);
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        if (SceneManager.GetActiveScene().name == "Training")
        {   
            //grab the original positions of moving objects to reset later
            gameObject.transform.localPosition = originalPaddlePos;
            BallBot.transform.localPosition = originalBallPos;
            ballRB.velocity = new Vector2(1,2).normalized * 20;     //Gives, "NullReferenceException: Object not set to an instance of an object"
                                                                    //  Likely due to velocity not being set initially in editor
            //call to build all the bricks in the arena             //  No noted impacts on performance
            parentBotArena.gameObject.GetComponent<TrainingLayer>().SetupBricks();
        }

        if(SceneManager.GetActiveScene().name == "WatchAI")
        {
            gameObject.transform.localPosition = new Vector2((float)17.25, -26);
            BallBot.transform.localPosition = new Vector2((float)19.75, -26);
            ballRB.velocity = new Vector2(1, 2).normalized * 20;    //Gives same error
            parentBotArena.gameObject.GetComponent<TrainingLayer>().SetupBricks();
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {   
        // initialize the actions as being discrete, aka not occuring over a range(like a force). 
        // The bot can move full speed left or right or sit still.
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = (int)Input.GetAxisRaw("PlayerAD")+1;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Ball position and velocity, and agent position
        sensor.AddObservation(BallBot.transform.localPosition.x);
        sensor.AddObservation(BallBot.transform.localPosition.y);
        sensor.AddObservation(ballRB.velocity.x);
        sensor.AddObservation(ballRB.velocity.y);
        sensor.AddObservation(this.transform.localPosition.x);    
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
            // Actions, size = 3
            float action = actionBuffers.DiscreteActions[0];
            float movement = 0;
            // each index in the actions array corresponds to one of the three movement options, left, right, or stationary.
            switch(action){
                case 0:
                    movement = -1;
                    break;
                case 1: 
                    movement = 0;
                    break;
                case 2:
                    movement = 1;
                    break;

            }
            // Apply the action results to move the Agent
            paddleRB.velocity = new Vector2(movement * 30f,0);    
    }

    private void OnCollisionEnter2D(Collision2D col) {
        // add a very small reward for a collision with the ball
        if (col.collider.gameObject == BallBot){
             AddReward(.02f);
        }
    }

    public void breakBrick(Transform brickBin){
        AgentTrainer thisTrainer = brickBin.parent.GetChild(0).GetComponent<AgentTrainer>();
        // add a small reward for breaking a brick
        AddReward(.1f);
        int children = brickBin.childCount - 1;
        // Check if no bricks are left in the arena, give a large reward and reset if so.
        if (children == 0){
            thisTrainer.AddReward(10.0f);
           thisTrainer.EndEpisode();
        }
    }

    public void droppedBall(GameObject arena){
        AgentTrainer thisTrainer = arena.transform.GetChild(0).GetComponent<AgentTrainer>();
        // moderately large penalty for losing the game
        thisTrainer.SetReward(-5.0f);
        
        //delete all the bricks in the arena and reset
        foreach(Transform child in arena.transform.GetChild(3)){
                 Destroy(child.gameObject);
        }
        thisTrainer.EndEpisode();
    }
}