using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class GameSystem : MonoBehaviour {

    protected Dictionary<Game.State, Game.UpdateMethod> stateToMethod = new Dictionary<Game.State, Game.UpdateMethod>();
    protected Game.UpdateMethod updateMethod;


    public void Awake () {
        stateToMethod.Add(Game.State.start,        OnStart);
        stateToMethod.Add(Game.State.direction,    OnDirection);
        stateToMethod.Add(Game.State.power,        OnPower);
        stateToMethod.Add(Game.State.movement,     OnMovement);
        stateToMethod.Add(Game.State.pause,        OnPause);
        stateToMethod.Add(Game.State.onCheckpoint, OnCheckpoint);
        stateToMethod.Add(Game.State.onObjectif,   OnObjectif);
        stateToMethod.Add(Game.State.die,          OnDie);
        stateToMethod.Add(Game.State.win,          OnWin);
    }


    public void Update () {
        stateToMethod[Game.instance.state]();
    }


    public virtual void OnStart      () {}
    public virtual void OnDirection  () {}
    public virtual void OnPower      () {}
    public virtual void OnMovement   () {}
    public virtual void OnPause      () {}
    public virtual void OnCheckpoint () {}
    public virtual void OnObjectif   () {}
    public virtual void OnDie        () {}
    public virtual void OnWin        () {}
}