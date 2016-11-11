using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestSocketIO : MonoBehaviour {
    SocketIOClient.Client socket;
    string lobbyname = "";
    string pseudo = "";
    string pass = "";

    private void CreateSocket()
    {
        socket = new SocketIOClient.Client("http://127.0.0.1:3000/");
        socket.On("connect", (fn) => {
            Debug.Log("connect - socket");

            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("pseudo", "toto");
            args.Add("password", "tata");
            socket.Emit("authentication", args);
            socket.On("unauthorized", (data) => {
                Debug.Log("unauthorized");
                socket.Close();
            });
            socket.On("authenticated", (data) => {
                Debug.Log("authenticated");
                socket.On("result", (result) =>
                {
                    Debug.Log("command result = " + result);
                });
                socket.On("lobbylist", (result) =>
                {
                    Debug.Log("lobby list = " + result.Json.ToJsonString());
                });
                socket.On("lobbycreated", (result) =>
                {
                    Debug.Log("lobby created // " + result.Json.ToJsonString() + "//");
                });
                socket.On("lobbyjoined", (result) =>
                {
                    Debug.Log("lobby joined // " + result.Json.ToJsonString()+ "//");
                });
                socket.On("lobbyleft", (result) =>
                {
                    Debug.Log("lobby left");
                });
                socket.On("readyset", (result) =>
                {
                    Debug.Log("readyset");
                });
                socket.On("goingame", (result) =>
                {
                    Debug.Log("goingame");
                });
                socket.On("unreadyset", (result) =>
                {
                    Debug.Log("unreadyset");
                });
                socket.On("cantunready", (result) =>
                {
                    Debug.Log("cantunready");
                });
                socket.On("countdown", (result) =>
                {
                    Debug.Log("countdown");
                });
                socket.On("playerjoined", (result) =>
                {
                    Debug.Log("playerjoined");
                });
                socket.On("playerleft", (result) =>
                {
                    Debug.Log("playerleft");
                });
                socket.On("opponentreadyup", (result) =>
                {
                    Debug.Log("opponentreadyup");
                });
                socket.On("opponentunready", (result) =>
                {
                    Debug.Log("opponentunready");
                });
                socket.On("update", (result) =>
                {
                    Debug.Log("update");
                });
                socket.On("gameover", (result) =>
                {
                    Debug.Log("gameover");
                });
                socket.On("result", (result) =>
                {
                    Debug.Log("result");
                });
            });
        });

        socket.Error += (sender, e) => {
            Debug.Log("socket Error: " + e.Message.ToString());
        };
    }

    void OnGUI()
    {
        

        lobbyname = GUI.TextField(new Rect(180, 30, 250, 30), lobbyname, 40);
        pseudo = GUI.TextField(new Rect(700, 30, 200, 30), pseudo, 40); ;
        pass = GUI.TextField(new Rect(700, 70, 200, 30), pass, 40); ;

        if (GUI.Button(new Rect(20, 70, 150, 30), "listlobby"))
        {
            Debug.Log("listlobby");

            Dictionary<string, string> args = new Dictionary<string, string>();
            socket.Emit("listlobby", args);
        }

        if (GUI.Button(new Rect(180, 70, 150, 30), "createlobby"))
        {
            Debug.Log("createlobby");

            Dictionary<string, string> args = new Dictionary<string, string>();
            if(string.IsNullOrEmpty(lobbyname))  args.Add("lobbyName", "Test");
            else args.Add("lobbyName", lobbyname);
            socket.Emit("createlobby", args);
        }

        if (GUI.Button(new Rect(340, 70, 150, 30), "joinlobby"))
        {
            Debug.Log("joinlobby");

            Dictionary<string, string> args = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(lobbyname)) args.Add("lobbyName", "Test");
            else args.Add("lobbyName", lobbyname);
            socket.Emit("joinlobby", args);
        }

        if (GUI.Button(new Rect(20, 120, 150, 30), "Open Connection"))
        {
            Debug.Log("Opening");
            CreateSocket();
            socket.Connect();
        }

        if (GUI.Button(new Rect(180, 120, 150, 30), "Close Connection"))
        {
            Debug.Log("Closing");

            socket.Close();
        }

        if (GUI.Button(new Rect(340, 120, 150, 30), "leave lobby"))
        {
            Debug.Log("leaving");
            Dictionary<string, string> args = new Dictionary<string, string>();
            socket.Emit("leavelobby", args);
        }

        if (GUI.Button(new Rect(700, 120, 150, 30), "Create user"))
        {
            StartCoroutine(UserCreationRequest());
        }
    }

    IEnumerator UserCreationRequest()
    {
        string json = "{\"pseudo\":\"" + pseudo + "\", \"pass\":\"" + pass + "\"}";
        byte[] encode = System.Text.Encoding.ASCII.GetBytes(json.ToCharArray());
        Dictionary<string, string> header = new Dictionary<string, string>();
        header.Add("Content-Type", "application/json");

        WWW www = new WWW("http://localhost:3000/createplayer", encode, header);
        yield return www;

        Debug.Log(www.error);
        Debug.Log(www.text);
        if(!string.IsNullOrEmpty(www.error))
        {

        }
    }

    void OnDestroy()
    {
       if(socket != null) socket.Close();
    }
}
