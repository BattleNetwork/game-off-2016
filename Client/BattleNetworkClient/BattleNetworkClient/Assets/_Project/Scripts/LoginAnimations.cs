using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginAnimations : MonoBehaviour {
    public GameObject titleText;
    public GameObject loginCanvas;
    public GameObject usernameField;
    public GameObject passwordField;
    public GameObject settingsButton;
    public Button quitButton;
    public GameObject loginButton;
    public Image loginListPanel;
    public float waitTime = 10;
    public GameObject connectButton;
    public GameObject refreshButton;
    public GameObject createButton;
    public GameObject userCreateMessagePanel;
    public Button createUserButton;
    public Button userCreateOkButton;
    public GameObject panelQuitConfirm;
    public GameObject panelLobbyReady;


    private ServerInterface serverInterface;

    private float actTime;
    private Text titleTextText;
    private int sequence = 1;
    private RectTransform qButtonRect;
    private RectTransform lButtonRect;
    private RectTransform sButtonRect;
    private RectTransform titleTextRect;
    private float alpha;

    void ReadyUp()
    {
        panelLobbyReady.SetActive(true);

    }

    void UserCreationConfirm()
    {
        userCreateMessagePanel.SetActive(true);
    }

    void CloseUserCreation()
    {
        userCreateMessagePanel.SetActive(false);
    }

    void RefreshList()
    {
        SendMessage("RefreshLobbyList");
    }

    void Login()
    {
        serverInterface.StartAuthentifiedConnection(usernameField.GetComponentInChildren<Text>().text, passwordField.GetComponentInChildren<Text>().text);
    }

    void UserCreation()
    {
        serverInterface.CreatePlayer(usernameField.GetComponentInChildren<Text>().text, passwordField.GetComponentInChildren<Text>().text);
    }

    void CreateLobby()
    {
        serverInterface.CreateLobby(usernameField.GetComponentInChildren<Text>().text);

        panelLobbyReady.SetActive(true);
        Button cancel = GameObject.Find("ButtonLobbyReadyCancel").GetComponent<Button>();
        cancel.onClick.AddListener(CancelSearch);
        serverInterface.CloseConnection();

    }

    void QuitConfirmation()
    {
        panelQuitConfirm.SetActive(true);
        Button quitConfirm = GameObject.Find("ButtonQuitConfirm").GetComponent<Button>();
        Button quitCancel = GameObject.Find("ButtonQuitCancel").GetComponent<Button>();
        quitConfirm.onClick.AddListener(Quit);
        quitCancel.onClick.AddListener(CancelQuit);
    }
    void Quit()
    {
        serverInterface.CloseConnection();
        Application.Quit();
    }
    void CancelQuit()
    {
        panelQuitConfirm.SetActive(false);
    }
    void Logout()
    {
        serverInterface.CloseConnection();
        SceneManager.LoadScene("MainMenu");
    }

    void WaitingForOpponent()
    {
    }

    void CancelSearch()
    {
        panelLobbyReady.SetActive(false);
    }

    void LobbyTransition()
    {
        switch(sequence)
        {
            case 1:
                createUserButton.gameObject.SetActive(false);
                usernameField.SetActive(false);
                passwordField.SetActive(false);
                sequence += 1;
                break;

            case 2:
                if (titleTextText.fontSize > 30 & actTime >= waitTime)
                {
                    titleTextText.fontSize -= 1;
                    actTime = 0.1f;
                }
                if (qButtonRect.anchoredPosition.y <= 95f)
                {
                    qButtonRect.anchoredPosition = new Vector2(qButtonRect.anchoredPosition.x, qButtonRect.anchoredPosition.y + 3.5f);
                    sButtonRect.anchoredPosition = new Vector2(sButtonRect.anchoredPosition.x, sButtonRect.anchoredPosition.y + 3.5f);
                    lButtonRect.anchoredPosition = new Vector2(lButtonRect.anchoredPosition.x, lButtonRect.anchoredPosition.y + 3.5f);
                }
                else if (qButtonRect.anchoredPosition.y >= 95f)
                {
                    titleTextText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 250);
                    titleTextText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
                    titleTextText.CrossFadeAlpha(0, 0.1f, true);
                    quitButton.GetComponent<Image>().CrossFadeAlpha(-1, 0.1f, true);
                    quitButton.GetComponentInChildren<Text>().CrossFadeAlpha(0, 0.1f, true);
                    loginButton.GetComponent<Image>().CrossFadeAlpha(0, 0.1f, true);
                    loginButton.GetComponentInChildren<Text>().CrossFadeAlpha(0, 0.1f, true);
                    settingsButton.GetComponent<Image>().CrossFadeAlpha(0, 0.1f, true);
                    alpha = quitButton.GetComponent<Image>().canvasRenderer.GetAlpha();
                }
                if (alpha < 0.0f)
                {
                    sequence += 1;
                }
                break;

            case 3:
                titleTextRect.anchorMax = new Vector2(0, 1);
                titleTextRect.anchorMin = new Vector2(0, 1);
                loginButton.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                loginButton.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                quitButton.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                quitButton.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                settingsButton.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);
                settingsButton.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
                titleTextText.rectTransform.anchoredPosition = new Vector2(150, -30);
                quitButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(90, -80);
                loginButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(145, -80);
                settingsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, -80);
                sequence += 1;
                break;

            case 4:
                titleTextText.CrossFadeAlpha(1, 0.1f, true);
                quitButton.GetComponent<Image>().CrossFadeAlpha(1, 0.1f, true);
                quitButton.GetComponentInChildren<Text>().CrossFadeAlpha(1, 0.1f, true);
                loginButton.GetComponent<Image>().CrossFadeAlpha(1, 0.1f, true);
                loginButton.GetComponentInChildren<Text>().CrossFadeAlpha(1, 0.1f, true);
                settingsButton.GetComponent<Image>().CrossFadeAlpha(1, 0.1f, true);
                alpha = quitButton.GetComponent<Image>().canvasRenderer.GetAlpha();
                alpha = Mathf.Round(alpha);
                if (alpha >= 1)
                {
                    sequence += 1;
                }
                break;
            case 5:
                loginListPanel.gameObject.SetActive(true);
                if (loginListPanel.fillAmount < 1)
                {
                    loginListPanel.fillAmount += 0.02f;
                }
                else if(loginListPanel.fillAmount >= 1)
                {
                    sequence += 1;
                }
                break;
            case 6:
                loginButton.GetComponentInChildren<Text>().text = "Logout";
                loginButton.GetComponent<Button>().onClick.RemoveAllListeners();
                loginButton.GetComponent<Button>().onClick.AddListener(Logout);


                refreshButton.SetActive(true);
                connectButton.SetActive(true);
                createButton.SetActive(true);

                break;

            default:
                break;
        }
    }

    // Use this for initialization
    void Start ()
    {
        serverInterface = GetComponent<ServerInterface>();

        panelQuitConfirm.SetActive(false);
        loginListPanel.gameObject.SetActive(false);
        refreshButton.SetActive(false);
        connectButton.SetActive(false);
        createButton.SetActive(false);
        userCreateMessagePanel.SetActive(false);
        panelLobbyReady.SetActive(false);

        titleTextText = titleText.GetComponent<Text>();
        titleTextRect = titleText.GetComponent<RectTransform>();
        qButtonRect = quitButton.gameObject.GetComponent<RectTransform>();
        lButtonRect = loginButton.GetComponent<RectTransform>();
        sButtonRect = settingsButton.GetComponent<RectTransform>();

        userCreateOkButton.onClick.AddListener(CloseUserCreation);
        loginButton.GetComponent<Button>().onClick.AddListener(Login);
        createUserButton.GetComponent<Button>().onClick.AddListener(UserCreation);
        quitButton.GetComponent<Button>().onClick.AddListener(QuitConfirmation);
        refreshButton.GetComponent<Button>().onClick.AddListener(RefreshList);
        createButton.GetComponent<Button>().onClick.AddListener(CreateLobby);
    }
}