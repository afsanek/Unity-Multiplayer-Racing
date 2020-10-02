using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class SetUpLocalPlayer : NetworkBehaviour
{
    //--Colors db--
    public Color[] Colors = { Color.black, Color.red, Color.grey, Color.blue, Color.yellow };
    public Mesh[] meshes;
    public Sprite[] Handlers;
    
    //--Change Color--
    public MeshFilter mesh;
    [SyncVar(hook = nameof(OnColorChange))]
    public Color playerColorInput;
    
    //--Change name--
    public Text nameLabel;
    [SyncVar(hook = nameof(OnNameChange))]
    public string playerNameInput;
    
    //--Put player on slider--
    [SerializeField]
    private Transform playerHolder;
    [SerializeField]
    private GameObject handler;
    private GameObject _playerHandler;
    private RectTransform _playerHandlerPos;

    //[SyncVar(hook = nameof(OnPlayerPosChange))]
    //public Vector3 playerPos;
    public List<Transform> gatesTransforms;
    private int _currGateIndex;
    
    //--End game controller
    public int currLap;
    private string _endGameTxt;

    //Find All players

    //----for test :
    /*
    private void OnGUI()
    {
        if (!isLocalPlayer) return;
        _tempText = GUI.TextField(new Rect(25, 15, 100, 30), _tempText);
        if (GUI.Button(new Rect(130,20,30,20),"Set"))
        {
            CmdChangeName(_tempText);
        }
       // _tempColor = GUI.TextField(new Rect(160, 15, 100, 30), _tempColor);
       // if (GUI.Button(new Rect(260,20,30,20),"Set"))
        {
       //     CmdChangeColor(_tempColor);
        }
    }
    */
    //----Initialize----------
    public override void OnStartClient()
    {
        base.OnStartClient();
        Invoke(nameof(UpdateStates),0.8f);
        CanvasController.instance.SetTotalLap(LevelTarget.instance.totalLap);
    }
    
    private void UpdateStates()
    {
        //---because of syncing data with late clients :
        OnNameChange(playerNameInput);
        OnColorChange(playerColorInput);
        //OnPlayerPosChange(playerPos);
    }
    private void Start()
    {
        CanvasController.instance.InitializeText();
        if (!isLocalPlayer)
        {
            GetComponent<CarMovement>().enabled = false;
        }
        else
        {
            GetComponent<CarMovement>().enabled = true;
            if (!(Camera.main is null)) Camera.main.GetComponent<FollowCar>().SetCar(gameObject.transform);
            //InvokeRepeating(nameof(UpdatePlayerHandlerPos),0.3f,0.2f);
        }
        foreach (var gate in GameObject.FindGameObjectsWithTag("Gate"))
        {
            gatesTransforms.Add(gate.GetComponent<Transform>());
        }

        //HandlerSetUp();
    }
    private void HandlerSetUp()
    {
        playerHolder = GameObject.Find("PlayerHolder").GetComponent<Transform>();
        _playerHandler = Instantiate(handler, playerHolder.GetComponent<RectTransform>().position, Quaternion.identity);
        _playerHandlerPos = _playerHandler.GetComponent<RectTransform>();
        _playerHandlerPos.anchoredPosition3D = new Vector3(0,0,0);
        _playerHandler.transform.SetParent(playerHolder,false);
    }
   //--Changing name part--
    [Command]
    public void CmdChangeName(string txt)
    {
        playerNameInput = txt;
        nameLabel.text =playerNameInput;
    }
    public void OnNameChange(string txt)
    {
        playerNameInput = txt;
        nameLabel.text =playerNameInput;
    }
    
    //--Changing color part--
    [Command]
    public void CmdChangeColor(Color color)
    {
        playerColorInput = color;
        mesh.mesh = SearchCarColor(playerColorInput);
        //_playerHandler.GetComponent<Image>().sprite = SearchHandlerColor(playerColorInput);
    }
    public void OnColorChange(Color color)
    {
        playerColorInput = color;
        mesh.mesh = SearchCarColor(playerColorInput);
        //_playerHandler.GetComponent<Image>().sprite = SearchHandlerColor(playerColorInput);
    }

    public Mesh SearchCarColor(Color color)
    {
        var index = System.Array.IndexOf(Colors, color);
        if (index < 0 || index > 4) return meshes[0];
        return meshes[index];
    }
    
    //--Put player on slider--
    private Sprite SearchHandlerColor(string color)
    {
        var index = System.Array.IndexOf(Colors, color);
        print(index);
        return Handlers[index];
    }
    /*
    //--Change player pos on slider--
    [Command]
    public void CmdHandlerPos(Vector3 newPos)
    {
        playerPos = newPos;
        _playerHandlerPos.anchoredPosition3D += playerPos;
    }
    private void OnPlayerPosChange(Vector3 newPos)
    {
        playerPos = newPos;
        _playerHandlerPos.anchoredPosition3D += playerPos;
    }
    private void UpdatePlayerHandlerPos()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        var pl = Distance(transform.position);
        var slidePos = pl / (LevelTarget.target.totalLap+ 1);
        if (slidePos <= 716f) //716 for test --need to change later
        {
            CmdHandlerPos(new Vector3(slidePos,0,0));
        }
    }
    public float Distance(Vector3 playerPos)
    {
        return Mathf.Sqrt(Mathf.Pow(playerPos.x - gatesTransforms[_currGateIndex].position.x, 2)
                          + Mathf.Pow( playerPos.z - gatesTransforms[_currGateIndex].position.z, 2)
                          + Mathf.Pow( playerPos.y - gatesTransforms[_currGateIndex].position.y, 2));
    }
    */
    //--trigger
    private void OnTriggerEnter(Collider other)
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (other.gameObject.CompareTag("Gate"))
        {
          //  _currGateIndex = (_currGateIndex + 1) % gatesTransforms.Count;
        }
        if (other.gameObject.tag.Equals("Finish"))
        {
            currLap++;
            CanvasController.instance.SetCurrentLap(currLap);
            if (currLap > LevelTarget.instance.totalLap)
            {
                CmdSetWinner();
            }
        }
    }
    
    //--Escape
    public void Escape()
    {
        if (isServer)
        {
            NetworkManager.singleton.StopHost();
        }
        else
        {
            NetworkManager.singleton.client.Disconnect();
            //Application.LoadLevel("title");
        }
    }
    
    //--End game
    [Command]
    public void CmdSetWinner()
    {
       RpcSetWinner(this.netId,playerNameInput);
    }

    [ClientRpc]
    public void RpcSetWinner(NetworkInstanceId netID,string playerName)
    {
        DisablePlayer();
        if (isLocalPlayer)
        {
            if (this.netId == netID)
            {
                _endGameTxt = "You Won!";
                CanvasController.instance.SetEndGameText(_endGameTxt);
                Invoke(nameof(BackToLobby),3f);
            }
            else
            {
                _endGameTxt = playerName + "Won!";
                print(_endGameTxt);
                CanvasController.instance.SetEndGameText(_endGameTxt);
            }
        }
    }

    public void DisablePlayer()
    {
        if (isLocalPlayer)
        {
            GetComponent<CarMovement>().enabled = false;
        }
    }

    public void BackToLobby()
    {
        FindObjectOfType<NetworkLobbyManager>().SendReturnToLobby();
    }
}
