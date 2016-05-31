using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

    void OnTriggerEnter (Collider other) {
        if (other.tag == "Player") {
            Level.CollectiblePicked();
            Destroy(gameObject);
        }
    }
}