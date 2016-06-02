using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

[System.Serializable]
public class LevelDescription {
    public string name;
    public string levelName;
    public Sprite image;
    [HideInInspector]
    public RectTransform reference;
}

public class LevelMenu : MonoBehaviour {
    private int _currIndex;
    private RectTransform _tContainer;
    private Vector3 _scrollStart;

    private float _snapPoint;
    private float _scrollForStep;
    private bool _isScrolling;

    private float _scrollStepDown;
    private float _scrollStepUp;
    private float _refStepUp;
    private float _refStepDown;

    public float appearTime = 1;
    public float spacing = 200;
    public ScrollRect scrollRect;
    public GameObject prefabLevelSelectorButton;
    public GameObject container;
    public List<LevelDescription> levels = new List<LevelDescription>();

    // Use this for initialization
    void Start () {
        _currIndex = 0;
        _snapPoint = 0; 
        _tContainer = container.GetComponent<RectTransform>();
        AddButtonLevels();
    }

    public void Appeareance()
    {
        StartCoroutine(AppearAnim());
    }

    void AddButtonLevels()
    {
        float offset = -(_tContainer.rect.height * 0.5f) + Screen.height * 0.5f;
        for (int i = 0; i < levels.Count; i++)
        {
            GameObject nButton = Instantiate(prefabLevelSelectorButton, Vector3.zero, Quaternion.identity) as GameObject;
            nButton.GetComponent<RectTransform>().localPosition = new Vector3(0, offset + (spacing * i), 0);
            nButton.GetComponent<Image>().sprite = levels[i].image;
            nButton.GetComponent<LevelIconSelector>().levelName = levels[i].levelName;
            nButton.GetComponent<Button>().onClick.AddListener(OnClickLevel);
            nButton.transform.SetParent(container.transform, false);
            nButton.GetComponentInChildren<Text>().text = levels[i].name;
            levels[i].reference = nButton.GetComponent<RectTransform>();
        }
        _scrollForStep = (levels[0].reference.rect.height + spacing) * 0.4f;
    }

    public void OnBeginDrag()
    {
        _isScrolling = true;
        _scrollStart = _tContainer.localPosition;
        _refStepUp = _scrollForStep;
        _refStepDown = -_scrollForStep;
    }

    public void OnEndDrag()
    {
        _isScrolling = false;
        scrollRect.verticalNormalizedPosition = _snapPoint;
    }

    public void OnScroll()
    {
        if (!_isScrolling)
            return;
        Vector3 totalScrolled = _scrollStart - _tContainer.localPosition;

        if (totalScrolled.y > (_refStepUp))
        {
            if ((_currIndex + 1) < levels.Count)
            {
                _refStepUp += _scrollForStep;
                _refStepDown += _scrollForStep;

                _currIndex++;
                _snapPoint = Mathf.Abs(((_currIndex * (250) + Screen.height * 0.5f) - Screen.height*0.5f) / _tContainer.rect.height);
            }
        }
        if (totalScrolled.y < _refStepDown)
        {
            if (_currIndex-1 >= 0)
            {
                _refStepUp -= _scrollForStep;
                _refStepDown -= _scrollForStep;

                _currIndex--;
                _snapPoint = Mathf.Abs(((_currIndex * (250) + Screen.height * 0.5f) - Screen.height * 0.5f) / _tContainer.rect.height);
            }
        }
    }

    public void OnClickLevel ()
    {
        string name = levels[_currIndex].levelName;
        Application.LoadLevel(name);
    }

    IEnumerator AppearAnim()
    {
        float _timer = 0;
        RectTransform _rec = transform.GetComponent<RectTransform>();
        while (_timer < appearTime)
        {
            _rec.localPosition = Vector3.Lerp(new Vector3(0, 320, 0), Vector3.zero, _timer/ appearTime);
            _timer += Time.deltaTime;
            yield return null;
        }
        _rec.localPosition = Vector3.zero;
    }
}
