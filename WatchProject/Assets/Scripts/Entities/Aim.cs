using UnityEngine;
using System.Collections;

public class Aim : GameSystem {

    [HideInInspector] public float ratioPower = 0;
    [HideInInspector] public float angle      = 0;

    public  Transform pivot;
    public  Transform arrow;
    public  Transform scale;
    public  float     anglePerSeconds  = 90;
    public  float     minScale         = 1;
    public  float     maxScale         = 2;
    public  float     growFrequency    = 3;
    private float     powerElapsedTime = 0;
    private float     scaleDifference;


    void Start () {
        scaleDifference = maxScale - minScale;

        Game.OnThrow          += Deactivate;
        Game.OnDirectionStart += Activate;

        Deactivate();
    }


    void OnDestroy () {
        Game.OnThrow          -= Deactivate;
        Game.OnDirectionStart -= Activate;
    }


    void Deactivate () {
        pivot.gameObject.SetActive(false);
    }


    void Activate () {
        pivot.gameObject.SetActive(true);
    }


    override public void OnDirection () {
        pivot.Rotate(pivot.forward * anglePerSeconds * Time.deltaTime);

        angle = (pivot.eulerAngles.z) * Mathf.Deg2Rad;
    }


    override public void OnPower () {
        powerElapsedTime += Time.deltaTime;

        ratioPower = (Mathf.Sin(powerElapsedTime * Mathf.PI * 2 * growFrequency) + 1) / 2;

        float scaleRatio = minScale + scaleDifference * ratioPower;

        scale.localScale = Vector3.one * scaleRatio;
    }
}