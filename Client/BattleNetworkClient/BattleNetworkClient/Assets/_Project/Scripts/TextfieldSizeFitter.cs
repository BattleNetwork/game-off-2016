using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RectTransform))]
public class TextfieldSizeFitter : MonoBehaviour {

    private Text _myText;
    private RectTransform _myRect;
    private Vector2 _initialSize;
    // Use this for initialization
    void Start () {
        _myText = GetComponentInChildren<Text>();
        _myRect = GetComponent<RectTransform>();
        _initialSize = _myRect.sizeDelta;
    }
	
	// Update is called once per frame
	void Update () {
	    if(_myText.preferredHeight > _initialSize.y)
        {
            if(!Mathf.Approximately(_myRect.sizeDelta.y,_myText.preferredHeight))
            {
                _myRect.sizeDelta = new Vector2(_myRect.sizeDelta.x, _myText.preferredHeight);
                GameObject.Find("Scroll View").GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
            }
        }
        else
        {
            _myRect.sizeDelta = new Vector2(_myRect.sizeDelta.x, _initialSize.y);
           // _myRect.anchoredPosition = new Vector2(_myRect.anchoredPosition.x , -5f); ;
        }
	}
}
