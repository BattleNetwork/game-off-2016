using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;

public class Connection : MonoBehaviour {

    public Button loginButton;
    public Button createUserButton;
    public int successOrFail; //debug: 0 = successful connection, 1 = fail.
    public Text inputFieldUserName;
    public Text inputFieldPassword;

    private int connectionStatus = -1;
    private ServerInterface serverInterface;
    private JSONNode test;

    void D_OfflineAuthentication()
    {
        if (successOrFail == 0)
        {
            connectionStatus = 0;
        }
        else if (successOrFail == 1)
        {
            connectionStatus = 1;
        }

    }

    void EventSubscribe()
    {
        ServerInterface.Authenticated += Authorised;
        ServerInterface.Unauthorized += Rejected;
        ServerInterface.UserCreated += UserCreated;
        ServerInterface.UserNotCreated += UserNotCreated;
        ServerInterface.LobbyList += PopulateList;
        ServerInterface.LobbyCreated += CreateLobby;

    }

    void PopulateList(JSONArray result)
    {
        foreach(JSONNode i in result.Childs)
        {
            Debug.Log(i["name"]);
        }
    }

    void UserNotCreated(SimpleJSON.JSONNode result)
    {
        throw new System.NotImplementedException();
    }

    void UserCreated(SimpleJSON.JSONNode result)
    {
        SendMessage("UserCreationConfirm");
    }

    void Authorised(SimpleJSON.JSONNode result)
    {
        connectionStatus = 0;
    }

    void Rejected(SimpleJSON.JSONNode result)
    {
        connectionStatus = 1;
    }



    void RefreshLobbyList()
    {
        serverInterface.ListLobby();

    }

    void CreateLobby(SimpleJSON.JSONNode result)
    {
        serverInterface.LeaveLobby();
    }

    // Use this for initialization
    void Start ()
    {
        EventSubscribe();
        serverInterface = GetComponent<ServerInterface>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (connectionStatus == 0)
        {
            GameObject.Find("LoggingInText").GetComponent<Text>().text = "Connected.";
            SendMessage("LobbyTransition");
        }
        else if (connectionStatus == 1)
        {
            GameObject.Find("LoggingInText").GetComponent<Text>().text = "Unauthorised.";
        }
    }
}