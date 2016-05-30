using UnityEngine;
using System.Collections;

public class WallPlane : MonoBehaviour {


    public float             restitution;
    public Player.BounceSide bounceSide;

    void OnTriggerEnter (Collider other) {
        if (other.tag != "Player") {
            return;
        }

        Player player = other.GetComponent<Player>();
        player.Bounce(bounceSide);

        if (bounceSide == Player.BounceSide.down) {
            player.onFloor = true;
        }
    }


    void OnTriggerExit (Collider other) {
        if (other.tag != "Player") {
            return;
        }

        Player player = other.GetComponent<Player>();

        if (bounceSide == Player.BounceSide.down) {
            player.onFloor = false;
        }
    }




}