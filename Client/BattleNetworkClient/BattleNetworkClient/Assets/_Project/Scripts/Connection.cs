using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using System.Collections.Generic;

public class Connection : MonoBehaviour {

    public Button loginButton;
    public Button createUserButton;
    public int successOrFail; //debug: 0 = successful connection, 1 = fail.
    public Text inputFieldUserName;
    public Text inputFieldPassword;
    public GameObject entry;
    public Text entryText;
    

    private int connectionStatus = -1;
    private ServerInterface serverInterface;
    private JSONNode test;
    private List<string> entryList; 

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
        Debug.Log(result);
        foreach (JSONNode i in result.Childs)
        {
            entryList.Add(i["name"].Value);
        }
    }

    void UserNotCreated(SimpleJSON.JSONNode result)
    {
        Debug.Log("USER NOT CREATED");
        Debug.Log(result);
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
        Debug.Log(entryList);

    }
}