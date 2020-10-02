using System.Collections;
using System.Collections.Generic;
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobbyPl = lobbyPlayer.GetComponent<LobbyPlayer>();
        SetUpLocalPlayer plCar = gamePlayer.GetComponent<SetUpLocalPlayer>();

        plCar.playerNameInput = lobbyPl.playerName;
        plCar.playerColorInput = lobbyPl.playerColor;
        
    }
}
