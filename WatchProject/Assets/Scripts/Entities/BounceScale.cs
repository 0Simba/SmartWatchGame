using UnityEngine;
using System.Collections;

public class BounceScale : MonoBehaviour {

    public float duration       = 0.2f;
    public float maxDeformation = 0.3f;
    public float maxStrength    = 10;


    private bool deforming = false;


    public void Apply (Player.BounceSide bounceSide, float strength) {
        if (deforming) {
            return;
        }

        float deformation = Mathf.Min(maxDeformation, strength / maxStrength * maxDeformation);
        StartCoroutine(Deformation(bounceSide, deformation));
    }


    IEnumerator Deformation (Player.BounceSide bounceSide, float maxDeformation) {
        deforming = true;

        Vector3 scaleSens   = (bounceSide == Player.BounceSide.left ||bounceSide == Player.BounceSide.right) ? Vector3.left : Vector3.down;
        float   elapsedTime = 0;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;

            float ratio       = Mathf.Min(elapsedTime / duration, 1);
            float smoothRatio = Mathf.Cos((ratio - 0.5f) * Mathf.PI);


            transform.localScale = Vector3.one - (-scaleSens * maxDeformation * smoothRatio);

            yield return null;
        }


        deforming = false;
    }


}