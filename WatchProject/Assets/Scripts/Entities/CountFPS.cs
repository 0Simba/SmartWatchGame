using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountFPS : MonoBehaviour {

    public Text text;

    void Start () {
        StartCoroutine(ShowFPS());
    }

    IEnumerator ShowFPS () {
        while (true) {
            int FPS = (int) (1 / Time.deltaTime);
            text.text = FPS.ToString();
            
            yield return new WaitForSeconds(0.5f);
        }
    }
}