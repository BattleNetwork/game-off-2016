using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;

public class ServerInterface : Singleton<ServerInterface>
{
    public string BaseAddress;
     
    private SocketIOClient.Client _socket;

    protected ServerInterface() { }

    #region Events
    public delegate void NetworkResult(JSONNode result);
    public delegate void NetworkListResult(JSONArray result);
    public delegate void NetworkError(string error);

    public static event NetworkResult UserCreated;
    public static event NetworkResult UserNotCreated;
    public static event NetworkResult Authenticated;
    public static event NetworkResult Unauthorized;
    public static event NetworkListResult LobbyList;
    public static event NetworkResult LobbyCreated;
    public static event NetworkResult LobbyJoined;
    public static event NetworkResult LobbyLeft;
    public static event NetworkResult ReadySet;
    public static event NetworkResult UnReadySet;
    public static event NetworkResult CantUnready;
    public static event NetworkResult GoInGame;
    public static event NetworkResult Countdown;
    public static event NetworkResult PlayerJoined;
    public static event NetworkResult PlayerLeft;
    public static event NetworkResult OpponentReadyUp;
    public static event NetworkResult OpponentUnready;
    public static event NetworkResult Update;
    public static event NetworkResult Gameover;
    public static event NetworkResult Result;
    public static event NetworkError Error;
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
            UserNotCreated(JSON.Parse("{\"error\": \""+ www.error + "\"}"));
        }
        else
        {
            JSONNode result = JSON.Parse(www.text);
            if (result["status"].Value == "error") UserNotCreated(JSON.Parse("{\"error\": \"Problem when adding to database\"}"));
            UserCreated(result["content"]);
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
        ClearEvents();
    }

    private void ClearEvents()
    {
        UserCreated = null;
        UserNotCreated = null;
        Authenticated = null;
        Unauthorized = null;
        LobbyList = null;
        LobbyCreated = null;
        LobbyJoined = null;
        LobbyLeft = null;
        ReadySet = null;
        UnReadySet = null;
        CantUnready = null;
        GoInGame = null;
        Countdown = null;
        PlayerJoined = null;
        PlayerLeft = null;
        OpponentReadyUp = null;
        OpponentUnready = null;
        Update = null;
        Gameover = null;
        Result = null;
        Error = null;
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
                if (Unauthorized != null)
                    Unauthorized(null);
                _socket.Close();
            });
            _socket.On("authenticated", (data) =>
            {
                Debug.Log("authenticated");
                if (Authenticated != null)
                    Authenticated(null);
                _socket.On("lobbylist", (result) =>
                {
                    Debug.Log("lobby list // " + result.Json.ToJsonString() + "//");
                    if (LobbyList != null)
                    {
                        
                        JSONNode resultjson = JSONNode.Parse(result.Encoded);
                        JSONArray arguments = resultjson["args"].AsArray;
                        JSONArray actualresult = arguments[0].AsArray;
                        LobbyList(actualresult);
                    }
                });
                _socket.On("lobbycreated", (result) =>
                {
                    Debug.Log("lobby created // " + result.Json.ToJsonString() + "//");
                    if (LobbyCreated != null)
                        LobbyCreated(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("lobbyjoined", (result) =>
                {
                    Debug.Log("lobby joined // " + result.Json.ToJsonString() + "//");
                    if (LobbyJoined != null)
                        LobbyJoined(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("lobbyleft", (result) =>
                {
                    Debug.Log("lobby left // " + result.Json.ToJsonString() + "//");
                    if (LobbyLeft != null)
                        LobbyLeft(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("readyset", (result) =>
                {
                    Debug.Log("readyset // " + result.Json.ToJsonString() + "//");
                    if (ReadySet != null)
                        ReadySet(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("goingame", (result) =>
                {
                    Debug.Log("goingame // " + result.Json.ToJsonString() + "//");
                    if (GoInGame != null)
                        GoInGame(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("unreadyset", (result) =>
                {
                    Debug.Log("unreadyset // " + result.Json.ToJsonString() + "//");
                    if (UnReadySet != null)
                        UnReadySet(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("cantunready", (result) =>
                {
                    Debug.Log("cantunready // " + result.Json.ToJsonString() + "//");
                    if (CantUnready != null)
                        CantUnready(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("countdown", (result) =>
                {
                    Debug.Log("countdown // " + result.Json.ToJsonString() + "//");
                    if (Countdown != null)
                        Countdown(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("playerjoined", (result) =>
                {
                    Debug.Log("playerjoined // " + result.Json.ToJsonString() + "//");
                    if (PlayerJoined != null)
                        PlayerJoined(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("playerleft", (result) =>
                {
                    Debug.Log("playerleft // " + result.Json.ToJsonString() + "//");
                    if (PlayerLeft != null)
                        PlayerLeft(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("opponentreadyup", (result) =>
                {
                    Debug.Log("opponentreadyup // " + result.Json.ToJsonString() + "//");
                    if (OpponentReadyUp != null)
                        OpponentReadyUp(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("opponentunready", (result) =>
                {
                    Debug.Log("opponentunready // " + result.Json.ToJsonString() + "//");
                    if (OpponentUnready != null)
                        OpponentUnready(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("update", (result) =>
                {
                    Debug.Log("update // " + result.Json.ToJsonString() + "//");
                    if (Update != null)
                        Update(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("gameover", (result) =>
                {
                    Debug.Log("gameover // " + result.Json.ToJsonString() + "//");
                    if (Gameover != null)
                        Gameover(JSONNode.Parse(result.Json.ToJsonString()));
                });
                _socket.On("result", (result) =>
                {
                    Debug.Log("result // " + result.Json.ToJsonString() + "//");
                    if (Result != null)
                        Result(JSONNode.Parse(result.Json.ToJsonString()));
                });
            });

            _socket.Error += (sender, e) => {
                Debug.Log("socket Error: " + e.Message.ToString());
                if (Error != null)
                    Error(e.Message.ToString());
            };
        });
    }

    public void SendCommand(string command)
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        args.Add("command", command);
        _socket.Emit("command", args);
    }

    new void OnDestroy()
    {
        base.OnDestroy();
        if (_socket != null) _socket.Close();
    }
}