using UnityEngine;
using System.Collections;

public class GameModel : Singleton<GameModel> {

    private MainMenuModel _menu;
    private InGameModel _ingame;
    private string _playerName;

    public MainMenuModel Menu
    {
        get
        {
            return _menu;
        }

        set
        {
            _menu = value;
        }
    }

    public InGameModel Ingame
    {
        get
        {
            return _ingame;
        }

        set
        {
            _ingame = value;
        }
    }

    public string PlayerName
    {
        get
        {
            return _playerName;
        }

        set
        {
            _playerName = value;
        }
    }

    protected GameModel()
    {
        _menu = new MainMenuModel();
        _ingame = new InGameModel();
        _playerName = "";
    }
	
}
