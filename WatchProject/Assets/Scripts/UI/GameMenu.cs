using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMenu : MonoBehaviour {
    private bool _inPause = false;


    [Header("HUD Params")]
    public GameObject hudPannel;
    public Text remainingHitText;
    public Text collectibleText;

    [Header("Pause Params")]
    public GameObject pausePannel;

    void Update () {
        if(PlayerDoubleTouch())
            ShowPause();

        if (_inPause)
            return;

        remainingHitText.text   = Level.instance.restThrow.ToString();
        collectibleText.text = Level.instance.collectiblePicked.ToString() + "/" + Level.instance.maxCollectible.ToString();
    }
    void HideHUD()
    {
        hudPannel.SetActive(false);
    }

    void ShowHUD()
    {
        HidePause();
        hudPannel.SetActive(true);
    }

    void HidePause()
    {
        _inPause = false;
        pausePannel.SetActive(false);
    }

    void ShowPause()
    {
        HideHUD();
        _inPause = true;
        pausePannel.SetActive(true);
    }

    bool PlayerDoubleTouch()
    {
        return (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(1).phase == TouchPhase.Began)) 
                || (Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1));
    }

    public void OnRestartClick()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void OnExitClick()
    {
        Application.LoadLevel("Menu");
    }
}