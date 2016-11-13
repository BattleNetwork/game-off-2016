using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Connection : MonoBehaviour {

    public int connectionStatus = -1;
    public Button loginButton;
    public int successOrFail; //debug: 0 = successful connection, 1 = fail.
    public Text inputFieldUserName;
    public Text inputFieldPassword;

    private float actTime;
    private float loginWaitTime = 5.0f;
    private ServerInterface serverInterface;

    void OnAuthenticated()
    {
        ServerInterface.Authenticated += Authorised;
    }

    void OnRejected()
    {
        ServerInterface.Unauthorized += Rejected;
    }

    void Authorised(SimpleJSON.JSONNode result)
    {
        GameObject.Find("LoggingInText").GetComponent<Text>().text = "Connected.";
        connectionStatus = 0;
    }

    void Rejected(SimpleJSON.JSONNode result)
    {
        GameObject.Find("LoggingInText").GetComponent<Text>().text = "Unauthorised.";
        connectionStatus = 1;
    }

    void LobbyConnection()
    {
        serverInterface.StartAuthentifiedConnection(inputFieldUserName.text, inputFieldPassword.text);
    }
    // Use this for initialization
    void Start ()
    {
        loginButton.onClick.AddListener(LobbyConnection);
        serverInterface = GetComponent<ServerInterface>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (connectionStatus == 0)
        {
            SendMessage("LobbyTransition");
        }
        else if (connectionStatus == 1)
        {

        }
    }
}