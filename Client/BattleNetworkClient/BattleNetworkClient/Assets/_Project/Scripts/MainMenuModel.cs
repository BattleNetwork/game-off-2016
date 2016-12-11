using SimpleJSON;
using System.Collections.Generic;
using System;

public class MainMenuModel
{
    private bool _isDirty;
    private string _status;
    private string _errorMessage;
    private Action _errorCallback;
    private Stack<Lobby> _lobbyList;
    private bool _lobbylistChanged;
    private string _opponentName;
    private bool _opponentStatus;
    private bool _playerStatus;
    private bool _goInGame;

    public bool IsDirty
    {
        get { return _isDirty; }
        set { _isDirty = value; }
    }

    public bool GoInGame
    {
        get { return _goInGame; }
        set { _goInGame = value; }
    }

    public bool PlayerStatus
    {
        get { return _playerStatus; }
        set { _playerStatus = value; }
    }

    public Action ErrorCallback
    {
        get { return _errorCallback; }
        set { _errorCallback = value; }
    }

    public bool OpponentStatus
    {
        get { return _opponentStatus; }
        set { _opponentStatus = value; }
    }

    public string Status
    {
        get { return _status; }
        set { _status = value; }
    }

    public string OpponentName
    {
        get { return _opponentName; }
        set { _opponentName = value; }
    }

    public Stack<Lobby> LobbyList
    {
        get { return _lobbyList; }
        set
        {
            _lobbyList = value;
            _lobbylistChanged = true;
        }
    }

    public bool LobbyListChanged
    {
        get { return _lobbylistChanged; }
        set { _lobbylistChanged = value; }
    }

    public string ErrorMessage
    {
        get { return _errorMessage; }
        set { _errorMessage = value; }
    }

    public MainMenuModel()
    {
        _isDirty = false;
        _status = "login";
        _errorMessage = "";
        _lobbylistChanged = false;
        _lobbyList = new Stack<Lobby>();
    }

    public void PlayerJoined(string name, bool status)
    {
        _opponentName = name;
        _opponentStatus = status;
        _isDirty = true;
    }

    public void PlayerLeft()
    {
        _opponentName = "";
        _isDirty = true;
    }

    public void SetOpponentStatus(bool isReady)
    {
        _opponentStatus = isReady;
        _isDirty = true;
    }
}