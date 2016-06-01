using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMenu : MonoBehaviour
{
    private bool _inPause = false;

    [Header("spShootRecap Params")]
    public GameObject spPannel;
    public Text spShootRecap;
    public Image spCollectibles;
    public float spShowDuration;

    [Header("Victory Params")]
    public GameObject vPannel;
    public Text vText;
    public Button vReset;
    public Button vMenu;
    public float vAppearDuration;

    [Header("HUD Params")]
    public GameObject hudPannel;
    public Text hudFPS;

    [Header("Pause Params")]
    public GameObject pPannel;
    public GameObject pExit;
    public GameObject pRestart;
    public GameObject pResume;
    public float pAppearTime;

    void Start()
    {
        Game.OnBallStop += OnBallStop;
        Game.OnWin += OnWin;
        Game.OnLose += OnLose;
        StartCoroutine(ShowHudFPS());
    }

    void OnWin()
    {
        vText.text = "YOU WIN !";
        StartCoroutine(VictoryScreenAnimation());
    }

    void OnLose()
    {
        vText.text = "YOU LOSE !";
        StartCoroutine(VictoryScreenAnimation());
    }

    void Update()
    {
        if (PlayerDoubleTouch())
            ShowPause();
    }

    void OnBallStop()
    {
        Time.timeScale = 0;
        StartCoroutine(ShowRecapShoot());
    }

    void NextMove()
    {
        Time.timeScale = 1;
        Game.instance.NewMovement();
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

    IEnumerator ShowHudFPS()
    {
        while (true)
        {
            int fps = (int)(1 / Time.deltaTime);
            hudFPS.text = fps.ToString();

            yield return new WaitForSeconds(0.5f);
        }
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
        pPannel.SetActive(true);
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

    public void OnResumeClick()
    {
        ShowHUD();
    }


    // ANIMATION PAUSE MENU OPEN
    IEnumerator MenuAnimation(bool side)
    {
        float timer = 0;
        RectTransform tExit = pExit.GetComponent<RectTransform>();
        RectTransform tRestart = pRestart.GetComponent<RectTransform>();
        RectTransform tResume = pResume.GetComponent<RectTransform>();
        pExit.GetComponent<Button>().interactable = false;
        pRestart.GetComponent<Button>().interactable = false;
        pResume.GetComponent<Button>().interactable = false;

        while (timer <= pAppearTime)
        {
            if (side)
            {
                tRestart.localPosition = Vector3.Lerp(new Vector3(0, 370, 0), new Vector3(0, 160, 0), timer / pAppearTime);
                tExit.localPosition = Vector3.Lerp(new Vector3(0, -370, 0), new Vector3(0, -160, 0), timer / pAppearTime);
                tResume.localPosition = Vector3.Lerp(new Vector3(0, -210, 0), new Vector3(0, 0, 0), timer / pAppearTime);
            }
            else
            {
                tRestart.localPosition = Vector3.Lerp(new Vector3(0, 160, 0), new Vector3(0, 370, 0), timer / pAppearTime);
                tExit.localPosition = Vector3.Lerp(new Vector3(0, -160, 0), new Vector3(0, -370, 0), timer / pAppearTime);
                tResume.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, -210, 0), timer / pAppearTime);
            }
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        if (side)
        {
            tRestart.localPosition = new Vector3(0, 160, 0);
            tExit.localPosition = new Vector3(0, -160, 0);
            tResume.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            tRestart.localPosition = new Vector3(0, 370, 0);
            tExit.localPosition = new Vector3(0, -370, 0);
            tResume.localPosition = new Vector3(0, -210, 0);
        }

        pExit.GetComponent<Button>().interactable = side;
        pRestart.GetComponent<Button>().interactable = side;
        tResume.GetComponent<Button>().interactable = side;
    }

    // ANIMATION SHOW COUPS MENU OPEN
    IEnumerator ShowRecapShoot()
    {
        spPannel.SetActive(true);
        spShootRecap.text = Level.instance.restThrow.ToString() + " / " + Level.instance.maxThrow.ToString();
        spCollectibles.fillAmount = (float)Level.instance.collectiblePicked / (float)Level.instance.maxCollectible;

        float timeFade = spShowDuration * 0.2f;
        float timeAppeareance = spShowDuration - (spShowDuration * 0.4f);
        float timer = 0;
        CanvasGroup iRecapShoot = spPannel.GetComponent<CanvasGroup>();

        while (timer <= timeFade)
        {
            iRecapShoot.alpha = Mathf.Lerp(0, 1, timer / timeFade);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        timer = 0;
        while (timer <= timeAppeareance)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        timer = 0;
        while (timer <= timeFade)
        {
            iRecapShoot.alpha = Mathf.Lerp(1, 0, timer / timeFade);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        spPannel.SetActive(false);
        NextMove();
    }

    // ANIMATION ON VICTORY APPEAR
    IEnumerator VictoryScreenAnimation()
    {
        float timer = 0;
        vPannel.SetActive(true);
        RectTransform _rectVictory = vPannel.GetComponent< RectTransform>();
        vReset.interactable = false;
        vMenu.interactable = false;
        while (timer < vAppearDuration)
        {
            timer += Time.deltaTime;
            _rectVictory.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), timer / vAppearDuration);
            yield return null;
        }
        vReset.interactable = true;
        vMenu.interactable = true;
    }
}