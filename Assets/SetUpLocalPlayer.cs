using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public enum Colors {
Red,Carbon,Blue,Yellow,Grey
}
[Serializable]
public struct CarColor
{
    public Mesh mesh;
    public Colors color;
}
public class SetUpLocalPlayer : NetworkBehaviour
{
    public CarColor[] carColors;
    public Text nameLabel;
    public MeshFilter mesh;
    [SyncVar(hook = nameof(OnNameChange))]
    public string playerNameInput;
    [SyncVar(hook = nameof(OnColorChange))]
    public string playerColorInput;
    
    private string _tempText;
    private string _tempColor;
    //----for test :
    private void OnGUI()
    {
        if (!isLocalPlayer) return;
        _tempText = GUI.TextField(new Rect(25, 15, 100, 30), _tempText);
        if (GUI.Button(new Rect(130,20,30,20),"Set"))
        {
            CmdChangeName(_tempText);
        }
        _tempColor = GUI.TextField(new Rect(160, 15, 100, 30), _tempColor);
        if (GUI.Button(new Rect(260,20,30,20),"Set"))
        {
            CmdChangeColor(_tempColor);
        }
    }
    //--------------
    public override void OnStartClient()
    {
        base.OnStartClient();
        Invoke(nameof(UpdateStates),0.8f);
    }

    void UpdateStates()
    {
        //---because of syncing data with late clients :
        OnNameChange(playerNameInput);
        OnColorChange(playerColorInput);
    }
    private void Start()
    {
        if (!isLocalPlayer)
        {
            GetComponent<CarMovement>().enabled = false;
        }
        else
        {
            GetComponent<CarMovement>().enabled = true;
            if (!(Camera.main is null)) Camera.main.GetComponent<FollowCar>().SetCar(gameObject.transform);
        }
        nameLabel.text = "Player";
        playerColorInput = "Red";
    }
    private void Reset()
    {
        nameLabel = GetComponentInChildren<Text>();
    }
    [Command]
    private void CmdChangeName(string txt)
    {
        playerNameInput = txt;
        nameLabel.text =playerNameInput;
    }
    private void OnNameChange(string txt)
    {
        playerNameInput = txt;
        nameLabel.text =playerNameInput;
    }
    [Command]
    private void CmdChangeColor(string color)
    {
        playerColorInput = color;
        mesh.mesh = SearchColor(playerColorInput);
    }
    private void OnColorChange(string color)
    {
        playerColorInput = color;
        mesh.mesh = SearchColor(playerColorInput);
    }
    private Mesh SearchColor(string color)
    {
        foreach (var carColor in carColors)
        {
            if (carColor.color.ToString() == color)
            {
                return carColor.mesh;
            }
        }

        return null;
    }
}
