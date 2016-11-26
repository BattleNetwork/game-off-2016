using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using SimpleJSON;
using System;

public class InGameController : MonoBehaviour {

    private InGameModel igModel;

    public InGameModel IgModel
    {
        get{ return igModel; }
    }

    // Use this for initialization
    void Start () {

        igModel = new InGameModel();

        ServerInterface.Gameover += GameOver;
        ServerInterface.PlayerLeft += OpponentLeft;
        ServerInterface.Result += CommandResult;
        ServerInterface.Countdown += Countdown;
    }

    private void CommandResult(JSONNode result)
    {
        igModel.ConsoleResult += '\n' + result.AsArray[0]["commandResult"];//must adapt server result
        igModel.IsDirty = true;
    }

    private void OpponentLeft(JSONNode result)
    {
        igModel.ErrorMessage = "Your Opponent left the game... Sorry about that. \nYou will be taken to the Lobby selection";
        igModel.IsDirty = true;
    }

    private void GameOver(JSONNode result)
    {
        igModel.Winner = result.AsArray[0]["winner"].AsInt;//must adapt server result
        igModel.IsDirty = true;
    }

    private void Countdown(JSONNode result)
    {
        igModel.Countdown = result.AsArray[0]["countdown"].AsFloat;//must adapt server result
        igModel.Team = result.AsArray[0]["team"].AsInt;//must adapt server result
    }

    public void SendCommand(string newConsoleContent)
    {
        string command = newConsoleContent.Remove(newConsoleContent.IndexOf(igModel.Console), igModel.Console.Length);
        //must add verification (at least empty command and command beginning with some char?)
        ServerInterface.Instance.SendCommand(command);
    }

    public void ErrorCallback()
    {
        ReturnToMenu();
    }

    public void GameOverCallback()
    {
        ReturnToMenu();
    }

    private void ReturnToMenu()
    {
        //maybe a fade to black would be good here
        SceneManager.LoadScene("MainMenu");
    }
}
