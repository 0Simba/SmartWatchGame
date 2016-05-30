using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {

    void OnTriggerEnter (Collider other) {
        if (other.tag == "Player") {
            Destroy(gameObject);
        }
    }
}