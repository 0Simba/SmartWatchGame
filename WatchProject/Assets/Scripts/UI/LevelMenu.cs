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
    private Vector3 _center;

    public ScrollRect scrollRect;
    public GameObject prefabLevelSelectorButton;
    public GameObject container;
    public List<LevelDescription> levels = new List<LevelDescription>();

    // Use this for initialization
    void Start () {
        _currIndex = 0;
        _tContainer = container.GetComponent<RectTransform>();
        AddButtonLevels();


    }

    void AddButtonLevels()
    {
        for (int i = 0; i < levels.Count; i++)
        {
            GameObject nButton = Instantiate(prefabLevelSelectorButton, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            nButton.GetComponent<RectTransform>().localPosition = new Vector3(0, (200 * i), 0);
            nButton.GetComponent<Image>().sprite = levels[i].image;
            nButton.GetComponent<LevelIconSelector>().levelName = levels[i].levelName;
            nButton.GetComponent<Button>().onClick.AddListener(OnClickLevel);
            nButton.transform.SetParent(container.transform, false);
            levels[i].reference = nButton.GetComponent<RectTransform>();
        }
    }

    public void OnBeginDrag()
    {
        _scrollStart = _tContainer.localPosition;
    }

    public void OnEndDrag()
    {
        //_tContainer.localPosition = _center;
    }

    public void OnScroll()
    {
        Vector3 totalScrolled = _scrollStart - _tContainer.localPosition;
        if(totalScrolled.y > 80)
        {
            if ((_currIndex + 1) < levels.Count)
            {
                _currIndex++;
                _center = -levels[_currIndex].reference.localPosition;
            }
        }
        if (totalScrolled.y < -80)
        {
            if (_currIndex-1 > 0)
            {
                _currIndex--;
                _center = -levels[_currIndex].reference.localPosition;
            }
        }
    }

    public void OnClickLevel ()
    {
        string name = levels[_currIndex].levelName;
        Application.LoadLevel(name);
    }
}
