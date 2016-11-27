using UnityEngine;
using System.Collections;
using SimpleJSON;

public class Config : MonoBehaviour {

    public static string BaseAddress;

    // Use this for initialization
    void Start () {
        TextAsset configFile = Resources.Load<TextAsset>("config");
        JSONClass config = JSONNode.Parse(configFile.text) as JSONClass;
        Config.BaseAddress = config["baseAddress"].Value;
    }
}
