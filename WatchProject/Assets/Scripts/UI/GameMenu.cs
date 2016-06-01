using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMenu : MonoBehaviour
{
    private bool _inPause = false;

    [Header("ShootRecap Params")]
    public GameObject recapShootPannel;
    public Text shootRecap;
    public Image collectibles;
    public float showDuration;

    [Header("HUD Params")]
    public GameObject hudPannel;
    public Text fps;

    [Header("Pause Params")]
    public GameObject pausePannel;
    public GameObject bExit;
    public GameObject bRestart;
    public GameObject bResume;
    public float timeToAppear;

    void Start()
    {
        Game.OnBallStop += OnBallStop;
        StartCoroutine(ShowFPS());
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

    IEnumerator ShowFPS()
    {
        while (true)
        {
            int FPS = (int)(1 / Time.deltaTime);
            fps.text = FPS.ToString();

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
        pausePannel.SetActive(true);
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
        RectTransform tExit = bExit.GetComponent<RectTransform>();
        RectTransform tRestart = bRestart.GetComponent<RectTransform>();
        RectTransform tResume = bResume.GetComponent<RectTransform>();
        bExit.GetComponent<Button>().interactable = false;
        bRestart.GetComponent<Button>().interactable = false;
        bResume.GetComponent<Button>().interactable = false;

        while (timer <= timeToAppear)
        {
            if (side)
            {
                tRestart.localPosition = Vector3.Lerp(new Vector3(0, 370, 0), new Vector3(0, 160, 0), timer / timeToAppear);
                tExit.localPosition = Vector3.Lerp(new Vector3(0, -370, 0), new Vector3(0, -160, 0), timer / timeToAppear);
                tResume.localPosition = Vector3.Lerp(new Vector3(0, -210, 0), new Vector3(0, 0, 0), timer / timeToAppear);
            }
            else
            {
                tRestart.localPosition = Vector3.Lerp(new Vector3(0, 160, 0), new Vector3(0, 370, 0), timer / timeToAppear);
                tExit.localPosition = Vector3.Lerp(new Vector3(0, -160, 0), new Vector3(0, -370, 0), timer / timeToAppear);
                tResume.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, -210, 0), timer / timeToAppear);
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

        bExit.GetComponent<Button>().interactable = side;
        bRestart.GetComponent<Button>().interactable = side;
        tResume.GetComponent<Button>().interactable = side;
    }

    // ANIMATION SHOW COUPS MENU OPEN
    IEnumerator ShowRecapShoot()
    {
        recapShootPannel.SetActive(true);
        shootRecap.text = Level.instance.restThrow.ToString() + " / " + Level.instance.maxThrow.ToString();
        collectibles.fillAmount = (float)Level.instance.collectiblePicked / (float)Level.instance.maxCollectible;

        float timeFade = showDuration * 0.2f;
        float timeAppeareance = showDuration - (showDuration * 0.4f);
        float timer = 0;
        CanvasGroup iRecapShoot = recapShootPannel.GetComponent<CanvasGroup>();

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
        recapShootPannel.SetActive(false);
        NextMove();
    }
}