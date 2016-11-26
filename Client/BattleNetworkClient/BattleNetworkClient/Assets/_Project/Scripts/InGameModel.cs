using UnityEngine;
using System.Collections;

public class InGameModel {

	private bool _isDirty;
    private string _errorMessage;
    private int _winner;
    private string _console;
    private string _consoleResult;
    private int _team;
    private float _countdown;
    private bool _isGameover;

    public bool IsDirty
    {
        get{ return _isDirty; }
        set{ _isDirty = value;}
    }

    public bool IsGameover
    {
        get { return _isGameover; }
        set { _isGameover = value; }
    }

    public string ErrorMessage
    {
        get{ return _errorMessage; }
        set{ _errorMessage = value;}
    }

    public int Winner
    {
        get{ return _winner; }
        set{ _winner = value;}
    }

    public string Console
    {
        get{ return _console; }
        set{ _console = value;}
    }

    public string ConsoleResult
    {
        get { return _consoleResult; }
        set { _consoleResult = value; }
    }


    public int Team
    {
        get{ return _team; }
        set{ _team = value;}
    }

    public float Countdown
    {
        get{ return _countdown; }
        set{ _countdown = value;}
    }

    public InGameModel()
    {
        _isDirty = false;
        _errorMessage = "";
        _winner = 0;
        _console = "";
        _team = -1;
        _countdown = -1;
        _isGameover = false;
    }
}
