using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ServerInterface : MonoBehaviour
{
    public string BaseAddress;

    private SocketIOClient.Client _socket;

    #region Events
    public delegate void NetworkEvent(string result);

    public static event NetworkEvent UserCreated;
    public static event NetworkEvent UserNotCreated;
    public static event NetworkEvent LobbyList;
    public static event NetworkEvent LobbyCreated;
    public static event NetworkEvent LobbyJoined;
    public static event NetworkEvent LobbyLeft;
    public static event NetworkEvent ReadySet;
    public static event NetworkEvent UnReadySet;
    public static event NetworkEvent CantUnready;
    public static event NetworkEvent GoInGame;
    public static event NetworkEvent Countdown;
    public static event NetworkEvent PlayerJoined;
    public static event NetworkEvent PlayerLeft;
    public static event NetworkEvent OpponentReadyUp;
    public static event NetworkEvent OpponentUnready;
    public static event NetworkEvent Update;
    public static event NetworkEvent Gameover;
    public static event NetworkEvent Result;
    #endregion



    public void CreatePlayer(string name, string password)
    {
        StartCoroutine(UserCreationRequest(name, password));
    }

    IEnumerator UserCreationRequest(string name, string password)
    {
        string json = "{\"pseudo\":\"" + name + "\", \"pass\":\"" + password + "\"}";
        byte[] encode = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "application/json");

        WWW www = new WWW("http://"+BaseAddress+"/createplayer", encode, header);
        yield return www;

        Debug.Log(www.error);
        Debug.Log(www.text);
        if(!string.IsNullOrEmpty(www.error))
        {
            UserNotCreated(www.error);
        }
        else
        {
            UserCreated(www.text);
        }
    }

    public void StartAuthentifiedConnection(string name, string password)
    {
        Debug.Log("Opening");
        CreateSocket(name, password);
        _socket.Connect();
    }

    public void ListLobby()
    {
        Debug.Log("listlobby");

        Dictionary<string, string> args = new Dictionary<string, string>();
        _socket.Emit("listlobby", args);
    }

    public void CreateLobby(string lobbyName)
    {
        Debug.Log("createlobby");

        Dictionary<string, string> args = new Dictionary<string, string>();
        args.Add("lobbyName", lobbyName);
        _socket.Emit("createlobby", args);
    }

    public void JoinLobby(string lobbyName)
    {
        Debug.Log("joinlobby");

        Dictionary<string, string> args = new Dictionary<string, string>();
        if (string.IsNullOrEmpty(lobbyName)) args.Add("lobbyName", "Test");
        else args.Add("lobbyName", lobbyName);
        _socket.Emit("joinlobby", args);
    }

    public void CloseConnection()
    {
        Debug.Log("Closing");

        _socket.Close();
    }

    public void LeaveLobby()
    {
        Debug.Log("leaving");
        Dictionary<string, string> args = new Dictionary<string, string>();
        _socket.Emit("leavelobby", args);
    }

    private void CreateSocket(string name, string password)
    {
        _socket = new SocketIOClient.Client("http://"+ BaseAddress +"/ ");
        _socket.On("connect", (fn) =>
        {
            Debug.Log("connect - socket");

            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("pseudo", name);
            args.Add("password", password);
            _socket.Emit("authentication", args);
            _socket.On("unauthorized", (data) =>
            {
                Debug.Log("unauthorized");
                _socket.Close();
            });
            _socket.On("authenticated", (data) =>
            {
                Debug.Log("authenticated");
                _socket.On("lobbylist", (result) =>
                {
                    Debug.Log("lobby list // " + result.Json.ToJsonString() + "//");
                    if (LobbyList != null)
                        LobbyList(result.Json.ToJsonString());
                });
                _socket.On("lobbycreated", (result) =>
                {
                    Debug.Log("lobby created // " + result.Json.ToJsonString() + "//");
                    if (LobbyCreated != null)
                        LobbyCreated(result.Json.ToJsonString());
                });
                _socket.On("lobbyjoined", (result) =>
                {
                    Debug.Log("lobby joined // " + result.Json.ToJsonString() + "//");
                    if (LobbyJoined != null)
                        LobbyJoined(result.Json.ToJsonString());
                });
                _socket.On("lobbyleft", (result) =>
                {
                    Debug.Log("lobby left // " + result.Json.ToJsonString() + "//");
                    if (LobbyLeft != null)
                        LobbyLeft(result.Json.ToJsonString());
                });
                _socket.On("readyset", (result) =>
                {
                    Debug.Log("readyset // " + result.Json.ToJsonString() + "//");
                    if (ReadySet != null)
                        ReadySet(result.Json.ToJsonString());
                });
                _socket.On("goingame", (result) =>
                {
                    Debug.Log("goingame // " + result.Json.ToJsonString() + "//");
                    if (GoInGame != null)
                        GoInGame(result.Json.ToJsonString());
                });
                _socket.On("unreadyset", (result) =>
                {
                    Debug.Log("unreadyset // " + result.Json.ToJsonString() + "//");
                    if (UnReadySet != null)
                        UnReadySet(result.Json.ToJsonString());
                });
                _socket.On("cantunready", (result) =>
                {
                    Debug.Log("cantunready // " + result.Json.ToJsonString() + "//");
                    if (CantUnready != null)
                        CantUnready(result.Json.ToJsonString());
                });
                _socket.On("countdown", (result) =>
                {
                    Debug.Log("countdown // " + result.Json.ToJsonString() + "//");
                    if (Countdown != null)
                        Countdown(result.Json.ToJsonString());
                });
                _socket.On("playerjoined", (result) =>
                {
                    Debug.Log("playerjoined // " + result.Json.ToJsonString() + "//");
                    if (PlayerJoined != null)
                        PlayerJoined(result.Json.ToJsonString());
                });
                _socket.On("playerleft", (result) =>
                {
                    Debug.Log("playerleft // " + result.Json.ToJsonString() + "//");
                    if (PlayerLeft != null)
                        PlayerLeft(result.Json.ToJsonString());
                });
                _socket.On("opponentreadyup", (result) =>
                {
                    Debug.Log("opponentreadyup // " + result.Json.ToJsonString() + "//");
                    if (OpponentReadyUp != null)
                        OpponentReadyUp(result.Json.ToJsonString());
                });
                _socket.On("opponentunready", (result) =>
                {
                    Debug.Log("opponentunready // " + result.Json.ToJsonString() + "//");
                    if (OpponentUnready != null)
                        OpponentUnready(result.Json.ToJsonString());
                });
                _socket.On("update", (result) =>
                {
                    Debug.Log("update // " + result.Json.ToJsonString() + "//");
                    if (Update != null)
                        Update(result.Json.ToJsonString());
                });
                _socket.On("gameover", (result) =>
                {
                    Debug.Log("gameover // " + result.Json.ToJsonString() + "//");
                    if (Gameover != null)
                        Gameover(result.Json.ToJsonString());
                });
                _socket.On("result", (result) =>
                {
                    Debug.Log("result // " + result.Json.ToJsonString() + "//");
                    if (Result != null)
                        Result(result.Json.ToJsonString());
                });
            });
        });
    }
}

