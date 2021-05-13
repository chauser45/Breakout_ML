using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public static bool hasWon = false, hasLost = false, testMode = false;
    public static bool easy, medium, hard;   //AI difficulty settings
    public static bool startOfGame = true;   //Used to prevent difficulty and sound from resetting between games
}


