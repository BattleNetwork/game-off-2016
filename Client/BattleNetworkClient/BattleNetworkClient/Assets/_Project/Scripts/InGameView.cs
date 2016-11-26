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
	    if(IgCtrl.IgModel.IsDirty && !IgCtrl.IgModel.IsGameover)
        {
            if(!string.IsNullOrEmpty(IgCtrl.IgModel.ErrorMessage))
            {
                Action errorAction = IgCtrl.ErrorCallback;
                OneButtonPopup.Configure("Error", IgCtrl.IgModel.ErrorMessage, "Ok", errorAction);
                IgCtrl.IgModel.ErrorMessage = "";// so we don't rise it again when the next dirty state is set
                OneButtonPopup.Show();
                return;
            }

            if(IgCtrl.IgModel.Countdown > 0)
            {
                StartCoroutine(CountDown(IgCtrl.IgModel.Countdown));
                IgCtrl.IgModel.Countdown = -1;
            }

            if(IgCtrl.IgModel.Winner != 0)
            {
                Action gameOverAction = IgCtrl.GameOverCallback;
                ScoreAnimation.Show(IgCtrl.IgModel.Winner, IgCtrl.IgModel.Team, gameOverAction);
                return;
            }

            if(!string.IsNullOrEmpty(IgCtrl.IgModel.ConsoleResult))
            {

                StartCoroutine(ConsoleLineByLineDisplay(IgCtrl.IgModel.ConsoleResult));
                IgCtrl.IgModel.ConsoleResult = "";
                
            }

            IgCtrl.IgModel.IsDirty = false;
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

        IgCtrl.IgModel.Console = Console.text;
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
