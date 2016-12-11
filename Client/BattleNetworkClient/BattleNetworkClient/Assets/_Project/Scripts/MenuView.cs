using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;

public class MenuView : MonoBehaviour {

    public Text Title;

    public CanvasGroup LoginScreen;
    public InputField Username;
    public InputField Password;

    public CanvasGroup LobbyListScreen;
    public GameObject LobbyListElementPrefab;
    public Transform LobbyListElementParent;
    public Text LobbyNameDetail;
    public Text LobbyNbPlayerDetail;

    public CanvasGroup LobbyScreen;
    public Text OpponentName;
    public Text OpponentStatus;
    public Text PlayerStatus;
    public Text StatusButtonText;
    public GameObject WaitAnimation;
    public Text PlayerName;

    public AccountCreationWindow AccountCreationWindow;
    public LobbyCreationWindow LobbyCreationWindow;
    public PopupOneButton OneButtonPopup;

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

    public void Update()
    {
        if(GameModel.Instance.Menu.IsDirty)
        {
            if(!string.IsNullOrEmpty(GameModel.Instance.Menu.ErrorMessage))
            {
                OneButtonPopup.Configure("Error", GameModel.Instance.Menu.ErrorMessage, "OK", GameModel.Instance.Menu.ErrorCallback);
                GameModel.Instance.Menu.ErrorCallback = null;
                OneButtonPopup.Show();
                GameModel.Instance.Menu.ErrorMessage = "";
            }

            string modelStatus = GameModel.Instance.Menu.Status;
            if(_viewStatus != modelStatus)
            {
                StatusTransition(modelStatus);
            }

            if(GameModel.Instance.Menu.LobbyListChanged)
            {
                foreach (Transform child in LobbyListElementParent.transform)
                {
                    GameObject.Destroy(child.gameObject);
                }
                while (GameModel.Instance.Menu.LobbyList.Count > 0)
                {
                    GameObject newElement = Instantiate<GameObject>(LobbyListElementPrefab);
                    newElement.transform.SetParent(LobbyListElementParent, false);
                    newElement.GetComponentInChildren<LobbyButton>().Configure(GameModel.Instance.Menu.LobbyList.Pop());
                }
                GameModel.Instance.Menu.LobbyListChanged = false;
            }

            if(OpponentName.text != GameModel.Instance.Menu.OpponentName)
            {
                if (string.IsNullOrEmpty(GameModel.Instance.Menu.OpponentName))
                {
                    OpponentName.text = "";
                    OpponentName.gameObject.SetActive(false);
                    WaitAnimation.SetActive(true);
                }
                else
                {
                    OpponentName.text = GameModel.Instance.Menu.OpponentName;
                    OpponentName.gameObject.SetActive(true);
                    WaitAnimation.SetActive(false);
                }
            }

            if (PlayerName.text != GameModel.Instance.PlayerName) PlayerName.text = GameModel.Instance.PlayerName;

            if ((OpponentStatus.text == "Unready" && GameModel.Instance.Menu.OpponentStatus) || (OpponentStatus.text == "Ready" && !GameModel.Instance.Menu.OpponentStatus))
            {
                OpponentStatus.text = (GameModel.Instance.Menu.OpponentStatus) ? "Ready" : "Unready";
            }

            if ((PlayerStatus.text == "Unready" && GameModel.Instance.Menu.PlayerStatus) || (PlayerStatus.text == "Ready" && !GameModel.Instance.Menu.PlayerStatus))
            {
                PlayerStatus.text = (GameModel.Instance.Menu.PlayerStatus) ? "Ready" : "Unready";
                StatusButtonText.text = (GameModel.Instance.Menu.PlayerStatus) ? "Unready" : "Ready"; ;
            }


            if(GameModel.Instance.Menu.GoInGame)
            {
                GameModel.Instance.Menu.GoInGame = false;
                SceneManager.LoadScene("InGame");
            }

            GameModel.Instance.Menu.IsDirty = false;
        }
    }

    internal void ReadyUp()
    {
        throw new NotImplementedException();
    }

    public void HideLobbyDetails()
    {
        LobbyNameDetail.text = "Name : ---";
        LobbyNbPlayerDetail.text = "Nb Player : -/2";
    }

    public void ShowLobbyDetails(Lobby lobby)
    {
        LobbyNameDetail.text = "Name : " + lobby.Name;
        LobbyNbPlayerDetail.text = "Nb Player : " + lobby.NbPlayer + "/2";
    }

    public void StatusTransition(string modelStatus)
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
                ServerInterface.Instance.ListLobby();
                actualCanvaGroup = LobbyListScreen;
            }
            else
            {
                AnimateTitle(true);
                LoginScreen.gameObject.SetActive(true);
                Username.text = "";
                Password.text = "";
                LoginScreen.DOFade(1.0f, 0.5f);
                actualCanvaGroup = LoginScreen;
            }

            _viewStatus = modelStatus;
        });
    }
}
