using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class LobbyCreationWindow : MonoBehaviour {

    public CanvasGroup Root;
    public CanvasGroup Form;
    public CanvasGroup Wait;
    public CanvasGroup Result;
    public InputField LobbyName;
    public Text ResultText;

    private Action  _callback;

    public void Show()
    {
        Form.gameObject.SetActive(true);
        Form.alpha = 1.0f;
        Wait.gameObject.SetActive(false);
        Wait.alpha = 0f;
        Result.gameObject.SetActive(false);
        Result.alpha = 0f;
        Root.gameObject.SetActive(true);
        Root.DOFade(1.0f, 0.2f);
    }

    public void SendCreationRequest()
    {
        Form.DOFade(0.0f, 0.1f).OnComplete(() =>
        {
            ServerInterface.Instance.CreateLobby(LobbyName.text);
            LobbyName.text = "";
            Form.gameObject.SetActive(false);
            Wait.gameObject.SetActive(true);
            Wait.DOFade(1.0f, 0.1f);
        });

    }

    public void ShowResult(string result, Action callback)
    {
        _callback = callback;
        Wait.DOFade(0.0f, 0.1f).OnComplete(() =>
        {
            ResultText.text = result;
            Wait.gameObject.SetActive(false);
            Result.gameObject.SetActive(true);
            Result.DOFade(1.0f, 0.1f);
        });
    }

    public void Hide()
    {
        Root.DOFade(1.0f, 0.2f).OnComplete(() =>
        {
            Root.gameObject.SetActive(false);
            LobbyName.text = "";
            _callback();
        });
    }

    public void Cancel()
    {
        Root.DOFade(1.0f, 0.2f).OnComplete(() =>
        {
            Root.gameObject.SetActive(false);
            LobbyName.text = "";
        });
    }
}
