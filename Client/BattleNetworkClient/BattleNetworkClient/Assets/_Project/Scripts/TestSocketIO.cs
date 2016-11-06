using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestSocketIO : MonoBehaviour {
    SocketIOClient.Client socket;
    // Use this for initialization
    void Start () {
        socket = new SocketIOClient.Client("http://127.0.0.1:3000/");
        socket.On("connect", (fn) => {
            Debug.Log("connect - socket");

            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("pseudo", "Test");
            args.Add("password", "Test");
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
            });
        });

        socket.Error += (sender, e) => {
            Debug.Log("socket Error: " + e.Message.ToString());
        };

        socket.Connect();
    }

    void OnGUI()
    {

        if (GUI.Button(new Rect(20, 70, 150, 30), "ls"))
        {
            Debug.Log("Sending");

            Dictionary<string, string> args = new Dictionary<string, string>();
            args.Add("name", "ls");
            socket.Emit("command", args);
        }

        if (GUI.Button(new Rect(20, 120, 150, 30), "Close Connection"))
        {
            Debug.Log("Closing");

            socket.Close();
        }
    }
}
