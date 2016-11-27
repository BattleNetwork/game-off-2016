using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class MenuView : MonoBehaviour {

    public Text Title;
    public CanvasGroup LoginScreen;
    public Text Username;
    public Text Password;
    public CanvasGroup LobbyListScreen;
    public CanvasGroup LobbyScreen;
    public AccountCreationWindow AccountCreationWindow;

    private string _viewStatus;
    private CanvasGroup actualCanvaGroup;

    // Use this for initialization
    void Start () {

        _viewStatus = GameModel.Instance.Menu.Status;

        if (_viewStatus == "inlobby")
        {
            SetReduceTitle();
            LobbyScreen.gameObject.SetActive(true);
            LobbyScreen.DOFade(1.0f, 0.5f);
            actualCanvaGroup = LobbyScreen;
        }
        else if(_viewStatus == "browsing")
        {
            SetReduceTitle();
            LobbyListScreen.gameObject.SetActive(true);
            LobbyListScreen.DOFade(1.0f, 0.5f);
            actualCanvaGroup = LobbyListScreen;
        }
        else
        {
            SetFullTitle();
            LoginScreen.gameObject.SetActive(true);
            LoginScreen.DOFade(1.0f, 0.5f);
            actualCanvaGroup = LoginScreen;
        }

        Title.gameObject.SetActive(true);
        Title.DOFade(1.0f, 0.5f);
        
    }

    private void SetFullTitle()
    {
        Title.GetComponent<RectTransform>().sizeDelta = new Vector2(800f, 90f);
        Title.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -75f, 0f);
    }

    private void SetReduceTitle()
    {
        Title.GetComponent<RectTransform>().sizeDelta = new Vector2(340, 65f);
        Title.GetComponent<RectTransform>().anchoredPosition = new Vector3(-90f, -30f, 0f);
    }

    private void AnimateTitle(bool full)
    {
        if(full)
        {
            Title.GetComponent<RectTransform>().DOSizeDelta(new Vector2(800f, 90f), 0.5f);
            Title.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0f, -75f), 0.5f);
        }
        else
        {
            Title.GetComponent<RectTransform>().DOSizeDelta(new Vector2(340, 65f), 0.5f);
            Title.GetComponent<RectTransform>().DOAnchorPos(new Vector2(-90f, -30f), 0.5f);
        }
        
    }

    public void NewAccount()
    {
        AccountCreationWindow.Show();
    }

    public void Login()
    {
        ServerInterface.Instance.StartAuthentifiedConnection(Username.text, Password.text);
    }

    public void Update()
    {
        if(GameModel.Instance.Menu.IsDirty)
        {
            string modelStatus = GameModel.Instance.Menu.Status;
            if(_viewStatus != modelStatus)
            {
                actualCanvaGroup.DOFade(0f, 0.2f).OnComplete(() =>
                {
                    actualCanvaGroup.gameObject.SetActive(false);
                    if (modelStatus == "inlobby")
                    {
                        AnimateTitle(false);
                        LobbyScreen.gameObject.SetActive(true);
                        LobbyScreen.DOFade(1.0f, 0.5f);
                        actualCanvaGroup = LobbyScreen;
                    }
                    else if (modelStatus == "browsing")
                    {
                        AnimateTitle(false);
                        LobbyListScreen.gameObject.SetActive(true);
                        LobbyListScreen.DOFade(1.0f, 0.5f);
                        actualCanvaGroup = LobbyListScreen;
                    }
                    else
                    {
                        AnimateTitle(true);
                        LoginScreen.gameObject.SetActive(true);
                        LoginScreen.DOFade(1.0f, 0.5f);
                        actualCanvaGroup = LoginScreen;
                    }

                    _viewStatus = modelStatus;
                });
                
                
            }
        }
    }
}
