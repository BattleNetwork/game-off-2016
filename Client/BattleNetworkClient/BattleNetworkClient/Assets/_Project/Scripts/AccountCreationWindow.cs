using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AccountCreationWindow : MonoBehaviour
{

    public CanvasGroup Root;
    public CanvasGroup Form;
    public CanvasGroup Wait;
    public CanvasGroup Result;
    public InputField Username;
    public InputField Password;
    public Text ResultText;

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
            ServerInterface.Instance.CreatePlayer(Username.text, Password.text);
            Username.text = "";
            Password.text = "";
            Form.gameObject.SetActive(false);
            Wait.gameObject.SetActive(true);
            Wait.DOFade(1.0f, 0.1f);
        });
        
    }

    public void ShowResult(string result)
    {
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
            Username.text = "";
            Password.text = "";
        });
    }
}