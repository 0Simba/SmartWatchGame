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
    public float recapFadeDuration              = 0.2f;
    public float showCollectiblesPickedDuration = 0.5f;
    public float showRestThrowDuration          = 0.5f;

    [Header("Victory Params")]
    public GameObject vPannel;
    public GameObject vScorePannel;
    public Text vScore;
    public Text vText;
    public GameObject vReset;
    public GameObject vNext;
    public GameObject vHome;
    public Button vMenu;
    public float vAppearDuration;
    public float vButtonRotationDuration = 0.5f;

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
        _oldCollValue = 1;
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
        StartCoroutine("ShowRecapShoot");
    }

    void NextMove()
    {
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
        StopCoroutine("ShowRecapShoot");
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
        Time.timeScale = 1;
        Application.LoadLevel("Menu");
    }

    public void OnResumeClick()
    {
        Debug.Log("OnResumeClick");
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
        float timer = 0;

        CanvasGroup iRecapShoot = spPannel.GetComponent<CanvasGroup>();
        spShootRecapTot.text = Level.instance.maxThrow.ToString();
        spShootRecapAct.text = _oldShootValue.ToString();

        while (timer <= recapFadeDuration)
        {
            iRecapShoot.alpha = Mathf.Lerp(0, 1, timer / recapFadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;

        int actThrow = Level.instance.restThrow;
        float collPicked = (float)Level.instance.collectiblePicked;

        while (timer <= showCollectiblesPickedDuration)
        {
            spCollectiblesAct.text = Mathf.Round(Mathf.Lerp(_oldCollValue, collPicked, timer / showCollectiblesPickedDuration * 0.8f)).ToString();
            timer += Time.deltaTime;
            yield return null;
        }
        spCollectiblesAct.text = collPicked.ToString();

        timer = 0;
        bool restThrowUpdatedDuration = false;
        while (timer <= showRestThrowDuration) {
            if (!restThrowUpdatedDuration && timer >= showRestThrowDuration / 2) {
                spShootRecapAct.text = actThrow.ToString();
            }

            timer += Time.deltaTime;
            yield return null;
        }
        spShootRecapAct.text = actThrow.ToString();
        _oldCollValue = Mathf.Max(collPicked, 1);
        _oldShootValue = Mathf.Max(actThrow, 1);

        timer = 0;
        while (timer <= recapFadeDuration)
        {
            iRecapShoot.alpha = Mathf.Lerp(1, 0, timer / recapFadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        iRecapShoot.alpha = 0;

        ShowPannel("HUD");
        NextMove();
    }

    // ANIMATION ON VICTORY APPEAR
    IEnumerator VictoryScreenAnimation(bool victory)
    {
        float timer = 0;
        ShowPannel("Victory");
        RectTransform _rectVictory   = vPannel.GetComponent<RectTransform>();
        CanvasGroup   _canvasVictory = vPannel.GetComponent<CanvasGroup>();

        vReset.GetComponent<Button>().interactable = false;
        vHome.GetComponent<Button>().interactable = false;
        vNext.GetComponent<Button>().interactable = false;

        vMenu.interactable = false;

        vReset.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
        vNext.transform.rotation  = Quaternion.Euler(Vector3.forward * 90);
        vHome.transform.rotation  = Quaternion.Euler(Vector3.forward * -90);

        float timer1 = vAppearDuration * 0.4f;
        float timer2 = vAppearDuration * 0.6f;
        while (timer < timer1)
        {
            timer += Time.deltaTime;
            float ratio = Mathf.Min(1, timer / timer1);
            _canvasVictory.alpha = ratio;
            yield return null;
        }
        _canvasVictory.alpha = 1;
        _rectVictory.localScale = Vector3.one;


        timer = 0;
        float mScore = _oldCollValue * _oldShootValue * 10;
        if (victory)
        {
            while (timer < timer2)
            {
                vScore.text = Mathf.Round(Mathf.Lerp(0, mScore, (timer / timer2 * 0.7f))).ToString();
                timer += Time.deltaTime;
                yield return null;
            }
        }
        vScore.text = mScore.ToString();
        timer = 0;
        while (timer < vButtonRotationDuration)
        {
            float ratio = Mathf.Min(1, timer / vButtonRotationDuration);
            vReset.transform.rotation = Quaternion.Euler(Vector3.Lerp(Vector3.forward * 90, Vector3.zero, ratio));
            vNext.transform.rotation = Quaternion.Euler(Vector3.Lerp(Vector3.forward * 90, Vector3.zero, ratio));
            vHome.transform.rotation = Quaternion.Euler(Vector3.Lerp(Vector3.forward * -90, Vector3.zero, ratio));

            timer += Time.deltaTime;
            yield return null;
        }

        vReset.transform.rotation = Quaternion.Euler(Vector3.zero);
        vNext.transform.rotation = Quaternion.Euler(Vector3.zero);
        vHome.transform.rotation = Quaternion.Euler(Vector3.zero);

        vMenu.interactable = true;
        vReset.GetComponent<Button>().interactable = true;
        vHome.GetComponent<Button>().interactable = true;
        vNext.GetComponent<Button>().interactable = true;
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
        _rec.localPosition = position;
        float time = 0.5f;
        while (timer < time)
        {
            _rec.localPosition = Vector3.Slerp(position, new Vector3(160, -160, 0), timer / time);
            _rec.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer / time);
            timer += Time.deltaTime;
            yield return null;
        }
        _rec.localPosition = new Vector3(160, -160, 0);
        timer = 0;
    }
}