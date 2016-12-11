using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using SimpleJSON;
using System;

public class InGameController : MonoBehaviour {

    // Use this for initialization
    void Start () {
        ServerInterface.Gameover += GameOver;
        ServerInterface.PlayerLeft += OpponentLeft;
        ServerInterface.Result += CommandResult;
        ServerInterface.Countdown += Countdown;
        StartCoroutine(SendInGame());
    }

    private IEnumerator SendInGame()
    {
        yield return new WaitForEndOfFrame();
        ServerInterface.Instance.NotifyInGame();
    }

    private void CommandResult(JSONNode result)
    {
        GameModel.Instance.Ingame.ConsoleResult += '\n' + result["commandResult"];//must adapt server result
        GameModel.Instance.Ingame.IsDirty = true;
    }

    private void OpponentLeft(JSONNode result)
    {
        GameModel.Instance.Ingame.ErrorMessage = "Your Opponent left the game... Sorry about that. \nYou will be taken to the Lobby selection";
        GameModel.Instance.Ingame.IsDirty = true;
    }

    private void GameOver(JSONNode result)
    {
        GameModel.Instance.Ingame.Winner = result["winner"].AsInt;//must adapt server result
        GameModel.Instance.Ingame.IsDirty = true;
    }

    private void Countdown(JSONNode result)
    {
        GameModel.Instance.Ingame.Countdown = result["countdown"].AsFloat;//must adapt server result
        GameModel.Instance.Ingame.Team = result["team"].AsInt;//must adapt server result
    }

    public void SendCommand(string newConsoleContent)
    {
        string command = newConsoleContent.Remove(newConsoleContent.IndexOf(GameModel.Instance.Ingame.Console), GameModel.Instance.Ingame.Console.Length);
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
