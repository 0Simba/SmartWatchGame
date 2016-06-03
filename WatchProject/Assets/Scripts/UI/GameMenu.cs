using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMenu : MonoBehaviour
{
    private Game.State _prevState;
    private float _oldCollValue;
    private int _oldShootValue;
    private Camera _cam;

    [Header("spShootRecap Params")]
    public GameObject spPannel;
    public Text spShootRecapAct;
    public Text spShootRecapTot;
    public Text spCollectiblesAct;
    public float spShowDuration;

    [Header("Victory Params")]
    public GameObject vPannel;
    public GameObject vScorePannel;
    public Text vScore;
    public Text vText;
    public GameObject vReset;
    public GameObject vNext;
    public Button vMenu;
    public float vAppearDuration;

    [Header("HUD Params")]
    public GameObject hudPannel;
    public Text hudFPS;
    public GameObject hudCollectibleImage;

    [Header("Pause Params")]
    public GameObject pPannel;
    public GameObject pExit;
    public GameObject pRestart;
    public GameObject pResume;
    public float pAppearTime;

    void Start()
    {
        _cam = Camera.main;
        Game.OnBallStop          += OnBallStop;
        Game.OnWin               += OnWin;
        Game.OnLose              += OnLose;
        Level.OnCollectibleTaken += CollectibleTaken;
        _oldShootValue = Level.instance.maxThrow;
        _oldCollValue = 0;
        StartCoroutine(ShowHudFPS());
    }


    void OnDestroy () {
        Game.OnBallStop          -= OnBallStop;
        Game.OnWin               -= OnWin;
        Game.OnLose              -= OnLose;
        Level.OnCollectibleTaken -= CollectibleTaken;
    }


    void CollectibleTaken (Transform elem) {
        StartCoroutine(ColletibleAlpha(elem.position));
    }


    void ShowPannel(string name)
    {
        spPannel.SetActive(false);
        hudPannel.SetActive(false);
        vPannel.SetActive(false);
        pPannel.SetActive(false);
        switch (name)
        {
            case "HUD":
                hudPannel.SetActive(true);
                break;
            case "Pause":
                pPannel.SetActive(true);
                break;
            case "Victory":
                vPannel.SetActive(true);
                break;
            case "ShootRecap":
                spPannel.SetActive(true);
                break;
        }
    }

    void OnWin()
    {
        vText.text = "YOU WIN !";
        vScorePannel.SetActive(true);
        vNext.SetActive(true);
        StartCoroutine(VictoryScreenAnimation(true));
    }

    void OnLose()
    {
        vText.text = "LOSE !";
        vScorePannel.SetActive(false);
        vReset.SetActive(true);
        StartCoroutine(VictoryScreenAnimation(false));
    }

    void Update()
    {
        if (PlayerDoubleTouch())
            ShowPause();
    }

    void OnBallStop()
    {
        Game.instance.OnPause();
        Time.timeScale = 0;
        StartCoroutine(ShowRecapShoot());
    }

    void NextMove()
    {
        Time.timeScale = 1;
        Game.instance.NewMovement();
    }

    void ShowHUD()
    {
        ShowPannel("HUD");
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

    public void SkipRecapScreen()
    {
        StopCoroutine(ShowRecapShoot());
        ShowPannel("HUD");
        NextMove();
    }

    void HidePause()
    {
        Game.instance.OnResume(_prevState);
        Time.timeScale = 1;
        StartCoroutine(MenuAnimation(false));
    }

    void ShowPause()
    {
        _prevState = Game.instance.state;
        Game.instance.OnPause();
        ShowPannel("Pause");
        Time.timeScale = 0;
        StartCoroutine(MenuAnimation(true));
    }

    bool PlayerDoubleTouch()
    {
        return (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began && Input.GetTouch(1).phase == TouchPhase.Began))
                || (Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1));
    }


    // CLICK PARAMS ---------------------------------------------------------------

    public void OnRestartClick()
    {
        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void OnNextLevel()
    {
        Time.timeScale = 1;
        int idLevel = Application.loadedLevel;
        idLevel++;
        if (idLevel < Application.levelCount)
            Application.LoadLevel(idLevel);
        else
            Application.LoadLevel("Menu");
    }

    public void OnExitClick()
    {
        Debug.Log("OnExitClick");
        Time.timeScale = 1;
        Application.LoadLevel("Menu");
    }

    public void OnResumeClick()
    {
        Time.timeScale = 1;
        HidePause();
    }


    // ANIMATION PAUSE MENU OPEN
    IEnumerator MenuAnimation(bool side)
    {
        float timer = 0;
        RectTransform tExit = pExit.GetComponent<RectTransform>();
        RectTransform tRestart = pRestart.GetComponent<RectTransform>();
        RectTransform tResume = pResume.GetComponent<RectTransform>();
        Image iExit = pExit.transform.GetChild(0).GetComponentInChildren<Image>();
        Image iRestart = tRestart.transform.GetChild(0).GetComponentInChildren<Image>();
        iExit.fillAmount = 0;
        iRestart.fillAmount = 0;

        pExit.GetComponent<Button>().interactable = false;
        pRestart.GetComponent<Button>().interactable = false;
        pResume.GetComponent<Button>().interactable = false;



        while (timer <= pAppearTime)
        {
            if (side)
            {
                iExit.fillAmount = Mathf.Lerp(0, 1, timer / pAppearTime);
                iRestart.fillAmount = Mathf.Lerp(0, 1, timer / pAppearTime);
                tRestart.localPosition = Vector3.Lerp(new Vector3(0, 370, 0), new Vector3(0, 160, 0), timer / pAppearTime);
                tExit.localPosition = Vector3.Lerp(new Vector3(0, -370, 0), new Vector3(0, -160, 0), timer / pAppearTime);
                tResume.localPosition = Vector3.Lerp(new Vector3(0, -210, 0), Vector3.zero, timer / pAppearTime);
            }
            else
            {
                iExit.fillAmount = Mathf.Lerp(1, 0, timer / pAppearTime);
                iRestart.fillAmount = Mathf.Lerp(1, 0, timer / pAppearTime);
                tRestart.localPosition = Vector3.Lerp(new Vector3(0, 160, 0), new Vector3(0, 370, 0), timer / pAppearTime);
                tExit.localPosition = Vector3.Lerp(new Vector3(0, -160, 0), new Vector3(0, -370, 0), timer / pAppearTime);
                tResume.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0, -210, 0), timer / pAppearTime);
            }
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        if (side)
        {
            iExit.fillAmount = 1;
            iRestart.fillAmount = 1;
            tRestart.localPosition = new Vector3(0, 160, 0);
            tExit.localPosition = new Vector3(0, -160, 0);
            tResume.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            iExit.fillAmount = 0;
            iRestart.fillAmount = 0;
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
        ShowPannel("ShootRecap");
        float timeFade = spShowDuration * 0.2f;
        float timeAppeareance = spShowDuration - (spShowDuration * 0.4f);
        float timer = 0;

        CanvasGroup iRecapShoot = spPannel.GetComponent<CanvasGroup>();
        spShootRecapTot.text = Level.instance.maxThrow.ToString();
        spShootRecapAct.text = _oldShootValue.ToString();

        while (timer <= timeFade)
        {
            iRecapShoot.alpha = Mathf.Lerp(0, 1, timer / timeFade);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        timer = 0;

        int actThrow = Level.instance.restThrow-1;
        float collPicked = (float)Level.instance.collectiblePicked;

        while (timer <= timeAppeareance)
        {
            spShootRecapAct.text = Mathf.Round(Mathf.Lerp(_oldShootValue, actThrow, timer / timeAppeareance * 0.5f)).ToString();
            spCollectiblesAct.text = Mathf.Round(Mathf.Lerp(_oldCollValue, collPicked, timer / timeAppeareance*0.8f)).ToString();
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        _oldCollValue = collPicked;
        _oldShootValue = actThrow;

        timer = 0;
        while (timer <= timeFade)
        {
            iRecapShoot.alpha = Mathf.Lerp(1, 0, timer / timeFade);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        ShowPannel("HUD");
        NextMove();
    }

    // ANIMATION ON VICTORY APPEAR
    IEnumerator VictoryScreenAnimation(bool victory)
    {
        float timer = 0;
        ShowPannel("Victory");
        RectTransform _rectVictory = vPannel.GetComponent< RectTransform>();
        vReset.GetComponent<Button>().interactable = false;
        vNext.GetComponent<Button>().interactable = false;
        vMenu.interactable = false;

        float timer1 = vAppearDuration * 0.4f;
        float timer2 = vAppearDuration * 0.6f;
        while (timer < timer1)
        {
            timer += Time.deltaTime;
            _rectVictory.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, timer / timer1);
            yield return null;
        }

        _rectVictory.localScale = Vector3.one;
        timer = 0;
        while (timer < timer2 || victory)
        {
            if (victory)
                vScore.text = Mathf.Round(Mathf.Lerp(0, _oldCollValue * _oldShootValue * 10, (timer / timer2 * 0.7f))).ToString();

            yield return null;
        }

        vReset.GetComponent<Button>().interactable = true;
        vNext.GetComponent<Button>().interactable = true;
        vMenu.interactable = true;
    }

    IEnumerator ColletibleAlpha (Vector3 posObject) {
        GameObject collectiblePrefab = Instantiate(hudCollectibleImage, Vector3.zero, Quaternion.identity) as GameObject;
        Image collectibleImage = collectiblePrefab.GetComponent<Image>();
        collectibleImage.transform.SetParent(hudPannel.transform);

        float timer = 0;
        CanvasGroup _cGroup = collectibleImage.GetComponent<CanvasGroup>();
        RectTransform _rec = collectibleImage.GetComponent<RectTransform>();
        Vector3 position = _cam.WorldToViewportPoint(posObject);
        position = new Vector3(position.x, position.y, 0);
        _rec.localScale = Vector3.one;
        //_cGroup.alpha = 0;
        _rec.localPosition = position;
        float time = 0.5f;
        while (timer < time)
        {
            _rec.localPosition = Vector3.Slerp(position, new Vector3(160, -160, 0), timer / time);
            _rec.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / time);
            //_cGroup.alpha = Mathf.Lerp(1.0f, 0f, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        _rec.localPosition = new Vector3(160, -160, 0);
        timer = 0;
        //_cGroup.alpha = 0;
    }
}