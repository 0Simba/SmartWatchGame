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


        if (player.alreadyCollide) {
            return;
        }
        player.alreadyCollide = true;

        player.Bounce(bounceSide, restitution);

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