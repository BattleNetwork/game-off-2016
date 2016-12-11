using UnityEngine;
using System.Collections;

public class WaitAnimation : MonoBehaviour {

    public float RefreshTime;

    private float elapsedTime;

    void Start()
    {
        elapsedTime = 0f;
    }
	// Update is called once per frame
	void Update () {
	    if(elapsedTime < RefreshTime)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            elapsedTime = 0f;
            transform.Rotate(0f, 0f, -90f);
        }
	}
}
