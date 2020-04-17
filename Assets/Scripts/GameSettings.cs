using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    private static GameDifficulty difficulty = GameDifficulty.UNSET;

    public static GameDifficulty Difficulty 
    {
        get 
        {
            return difficulty;
        }
        set 
        {
            difficulty = value;
        }
    }
}
