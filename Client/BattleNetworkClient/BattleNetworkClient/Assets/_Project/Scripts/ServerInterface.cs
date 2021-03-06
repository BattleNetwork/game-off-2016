﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System;
using System.IO;

public class ServerInterface : Singleton<ServerInterface>
{
    private string _baseAddress;
     
    private SocketIOClient.Client _socket;

    protected ServerInterface()
    {
        _baseAddress = Config.BaseAddress;
    }

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
    public static event NetworkResult LobbyFull;
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

        WWW www = new WWW("http://"+_baseAddress+"/createplayer", encode, header);
        yield return www;

        Debug.Log(www.error);
        Debug.Log(www.text);
        if(!string.IsNullOrEmpty(www.error))
        {
            JSONClass result = new JSONClass();
            result.Add("error", new JSONData(www.error));
            UserNotCreated(result);
        }
        else
        {
            JSONNode result = JSON.Parse(www.text);
            if (result["status"].Value == "error")
            {
                JSONClass message = new JSONClass();
                message.Add("error", new JSONData("Problem when adding to database" + '\n' + "username may be taken or one of the field is empty"));
                UserNotCreated(message);
            }
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

    public void Unready()
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        _socket.Emit("unreadytoplay", args);
    }

    public void ReadyUp()
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        _socket.Emit("readytoplay", args);
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
        _socket = new SocketIOClient.Client("http://"+ _baseAddress +"/ ");
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
                    Unauthorized(JSON.Parse(data.Encoded));
                _socket.Close();
            });
            _socket.On("authenticated", (data) =>
            {
                Debug.Log("authenticated");
                if (Authenticated != null)
                {
                    Authenticated(JSONNode.Parse(data.Encoded));
                }
                    
                _socket.On("lobbylist", (result) =>
                {
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
                    if (LobbyCreated != null)
                        LobbyCreated(JSONNode.Parse(result.Encoded));
                });
                _socket.On("lobbyfull", (result) =>
                {
                    if (LobbyFull != null)
                        LobbyFull(JSONNode.Parse(result.Encoded));
                });
                _socket.On("lobbyjoined", (result) =>
                {
                    if (LobbyJoined != null)
                    {
                        JSONNode resultjson = JSONNode.Parse(result.Encoded)["args"][0];
                        LobbyJoined(resultjson);
                    }
                });
                _socket.On("lobbyleft", (result) =>
                {
                    if (LobbyLeft != null)
                        LobbyLeft(JSONNode.Parse(result.Encoded));
                });
                _socket.On("readyset", (result) =>
                {
                    if (ReadySet != null)
                    {
                        JSONNode resultjson = JSONNode.Parse(result.Encoded)["args"][0];
                        ReadySet(resultjson);
                    }
                });
                _socket.On("goingame", (result) =>
                {
                    if (GoInGame != null)
                        GoInGame(JSONNode.Parse(result.Encoded)["args"][0]);
                });
                _socket.On("unreadyset", (result) =>
                {
                    if (UnReadySet != null)
                        UnReadySet(JSONNode.Parse(result.Encoded)["args"][0]);
                });
                _socket.On("cantunready", (result) =>
                {
                    if (CantUnready != null)
                        CantUnready(JSONNode.Parse(result.Encoded)["args"][0]);
                });
                _socket.On("countdown", (result) =>
                {
                    if (Countdown != null)
                        Countdown(JSONNode.Parse(result.Encoded)["args"][0]);
                });
                _socket.On("playerjoined", (result) =>
                {
                    if (PlayerJoined != null)
                        PlayerJoined(JSONNode.Parse(result.Encoded)["args"][0]);
                });
                _socket.On("playerleft", (result) =>
                {
                    if (PlayerLeft != null)
                        PlayerLeft(JSONNode.Parse(result.Encoded)["args"][0]);
                });
                _socket.On("opponentreadyup", (result) =>
                {
                    if (OpponentReadyUp != null)
                        OpponentReadyUp(JSONNode.Parse(result.Encoded)["args"][0]);
                });
                _socket.On("opponentunready", (result) =>
                {
                    if (OpponentUnready != null)
                        OpponentUnready(JSONNode.Parse(result.Encoded)["args"][0]);
                });
                _socket.On("update", (result) =>
                {
                    if (Update != null)
                        Update(JSONNode.Parse(result.Encoded)["args"][0]);
                });
                _socket.On("gameover", (result) =>
                {
                    
                    if (Gameover != null)
                        Gameover(JSONNode.Parse(result.Encoded)["args"][0]);
                });
                _socket.On("result", (result) =>
                {
                    
                    if (Result != null)
                        Result(JSONNode.Parse(result.Encoded)["args"][0]);
                });
            });
        });

        _socket.Error += (sender, e) => {
            Debug.Log("socket Error: " + e.Message.ToString());
            if (Error != null)
                Error(e.Message.ToString());
        };
    }

    public void NotifyInGame()
    {
        Dictionary<string, string> args = new Dictionary<string, string>();
        _socket.Emit("ingame", args);
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
        if (_socket != null)
        {
            LeaveLobby();
            _socket.Close();
        }
    }
}