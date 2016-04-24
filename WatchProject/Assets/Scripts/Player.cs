using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : GameSystem {

    public enum BounceSide { left, right, top, down };


    public Vector2 gravity;
    public Vector2 velocity;
    public Arrow   arrow;


    void Start () {
        stateToMethod.Add(Game.State.start, OnStart);
    }


    void ExecuteMovement ()
    {

    }


    void Bounce (BounceSide bounceSide)
    {

    }


    void Die ()
    {

    }


    void GoOnCheckPoint ()
    {

    }


    void OnStart ()
    {

    }


    void Waiting ()
    {

    }


    void Win ()
    {

    }
}