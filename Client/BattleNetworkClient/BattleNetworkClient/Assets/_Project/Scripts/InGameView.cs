using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class InGameView : MonoBehaviour {

    public InGameController IgCtrl;
    public InputField Console;
    public PopupOneButton OneButtonPopup;
    public ScoreWindow ScoreAnimation;
    public CanvasGroup GoCanvasGroup;
    public CanvasGroup NumberCanvasGroup;

    void Start()
    {
        Console.interactable = false;
    }
	
	// Update is called once per frame
	void Update () {
	    if(GameModel.Instance.Ingame.IsDirty && !GameModel.Instance.Ingame.IsGameover)
        {
            if(!string.IsNullOrEmpty(GameModel.Instance.Ingame.ErrorMessage))
            {
                Action errorAction = IgCtrl.ErrorCallback;
                OneButtonPopup.Configure("Error", GameModel.Instance.Ingame.ErrorMessage, "Ok", errorAction);
                GameModel.Instance.Ingame.ErrorMessage = "";// so we don't rise it again when the next dirty state is set
                OneButtonPopup.Show();
                return;
            }

            if(GameModel.Instance.Ingame.Countdown > 0)
            {
                StartCoroutine(CountDown(GameModel.Instance.Ingame.Countdown));
                GameModel.Instance.Ingame.Countdown = -1;
            }

            if(GameModel.Instance.Ingame.Winner != 0)
            {
                Action gameOverAction = IgCtrl.GameOverCallback;
                ScoreAnimation.Show(GameModel.Instance.Ingame.Winner, GameModel.Instance.Ingame.Team, gameOverAction);
                return;
            }

            if(!string.IsNullOrEmpty(GameModel.Instance.Ingame.ConsoleResult))
            {

                StartCoroutine(ConsoleLineByLineDisplay(GameModel.Instance.Ingame.ConsoleResult));
                GameModel.Instance.Ingame.ConsoleResult = "";
                
            }

            GameModel.Instance.Ingame.IsDirty = false;
        }
	}

    private IEnumerator CountDown(float countdown)
    {
        while(countdown > 0)
        {
            DisplayCountDownAnimation(countdown);
            yield return new WaitForSeconds(1f);
            countdown -= 1f;
        }
        DisplayGO();
    }

    private void DisplayGO()
    {
        GoCanvasGroup.gameObject.SetActive(true);
        Sequence goSequence = DOTween.Sequence();
        goSequence.Append(GoCanvasGroup.DOFade(1f, 0.1f));
        goSequence.Insert(0f, GoCanvasGroup.transform.DOScale(0f, 0.9f));
        goSequence.AppendCallback(() =>
        {
            GoCanvasGroup.gameObject.SetActive(false);
            Console.interactable = true;
            Console.Select();
            Console.ActivateInputField();
        });
        
    }

    private void DisplayCountDownAnimation(float countdown)
    {
        NumberCanvasGroup.gameObject.SetActive(true);
        Sequence numSequence = DOTween.Sequence();
        numSequence.Append(NumberCanvasGroup.DOFade(1f, 0.1f));
        numSequence.Insert(0f, NumberCanvasGroup.transform.DOScale(0f, 0.9f));
        numSequence.AppendCallback(() =>
        {
            NumberCanvasGroup.gameObject.SetActive(false);
            NumberCanvasGroup.alpha = 0;
            NumberCanvasGroup.transform.localScale = new Vector3(1f, 1f, 1f);
        });
    }

    private IEnumerator ConsoleLineByLineDisplay(string toAdd)
    {
        string[] lines = toAdd.Split('\n');
        int actualLine = 0;
        while (actualLine < lines.Length)
        {
            Console.text += '\n' + lines[actualLine];
            actualLine++;
            yield return new WaitForSeconds(0.5f);
        }

        GameModel.Instance.Ingame.Console = Console.text;
        Console.interactable = true;
        Console.Select();
        Console.ActivateInputField();
    }

    public void SubmitCommand()
    {
        Console.interactable = false;
        IgCtrl.SendCommand(Console.text);
    }
}
