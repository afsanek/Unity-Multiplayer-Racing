using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RaceController : NetworkBehaviour
{
    public string playerName;
    public Color playerColor;
    public int currLap;

    private static List<RaceController> players = new List<RaceController>();
    private void Start()
    {
        CanvasController.instance.InitializeText();
    }

    [ServerCallback]
    private void OnEnable()
    {
        if (!players.Contains(this))
        {
            players.Add(this);
        }
    }
    [ServerCallback]
    private void OnDisable()
    {
        if (players.Contains(this))
        {
            players.Remove(this);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }

        if (other.gameObject.tag.Equals("Finish"))
        {
            currLap++;
            CanvasController.instance.SetCurrentLap(currLap);
            if (currLap > LevelTarget.target.totalLap)
            {
                
            }
        }
    }

    private void Win()
    {
        if (isLocalPlayer )
        {
            print("you win");
        }
    }

    public void Escape()
    {
        if (isServer)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.client.Disconnect();
            Application.LoadLevel("title");
        }
    }
}
