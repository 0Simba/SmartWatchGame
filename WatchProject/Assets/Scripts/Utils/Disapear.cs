using UnityEngine;
using System.Collections;

public class Disapear : MonoBehaviour {

    public float disapearDuration;


    public void Launch () {
        StartCoroutine(Set());
    }

    IEnumerator Set () {
        Vector3 startScale  = transform.localScale;
        float   elapsedTime = 0;

        while (elapsedTime < disapearDuration) {
            elapsedTime += Time.deltaTime;
            float ratio = Mathf.Min(1, elapsedTime / disapearDuration);

            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, ratio);
            yield return null;
        }

        Destroy(gameObject);
    }


}