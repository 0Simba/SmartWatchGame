using UnityEngine;
using System.Collections;

public class Objectifs : MonoBehaviour {

    public  Disapear disapear;
    private bool     picked = false;


    void OnTriggerEnter (Collider other) {
        if (other.tag != "Player") {
            return;
        }

        disapear.Launch();
        Game.instance.ObjectifPicked();
    }
}