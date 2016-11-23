using UnityEngine;
using System.Collections;

public class InGameModel {

	private bool _isDirty;
    private string _errorMessage;
    private int _winner;
    private string _console;

    public bool IsDirty
    {
        get{ return _isDirty; }
        set{ _isDirty = value;}
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

    public InGameModel()
    {
        _isDirty = false;
        _errorMessage = "";
        _winner = 0;
        _console = "";
    }
}
