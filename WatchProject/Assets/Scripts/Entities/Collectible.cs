using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

    public  Disapear disapear;
    private bool     picked = false;

    void OnTriggerEnter (Collider other) {
        if (other.tag == "Player" && !picked) {
            picked = true;
            disapear.Launch();
            Level.CollectiblePicked();
        }
    }
}