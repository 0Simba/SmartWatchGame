using UnityEngine;
using System.Collections;

public class Aim : GameSystem {

    public  Transform pivot;
    public  Transform arrow;
    public  Transform scale;
    public  float     anglePerSeconds  = 90;
    public  float     minScale         = 1;
    public  float     maxScale         = 2;
    public  float     growFrequency    = 3;
    public  float     minPower         = 1;
    public  float     maxPower         = 10;
    private float     powerElapsedTime = 0;
    private float     scaleDifference;


    void Start () {
        scaleDifference = maxScale - minScale;

        Game.OnStateChanged += OnStateChanged;
    }


    public void OnStateChanged (Game.State state) {
        if (state == Game.State.movement) {
            pivot.gameObject.SetActive(false);
        }
        else if (state == Game.State.direction) {
            pivot.gameObject.SetActive(true);
        }
    }


    override public void OnDirection () {
        pivot.Rotate(pivot.forward * anglePerSeconds * Time.deltaTime);
    }


    override public void OnPower () {
        powerElapsedTime += Time.deltaTime;

        float power = Mathf.Sin(powerElapsedTime * Mathf.PI * 2 * growFrequency) * (scaleDifference / 2) + minPower + scaleDifference / 2;

        scale.localScale = Vector3.one * power;
    }
}