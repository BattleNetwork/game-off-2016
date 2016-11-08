using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bootup : MonoBehaviour {

    public TextAsset bootSequenceText;
    public string[] textLines;
    public Text text;
    public Text logoText;
    public float waitTime;
    public float speed;

    public Image logo;

    private int currentLine = 0;
    private int endLine;
    private int sequence;
    private float actTime;



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
        if (actTime >= waitTime & logo.fillAmount < 1f)
        {
            logo.fillAmount += 0.1f;
            actTime = 0.9f;
        }
        else if(logo.fillAmount >= 1.0f)
        {
            sequence += 1;
        }
        if (sequence == 1)
        {
            logoText.enabled = true;
            sequence += 1;
            actTime = 0;
        }
        if (actTime >= waitTime & sequence > 2)
        {
            if (currentLine < endLine)
            {
                text.text = text.text + "\n" + textLines[currentLine];
                currentLine += 1;
                actTime = 0;
            }
        }
    }
}
