using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameType
{
    LapMode,
    TimerMode,
    TrainingMode
}
public class LevelTarget : MonoBehaviour
{
    public static LevelTarget instance;

    [Header("Declaring game type")]
    public GameType gameType;
    public int totalLap;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
}
