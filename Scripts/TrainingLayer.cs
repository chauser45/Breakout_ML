using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingLayer : MonoBehaviour
{

    public GameObject[] bricks; //Holds the four brick prefabs, yellow, green, blue, red

    public GameObject Arena;           
    public int rows;    

    // Start is called before the first frame update
    void Start()
    {
            SetupBricks();  //Make bricks on start of scene
    }

    public void SetupBricks()
    {   
        string topName = Arena.transform.GetChild(2).name;  //Ceiling name
        string wallName = Arena.transform.GetChild(5).name; //Name of left wall
        float screenWidth, screenHeight, brickScaler;
        int columns;

        //Get sizes of sprites
        float brickWidth = bricks[0].GetComponent<SpriteRenderer>().bounds.size.x;  
        float brickHeight = bricks[0].GetComponent<SpriteRenderer>().bounds.size.y;
              
        SetBrickValues(brickWidth, out screenWidth, out screenHeight, out columns, out brickScaler, topName, wallName);

        //Make a brick in each transform.localPosition
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                if (y < 2)  //Top two rows red
                {
                    MakeBrick(bricks[3], screenWidth, screenHeight, brickHeight, brickWidth, brickScaler, x, y, topName);
                }
                else if (y < 4) //Next two rows blue
                {
                    MakeBrick(bricks[2], screenWidth, screenHeight, brickHeight, brickWidth, brickScaler, x, y, topName);
                }
                else if (y < 6) //Next two rows green
                {
                    MakeBrick(bricks[1], screenWidth, screenHeight, brickHeight, brickWidth, brickScaler, x, y, topName);
                }
                else    //Bottom two rows yellow
                {
                    MakeBrick(bricks[0], screenWidth, screenHeight, brickHeight, brickWidth, brickScaler, x, y, topName);
                }
            }
        }
    }

    void MakeBrick(GameObject brickType, float screenWidth, float screenHeight, float brickHeight, float brickWidth, float brickScaler, int x, int y, string topName)
    {
        float xOffset = (float)(Arena.transform.GetChild(2).transform.localPosition.x); //Starting transform.localPosition of bricks = left most side of their arena's top
        GameObject newBrick = (GameObject)Instantiate(brickType, Arena.transform.GetChild(3));   //Create the new brick

                                                    //Place at next brick over            ,  Place at correct height
        newBrick.transform.localPosition = new Vector3((brickWidth * brickScaler * x) + xOffset, screenHeight / 2 - (brickHeight * y), 0);
        //Size the new brick to fit the arena space
        newBrick.transform.localScale = new Vector3(newBrick.transform.localScale.x * brickScaler, newBrick.transform.localScale.y, 1);
    }

    void SetBrickValues(float brickWidth, out float screenWidth, out float screenHeight, out int columns, out float brickScaler, string topName, string wallName)
    {
        //Camera mainCamera = Camera.main;      
        //Get the reference walls for the correct arena
        GameObject wall = Arena.transform.GetChild(4).gameObject;
        GameObject topWall = Arena.transform.GetChild(2).gameObject;

        //Get width of the arena. Box collider width always = 50, need to scale it to topWall               
        screenWidth = topWall.GetComponent<BoxCollider2D>().size.x * topWall.transform.localScale.x;

        screenHeight = wall.transform.localPosition.y + 20; 
        columns = (int)(screenWidth / brickWidth);  //How many bricks fit in the arena
        brickScaler = screenWidth / (brickWidth * columns); //Scale the bricks to fit the entire arena
    }
}
