using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public enum BounceSide { left, right, top, down };


    public Vector2 velocity;
    public Arrow   arrow;

    private Game.UpdateMethod updateMethod;
    private Dictionary<Game.State, Game.UpdateMethod> stateToMethod = new Dictionary<Game.State, Game.UpdateMethod>();



    void Start () {
        stateToMethod.Add(Game.Start, OnStart);
    }


    void Update () {
        stateToMethod[Game.instance.state]();
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