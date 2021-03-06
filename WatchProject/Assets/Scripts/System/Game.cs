using UnityEngine;
using System.Collections;

public class Game : GameSystem {

    public delegate void UpdateMethod ();
    public delegate void StateChanged (State state);
    public delegate void SimpleAction ();

    static public Game  instance;
    static public event SimpleAction OnThrow;
    static public event SimpleAction OnDirectionStart;
    static public event SimpleAction OnBallStop;
    static public event SimpleAction OnLose;
    static public event SimpleAction OnWin;

    public enum State { start, direction, power, movement, pause, onBallStop, onCheckpoint, onObjectif, lose, win };


    public Game.State state;


    void Start () {
        instance = this;
        state    = State.start;
    }


    public void Launch ()
    {
        NewMovement();
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
            OnThrow();
        }
    }


    public override void OnMovement ()
    {
    }


    public void EndMovement () {
        if (Level.instance.restThrow <= 0)
        {
            Lose();
        }
        else {
            ChangeState(Game.State.onBallStop);
            OnBallStop();
        }
    }

    public override void OnPause()
    {
        ChangeState(State.pause);
    }

    public void OnResume(State state)
    {
        ChangeState(state);
    }

    public void NewMovement()
    {
        ChangeState(Game.State.direction);
        OnDirectionStart();
    }

    public void ObjectifPicked () {
        Win();
    }


    void ChangeState (Game.State state) {
        this.state = state;
    }


    void Lose () {
        if (OnLose != null) {
            OnLose();
        } 
        ChangeState(State.lose);
    }


    void Win () {
        if (OnWin != null) {
            OnWin();
        } 
        ChangeState(State.win);
    }


    bool PlayerTap () {
        return (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0));
    }
}