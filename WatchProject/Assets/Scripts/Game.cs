using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {

    public delegate void UpdateMethod ();


    static public Game instance;
    enum State { start, direction, power, movement, pause, onCheckpoint, onObjectif, die, win };


    public State state = State.start;


    void Start ()
    {
        instance = this;
    }


    void Update ()
    {

    }


    void OnTap ()
    {

    }
}