using UnityEngine;
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
    }

    private void CommandResult(JSONNode result)
    {
        igModel.Console += '\n' + result.AsArray[0]["commandResult"];//must adapt server result
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

    public void SendCommand(string newConsoleContent)
    {
        string command = newConsoleContent.Remove(newConsoleContent.IndexOf(igModel.Console), igModel.Console.Length);
        //must add verification (at least empty command and command beginning with some char?)
        ServerInterface.Instance.SendCommand(command);
    }
}
