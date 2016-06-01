using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public struct LevelDescription {
    public string name;
    public string levelName;
    public Sprite image;
}

public class LevelMenu : MonoBehaviour {
    private int _currIndex;
    private RectTransform _tContainer;

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
            nButton.GetComponent<RectTransform>().position = new Vector3((300 * i), 0, 0);
            nButton.GetComponent<Image>().sprite = levels[i].image;
            nButton.GetComponent<LevelIconSelector>().levelName = levels[i].levelName;
            nButton.GetComponent<Button>().onClick.AddListener(OnClickLevel);
            nButton.transform.SetParent(container.transform, false);
        }
    }

    public void OnClickLevel ()
    {
        string name = levels[_currIndex].levelName;
        Application.LoadLevel(name);
    }

    public void onGoLeft ()
    {
        if (_currIndex <= 0)
            return;

        _tContainer.localPosition = new Vector3(_tContainer.localPosition.x + 300, 0, 0);
        _currIndex--;
    }

    public void onGoRight()
    {
        if (_currIndex >= (levels.Count - 1))
            return;

        _tContainer.localPosition = new Vector3(_tContainer.localPosition.x - 300, 0, 0);
        _currIndex++;
    }

}
