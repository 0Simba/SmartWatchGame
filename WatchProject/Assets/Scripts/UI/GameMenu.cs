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
    public GameObject bExit;
    public GameObject bRestart;
    public float timeToAppear; 

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
        Time.timeScale = 1;
        StartCoroutine(MenuAnimation(false));
    }

    void ShowPause()
    {
        HideHUD();
        _inPause = true;
        Time.timeScale = 0;
        StartCoroutine(MenuAnimation(true));
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

    IEnumerator MenuAnimation(bool side)
    {
        float timer = 0;
        RectTransform tExit = bExit.GetComponent<RectTransform>();
        RectTransform tRestart = bRestart.GetComponent<RectTransform>();

        bExit.GetComponent<Button>().interactable = false;
        bRestart.GetComponent<Button>().interactable = false;

        while (timer <= timeToAppear)
        {
            if (side)
            {
                tRestart.localPosition = Vector3.Lerp(new Vector3(0, 320, 0), new Vector3(0, 160, 0), timer / timeToAppear);
                tExit.localPosition = Vector3.Lerp(new Vector3(0, -320, 0), new Vector3(0, -160, 0), timer / timeToAppear);
            }
            else
            {
                tRestart.localPosition = Vector3.Lerp(new Vector3(0, 160, 0), new Vector3(0, 320, 0), timer / timeToAppear);
                tExit.localPosition = Vector3.Lerp(new Vector3(0, -160, 0), new Vector3(0, -320, 0), timer / timeToAppear);
            }
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        if (side)
        {
            tRestart.localPosition =  new Vector3(0, 160, 0);
            tExit.localPosition = new Vector3(0, -160, 0);
        }
        else
        {
            tRestart.localPosition = new Vector3(0, 320, 0);
            tExit.localPosition = new Vector3(0, -320, 0);
        }
        pausePannel.SetActive(side);
        bExit.GetComponent<Button>().interactable = side;
        bRestart.GetComponent<Button>().interactable = side;
    }
}