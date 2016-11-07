using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Bootup : MonoBehaviour {

    public TextAsset sequence;
    public string[] textLines;
    public Text text;
    public float waitTime;

    private int currentLine = -1;
    private int endLine;
    private float actTime;


	// Use this for initialization
	void Start ()
    {
        textLines = (sequence.text.Split('\n'));
        endLine = textLines.Length;

	}
	
	// Update is called once per frame
	void Update ()
    {
        actTime += Time.deltaTime;
        Debug.Log(actTime);
        foreach (string i in textLines)
        {
            if (actTime >= waitTime)
            {
                if (currentLine <= endLine)
                {
                    currentLine += 1;
                    text.text = text.text + "\n" + textLines[currentLine];
                    actTime = 0;
                }
            }
        }
    }
}
