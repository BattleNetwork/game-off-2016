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
    public GameObject lobbyEntry;
    public GameObject panelLobbyList;



    private int connectionStatus = -1;
    private ServerInterface serverInterface;
    private List<string> serverList;

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
        ServerInterface.LobbyList += CreateList;
        ServerInterface.LobbyCreated += CreateLobby;
        ServerInterface.LobbyJoined += EnterLobby;
    }

    void EnterLobby(JSONNode result)
    {
        SendMessage("Readyup");
    }

    void CreateList(JSONArray result)
    {
        foreach (JSONNode i in result.Childs)
        {
            serverList.Add(i["name"]);
        }
    }

    IEnumerator PopulateList(List<string> serverList)
    {
        Debug.Log(serverList);
        GameObject lobbyEntryButton = new GameObject();
        lobbyEntryButton.name = "LobbyEntryButton";
        lobbyEntryButton.AddComponent<RectTransform>();
        RectTransform lobbyEntryButtonRect = lobbyEntryButton.GetComponent<RectTransform>();
        lobbyEntryButton.AddComponent<Button>();
        lobbyEntryButton.AddComponent<Text>();
        lobbyEntryButton.GetComponent<Text>().font = Resources.Load<Font>("Fonts/chintzy");
        lobbyEntryButton.GetComponent<Text>().alignment = TextAnchor.MiddleLeft;
        lobbyEntryButton.GetComponent<Text>().fontSize = 14;
        Text lobbyEntryText = lobbyEntryButton.GetComponent<Text>();
        lobbyEntryButtonRect.SetParent(panelLobbyList.transform);
        lobbyEntryButtonRect.anchorMin = new Vector2(0.05f, 0.8f);
        lobbyEntryButtonRect.anchorMax = new Vector2(0.7f, 0.9f);
        lobbyEntryButtonRect.localScale = Vector3.one;
        lobbyEntryButtonRect.sizeDelta = Vector3.zero;
        lobbyEntryButtonRect.anchoredPosition3D = Vector3.zero;

        Vector2 offset = new Vector2(0f, 0f);
        for (int i = serverList.Count - 1; i >= 0; i--)
        {
            GameObject entryCopy = (GameObject)Instantiate(lobbyEntryButton,new Vector3(lobbyEntryButtonRect.position.x, lobbyEntryButtonRect.position.y + offset.y, lobbyEntryButtonRect.position.z), Quaternion.identity, panelLobbyList.transform);
            entryCopy.GetComponent<Text>().text = serverList[i];
            string joinArg = entryCopy.GetComponent<Text>().text;
            AddListener(entryCopy, joinArg);
            serverList.RemoveAt(i);
            offset.y -= 0.6f;
        }
        Destroy(lobbyEntryButton);
        StopCoroutine("PopulateList");
        yield return null;
    }

    void AddListener(GameObject button, string lobbyName)
    {
        button.GetComponent<Button>().onClick.AddListener(() => JoinLobby(lobbyName));
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

    void JoinLobby(string lobbyName)
    {
        serverInterface.JoinLobby(lobbyName);
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
        serverList = new List<string>();
    }

    // Update is called once per frame
    void Update ()
    {
        if(serverList.Count != 0)
        {
            StartCoroutine(PopulateList(serverList));
        }
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