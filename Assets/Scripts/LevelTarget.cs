using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public enum GameType
{
    LapMode,
    TimerMode,
    TrainingMode
}
public class LevelTarget : NetworkBehaviour
{
    public static LevelTarget target;
    [Header("Declaring game type")]
    public GameType gameType;
    public int totalLap;
    public int totalTime;

    private void Awake()
    {
        if (target == null)
        {
            target = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
