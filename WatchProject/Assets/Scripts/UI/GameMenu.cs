using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMenu : MonoBehaviour
{
    private Game.State _prevState;
    private float _oldFillValue;

    [Header("spShootRecap Params")]
    public GameObject spPannel;
    public Text spShootRecap;
    public Image spCollectibles;
    public float spShowDuration;

    [Header("Victory Params")]
    public GameObject vPannel;
    public Text vText;
    public GameObject vReset;
    public GameObject vNext;
    public Button vMenu;
    public float vAppearDuration;

    [Header("HUD Params")]
    public GameObject hudPannel;
    public Text hudFPS;
    public Image collectibleImage;

    [Header("Pause Params")]
    public GameObject pPannel;
    public GameObject pExit;
    public GameObject pRestart;
    public GameObject pResume;
    public float pAppearTime;

    void Start()
    {
        _oldFillValue = 0;
        Game.OnBallStop += OnBallStop;
        Game.OnWin += OnWin;
        Game.OnLose += OnLose;
        Level.OnCollectibleTaken += CollectibleTaken;
        StartCoroutine(ShowHudFPS());
    }

    void CollectibleTaken()
    {
        StartCoroutine(ColletibleAlpha());
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
        vNext.SetActive(true);
        StartCoroutine(VictoryScreenAnimation());
    }

    void OnLose()
    {
        vText.text = "YOU LOSE !";
        vReset.SetActive(true);
        StartCoroutine(VictoryScreenAnimation());
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
        Debug.Log("OnRestartClick");
        Time.timeScale = 1;
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void OnNextLevel()
    {
        Debug.Log("OnNextLevel");
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
                tResume.localPosition = Vector3.Lerp(new Vector3(0, -210, 0), new Vector3(0, 0, 0), timer / pAppearTime);
            }
            else
            {
                iExit.fillAmount = Mathf.Lerp(1, 0, timer / pAppearTime);
                iRestart.fillAmount = Mathf.Lerp(1, 0, timer / pAppearTime);
                tRestart.localPosition = Vector3.Lerp(new Vector3(0, 160, 0), new Vector3(0, 370, 0), timer / pAppearTime);
                tExit.localPosition = Vector3.Lerp(new Vector3(0, -160, 0), new Vector3(0, -370, 0), timer / pAppearTime);
                tResume.localPosition = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, -210, 0), timer / pAppearTime);
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
        spShootRecap.text = Level.instance.restThrow.ToString() + " / " + Level.instance.maxThrow.ToString();
        spCollectibles.fillAmount = 0.0f;

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
        float fill = (float)Level.instance.collectiblePicked / (float)Level.instance.maxCollectible;
        while (timer <= timeAppeareance)
        {
            spCollectibles.fillAmount = Mathf.Lerp(_oldFillValue, fill, timer/ (timeAppeareance*0.5f) );
            Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), timer / vAppearDuration);
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        _oldFillValue = fill;
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
    IEnumerator VictoryScreenAnimation()
    {
        float timer = 0;
        ShowPannel("Victory");
        RectTransform _rectVictory = vPannel.GetComponent< RectTransform>();
        vReset.GetComponent<Button>().interactable = false;
        vNext.GetComponent<Button>().interactable = false;
        vMenu.interactable = false;
        while (timer < vAppearDuration)
        {
            timer += Time.deltaTime;
            _rectVictory.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), timer / vAppearDuration);
            yield return null;
        }
        vReset.GetComponent<Button>().interactable = true;
        vNext.GetComponent<Button>().interactable = true;
        vMenu.interactable = true;
    }

    IEnumerator ColletibleAlpha()
    {
        float timer = 0;
        CanvasGroup _cGroup = collectibleImage.GetComponent<CanvasGroup>();
        while (timer < 0.15f)
        {
            _cGroup.alpha = Mathf.Lerp(0.0f, 0.5f, timer / 0.15f);
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0;
        while (timer < 0.1f)
        {
            _cGroup.alpha = Mathf.Lerp(0.5f, 0.0f, timer / 0.15f);
            timer += Time.deltaTime;
            yield return null;
        }
        _cGroup.alpha = 0;
    }
}