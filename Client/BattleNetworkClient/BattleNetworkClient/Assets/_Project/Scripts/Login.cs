using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Login : MonoBehaviour {
    public GameObject titleText;
    public GameObject loginCanvas;
    public GameObject usernameField;
    public GameObject passwordField;
    public GameObject settingsButton;
    public GameObject quitButton;
    public GameObject loginButton;
    public float waitTime = 10;
    public int successOrFail; //debug: 0 = successful connection, 1 = fail.

    private float actTime;
    private float loginWaitTime = 5.0f;
    private Text titleTextText;
    private int connectionStatus= -1;
    private int sequence = 1;
    private RectTransform qButtonRect;
    private RectTransform lButtonRect;
    private RectTransform sButtonRect;
    private RectTransform titleTextRect;




    public void LobbyConnection()
    {
        while (connectionStatus < 0)
        {
            actTime += Time.deltaTime;
            if (successOrFail == 0 & actTime >= loginWaitTime) //change to connection result SUCCESS when implemented
            {
                GameObject.Find("LoggingInText").GetComponent<Text>().text = "Connected.";
                connectionStatus = 0;
            }
            else if (successOrFail == 1 & actTime >= loginWaitTime)
            {
                GameObject.Find("LoggingInText").GetComponent<Text>().text = "Connection Failed.";
                connectionStatus = 1;
            }
        }
    }

    void LobbyTransition()
    {
        switch(sequence)
        {
            case 1:
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
                    sequence += 1;
                }
                break;
            case 3:
                titleTextText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 250);
                titleTextText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
                titleTextText.CrossFadeAlpha(0, 0.1f, true);
                quitButton.GetComponent<Image>().CrossFadeAlpha(0, 0.1f, true);
                quitButton.GetComponentInChildren<Text>().CrossFadeAlpha(0, 0.1f, true);
                loginButton.GetComponent<Image>().CrossFadeAlpha(0, 0.1f, true);
                loginButton.GetComponentInChildren<Text>().CrossFadeAlpha(0, 0.1f, true);
                settingsButton.GetComponent<Image>().CrossFadeAlpha(0, 0.1f, true);
                //titleTextRect.anchorMax = new Vector2(0, 1);
                //titleTextRect.anchorMin = new Vector2(0, 1);
                break;

        }
    }

    // Use this for initialization
    void Start ()
    {
        titleTextText = titleText.GetComponent<Text>();
        titleTextRect = titleText.GetComponent<RectTransform>();
        Button lButton = loginButton.GetComponent<Button>();
        lButton.onClick.AddListener(LobbyConnection);
        qButtonRect = quitButton.GetComponent<RectTransform>();
        lButtonRect = loginButton.GetComponent<RectTransform>();
        sButtonRect = settingsButton.GetComponent<RectTransform>();

    }

    // Update is called once per frame
    void Update ()
    {
        actTime += Time.deltaTime;
        if (connectionStatus == 0)
        {
            LobbyTransition();
        }
	}
}
