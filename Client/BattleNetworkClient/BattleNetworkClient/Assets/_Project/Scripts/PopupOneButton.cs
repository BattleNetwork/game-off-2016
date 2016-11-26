using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupOneButton : MonoBehaviour
{
    public CanvasGroup Root;
    public Text Title;
    public Text Message;
    public Text ButtonText;

    private Action _buttonCallback;

    public void Configure(string title, string message, string buttonText, Action buttonCallback)
    {
        Title.text = title;
        Message.text = message;
        ButtonText.text = buttonText;
        _buttonCallback = buttonCallback;
    }

    public void Show()
    {
        Root.gameObject.SetActive(true);
        Root.DOFade(1.0f, 0.2f);
    }

    public void Hide()
    {
        Root.DOFade(0f, 0.2f).OnComplete(() =>
        {
            Root.gameObject.SetActive(false);
            _buttonCallback();
        });
    }
}