using UnityEngine;
using System.Collections;
using SimpleJSON;
using System;

public class MenuController : MonoBehaviour {

    public MenuView menuView;

    private LobbyButton selectedLobby;

	// Use this for initialization
	void Start () {
        RegisterEvents();
    }

    private void Unauthorized(JSONNode result)
    {
        Debug.Log(result["args"][0]["message"].Value);
        if(result["args"][0]["message"].Value == "User not found")
            GameModel.Instance.Menu.ErrorMessage = "Wrong Username or Password";
        else
            GameModel.Instance.Menu.ErrorMessage = "Unknown error when trying to log in";
        GameModel.Instance.Menu.IsDirty = true;
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

    private void LobbyCreated(JSONNode result)
    {
        menuView.LobbyCreationWindow.ShowResult("Lobby " + result["args"][0]["lobbyName"] + " has been created", GoToLobby);
    }

    private void GoToLobby()
    {
        GameModel.Instance.Menu.Status = "inlobby";
        GameModel.Instance.Menu.IsDirty = true;
    }

    private void LobbyJoined(JSONNode result)
    {
        string opponentName = result;
        if(result.AsInt > 0)
        GameModel.Instance.Menu.SetOpponentStatus(result.AsBool);
        GameModel.Instance.Menu.PlayerJoined(opponentName);
        GoToLobby();
    }

    private void LobbyList(JSONArray result)
    {
        foreach(JSONClass lobby in result.Childs)
        {
            GameModel.Instance.Menu.LobbyList.Push(new Lobby(lobby["name"].Value, lobby["nbPlayer"].AsInt));
        }
        GameModel.Instance.Menu.IsDirty = true;
    }

    private void OpponentUnready(JSONNode result)
    {
        GameModel.Instance.Menu.SetOpponentStatus(false);
    }

    private void OpponentReadyUp(JSONNode result)
    {
        GameModel.Instance.Menu.SetOpponentStatus(true);
    }

    private void PlayerLeft(JSONNode result)
    {
        GameModel.Instance.Menu.PlayerLeft();
    }

    private void PlayerJoined(JSONNode result)
    {
        string opponentName = result;
        GameModel.Instance.Menu.PlayerJoined(opponentName);
    }

    private void UnreadySet(JSONNode result)
    {
        GameModel.Instance.Menu.PlayerStatus = false;
        GameModel.Instance.Menu.IsDirty = true;
    }

    private void ReadySet(JSONNode result)
    {
        GameModel.Instance.Menu.PlayerStatus = true;
        GameModel.Instance.Menu.IsDirty = true;
    }

    private void Error(string error)
    {
        Debug.Log("ERROR : " + error);
        GameModel.Instance.Menu.ErrorMessage = error;
        GameModel.Instance.Menu.IsDirty = true;
    }

    public void NewAccount()
    {
        menuView.AccountCreationWindow.Show();
    }

    public void NewLobby()
    {
        menuView.LobbyCreationWindow.Show();
    }

    public void Login()
    {
        ServerInterface.Instance.StartAuthentifiedConnection(menuView.Username.text, menuView.Password.text);
    }

    public void ChangeStatus()
    {
        if (!GameModel.Instance.Menu.PlayerStatus)
        {
            ReadyUp();
        }
        else
        {
            Unready();
        }
    }

    private void ReadyUp()
    {
        ServerInterface.Instance.ReadyUp();
    }

    private void Unready()
    {
        ServerInterface.Instance.Unready();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LeaveLobby()
    {
        ServerInterface.Instance.LeaveLobby();
        GameModel.Instance.Menu.Status = "browsing";
        GameModel.Instance.Menu.IsDirty = true;
    }

    public void logout()
    {

        ServerInterface.Instance.CloseConnection();
        GameModel.Instance.Menu.Status = "login";
        GameModel.Instance.Menu.IsDirty = true;
        RegisterEvents();
    }

    public void RefreshLobbyList()
    {
        ServerInterface.Instance.ListLobby();
    }

    public void JoinLobby()
    {
        if(selectedLobby != null)
        {
            ServerInterface.Instance.JoinLobby(selectedLobby.Lobby.Name);
        }
    }

    public void DeselectLobby(LobbyButton lobbyButton)
    {
        menuView.HideLobbyDetails();
        selectedLobby = null;
    }

    public void SelectLobby(LobbyButton lobbyButton)
    {
        menuView.ShowLobbyDetails(lobbyButton.Lobby);
        if(selectedLobby != null)selectedLobby.Deselect();
        selectedLobby = lobbyButton;
    }

    private void RegisterEvents()
    {
        ServerInterface.UserCreated += UserCreated;
        ServerInterface.UserNotCreated += UserNotCreated;
        ServerInterface.Authenticated += Authenticated;
        ServerInterface.Unauthorized += Unauthorized;
        ServerInterface.LobbyList += LobbyList;
        ServerInterface.LobbyCreated += LobbyCreated;
        ServerInterface.LobbyJoined += LobbyJoined;
        ServerInterface.PlayerJoined += PlayerJoined;
        ServerInterface.PlayerLeft += PlayerLeft;
        ServerInterface.OpponentReadyUp += OpponentReadyUp;
        ServerInterface.OpponentUnready += OpponentUnready;
        ServerInterface.ReadySet += ReadySet;
        ServerInterface.UnReadySet += UnreadySet;
        ServerInterface.Error += Error;
    }
}
