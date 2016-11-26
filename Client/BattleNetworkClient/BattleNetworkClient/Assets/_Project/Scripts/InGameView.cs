using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;

public class InGameView : MonoBehaviour {

    public InGameController IgCtrl;
    public InputField Console;
    public PopupOneButton OneButtonPopup;
    public ScoreWindow ScoreAnimation;

    void Start()
    {
        Console.Select();
        Console.ActivateInputField();
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
