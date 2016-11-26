using DG.Tweening;
using System;
using UnityEngine;

public class ScoreWindow : MonoBehaviour
{
    public CanvasGroup Root;
    public GameObject LooserAnimation;
    public GameObject WinnerAnimation;

    private Action _buttonCallback;


    public void Show(int winner, int team, Action buttonCallback)
    {
        _buttonCallback = buttonCallback;
        if (team == winner)
            ShowWinner();
        else
            ShowLooser();
    }

    private void ShowLooser()
    {
        LooserAnimation.SetActive(true);
    }

    private void ShowWinner()
    {
        WinnerAnimation.SetActive(true);
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