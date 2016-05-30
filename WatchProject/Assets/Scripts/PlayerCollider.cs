using UnityEngine;
using System.Collections;

public class PlayerCollider : MonoBehaviour {

    public Player            player;
    public Vector3           ejection;
    public Player.BounceSide bounceSide;

    public void OnTriggerEnter (Collider other) {
        if (other.tag != "Wall") {
            return;
        }

        player.transform.position = transform.position + ejection;
        player.Bounce(bounceSide);
    }


    public void OnTriggerStay (Collider other) {
        if (other.tag != "Wall") {
            return;
        }

        //if (Vector3.Dot(player.velocity.normalized, ejection) < 0) {
        //    player.velocity = Vector3.zero;            
        //}
        //player.transform.position = transform.position - transform.localPosition;
    }

}