using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

public class MenuController : MonoBehaviour {

    public MenuView menuView;

	// Use this for initialization
	void Start () {
        ServerInterface.UserCreated += UserCreated;
        ServerInterface.UserNotCreated += UserNotCreated;
        ServerInterface.Authenticated += Authenticated;
        ServerInterface.Unauthorized += Unauthorized;
        ServerInterface.Error += Error;
    }

    private void Unauthorized(JSONNode result)
    {
        //rise popup;
        Debug.Log(result);
    }

    private void Authenticated(JSONNode result)
    {
        GameModel.Instance.PlayerName = menuView.Username.text;
        GameModel.Instance.Menu.Status = "browsing";
        GameModel.Instance.Menu.IsDirty = true;
    }

    private void UserCreated(JSONNode result)
    {
        menuView.AccountCreationWindow.ShowResult("User has been created");
    }

    private void UserNotCreated(JSONNode result)
    {
        string error = result["error"];
        menuView.AccountCreationWindow.ShowResult("Error Creating your account : " + '\n' + error);
    }

    private void Error(string error)
    {
        Debug.Log("ERROR : " + error);
    }
}
