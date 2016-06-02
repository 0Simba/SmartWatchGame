using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HomeMenu : MonoBehaviour {
    private bool _firstView;

    [Header("Transforms Params")]
    public Transform bPannelMenu;
    public Transform bPannelOptions;
    public Transform bPannelSelectLevel;

    [Header("Transforms Params")]
    public Transform bPlay;
    public Transform bExit;
    public Transform bOption;

    [Header("Animation Params")]
    public float step1duration;
    public float step2NbRotation;
    public float step2TimerPerRotation;
    public float step3duration;

	// Use this for initialization
	void Start () {
        StartCoroutine(IntroRotation());
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnPlayClick()
    {
        bPannelSelectLevel.gameObject.SetActive(true);
        bPannelMenu.gameObject.SetActive(false);
    }

    public void OnExitClick ()
    {
        Application.Quit();
    }

    public void OnOptionClick()
    {
        bPannelOptions.gameObject.SetActive(true);
        bPannelMenu.gameObject.SetActive(false);
    }

    // --------------------------------- INTRO ANIMATION --------------------------------------------------
    IEnumerator IntroRotation()
    {
        // SETUP ANIMATION ---------------------------------
        RectTransform _pannelRect = bPannelMenu.GetComponent<RectTransform>();
        RectTransform _bOptionRect = bOption.GetComponent<RectTransform>();

        Text _playText = bPlay.GetChild(0).GetComponent<Text>();
        Text _ExitText = bExit.GetChild(0).GetComponent<Text>();
        Text _OptionText = bOption.GetChild(0).GetComponent<Text>();

        _playText.enabled = false;
        bPlay.GetComponent<Button>().interactable = false;
        _ExitText.enabled = false;
        bExit.GetComponent<Button>().interactable = false;
        _OptionText.enabled = false;
        bOption.GetComponent<Button>().interactable = false;

        _bOptionRect.localScale = new Vector3(0, 0, 0);

        // ANIMATION ---------------------------------
        float timer = 0;
        while (timer < step1duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        int rotation = 0;
        while (rotation < step2NbRotation)
        {
            if(timer > step2TimerPerRotation)
            {
                rotation++;
                timer = 0;
            }
            _pannelRect.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(0, 0, 360), timer / step2TimerPerRotation));
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0;
        while (timer < step3duration)
        {
            _bOptionRect.localScale = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(1, 1, 1), timer / step3duration);
            timer += Time.deltaTime;
            yield return null;
        }

        // END ANIMATION

        _playText.enabled = true;
        bPlay.GetComponent<Button>().interactable = true;
        _ExitText.enabled = true;
        bExit.GetComponent<Button>().interactable = true;
        _OptionText.enabled = true;
        bOption.GetComponent<Button>().interactable = true;
    }
}
