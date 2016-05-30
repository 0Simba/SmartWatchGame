using UnityEngine;
using System.Collections;

public class Game : GameSystem {

    public delegate void UpdateMethod ();
    public delegate void StateChanged (State state);

    static public Game  instance;
    static public event StateChanged OnStateChanged;

    public enum State { start, direction, power, movement, pause, onCheckpoint, onObjectif, die, win };




    public Game.State state;


    void Start ()
    {
        instance = this;
        state    = State.direction;
    }


    public override void OnDirection ()
    {
        if (PlayerTap()) {
            ChangeState(Game.State.power);
        }
    }


    public override void OnPower ()
    {
        if (PlayerTap()) {
            ChangeState(Game.State.movement);
        }
    }


    public override void OnMovement () {
        if (PlayerTap()) {
            ChangeState(Game.State.direction);
        }
    }


    void ChangeState (Game.State state) {
        this.state = state;

        if (Game.OnStateChanged != null) {
            Game.OnStateChanged(state);
        }
    }


    bool PlayerTap () {
        return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0));
    }
}