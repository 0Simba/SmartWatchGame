using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameHUD : MonoBehaviour {

    public Text restThrowText;
    public Text collectibleText;


    void Update () {
        restThrowText.text   = Level.instance.restThrow.ToString();
        collectibleText.text = Level.instance.collectiblePicked.ToString() + "/" + Level.instance.maxCollectible.ToString();
    }
}