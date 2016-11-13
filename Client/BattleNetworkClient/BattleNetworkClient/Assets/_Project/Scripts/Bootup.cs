using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Bootup : MonoBehaviour {

    public TextAsset bootSequenceText;
    public string[] textLines;
    public Text text;
    public Text logoText;
    public float waitTime;
    public float speed;

    public Image logo;
    public Image troll;

    private int currentLine = 0;
    private int endLine;
    private int sequence;
    private float actTime;
    

    bool LoadingSequence()
    {
        if (actTime >= waitTime & logo.fillAmount < 1f)
        {
            logo.fillAmount += 0.1f;
            actTime = 0.9f;
        }
        else if (logo.fillAmount == 1.0f & sequence == 0)
        {
            sequence += 1;
        }
        if (sequence == 1)
        {
            logoText.enabled = true;
            sequence += 1;
            actTime = 0;
        }
        if (actTime >= waitTime & sequence == 2)
        {
            if (currentLine < endLine)
            {
                text.text = text.text + "\n" + textLines[currentLine];
                currentLine += 1;
                actTime = 0;
            }
            else if (currentLine == endLine)
            {
                sequence = 3;
            }
            
        }
        if (sequence == 3)
        {
            return true;
        }
        return false;
    }

	// Use this for initialization
	void Start ()
    {
        textLines = (bootSequenceText.text.Split('\n'));
        endLine = textLines.Length;
        logoText.enabled = false;

	}
	
	// Update is called once per frame
	void Update ()
    {
        actTime += Time.deltaTime;
        LoadingSequence();
        if (sequence == 3 & Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && sequence != 3)
            DisplayTroll();
    }

    private void DisplayTroll()
    {
        Sequence trollSequence = DOTween.Sequence();
        trollSequence.Append(troll.DOFade(1.0f, 0.2f));
        trollSequence.AppendInterval(1.0f);
        trollSequence.Append(troll.DOFade(0.0f, 0.2f));

    }
}
