using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems; 


public class ScrollViewEvents : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public LevelMenu Levelmenu;

	// Use this for initialization
	void Start () {
	
	}

    // Update is called once per frame
    public void OnBeginDrag(PointerEventData data)
    {
        Levelmenu.OnBeginDrag();
    }

    public void OnEndDrag(PointerEventData data)
    {
        Levelmenu.OnEndDrag();
    }
}
