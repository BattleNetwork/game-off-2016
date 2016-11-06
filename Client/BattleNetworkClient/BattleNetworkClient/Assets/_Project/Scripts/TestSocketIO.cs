using UnityEngine;
using System.Collections;
using System;
using Quobject.SocketIoClientDotNet.Client;

public class TestSocketIO : MonoBehaviour {
    Socket socket;
    // Use this for initialization
    void Start () {
        

    }

    void test()
    {
        socket = IO.Socket("http://localhost");
        socket.On(Socket.EVENT_CONNECT, () =>
        {
            socket.Emit("hi");

        });
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
