using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreAnimation : MonoBehaviour {

    public string ContentText;
    public Text UIText;
    public CanvasGroup button;

    private int actualLetter;
	// Use this for initialization
	void Start () {
        UIText.text = "";
        actualLetter = 0;
        button.alpha = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (actualLetter < ContentText.Length)
        {
            UIText.text += ContentText[actualLetter];
            actualLetter++;
        }
        else
        {
            button.gameObject.SetActive(true);
            button.DOFade(1f, 0.5f);
            enabled = false;
        }
            
	}
}
