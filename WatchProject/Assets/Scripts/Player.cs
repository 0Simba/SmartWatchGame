using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : GameSystem {

    static public Dictionary<BounceSide, Vector3> bounceToVector;



    public enum BounceSide { left, right, top, down };


    private Vector3 velocity;

    public  LayerMask collideMask;
    public  Vector3   gravity;
    public  float     friction;
    public  float     bounceRestitution;
    public  float     minPower       = 0.1f;
    public  float     maxPower       = 1;
    public  float     velocityToStop = 1; 
    public  bool      onFloor        = false;

    public  Aim     aim;


    void Start () {
        bounceToVector = new Dictionary<BounceSide, Vector3>();

        bounceToVector.Add(BounceSide.left,  Vector3.left);
        bounceToVector.Add(BounceSide.right, Vector3.right);
        bounceToVector.Add(BounceSide.top,   Vector3.up);
        bounceToVector.Add(BounceSide.down,  Vector3.down);

        Game.OnThrow += Throw;
    }


    void Throw () {
        float power = aim.ratioPower * (maxPower - minPower) + minPower;
        velocity    = aim.transform.up.normalized * power;
    }


    override public void OnMovement () {
        if (!onFloor) {
            velocity += gravity;            
        }


        velocity *= Mathf.Exp(-friction * Time.deltaTime);
        transform.position += velocity * Time.deltaTime;

        if (velocityToStop >= velocity.magnitude && onFloor) {
            Game.instance.EndMovement();
        }
    }


    public void Bounce (BounceSide bounceSide) {
        if (bounceSide == BounceSide.left || bounceSide == BounceSide.right) {
            velocity.x *= -1;
        }
        else {
            velocity.y *= -1;
        }


        velocity *= bounceRestitution;
    }


    void Die () {

    }


    void GoOnCheckPoint () {

    }


    void OnStart () {

    }


    void Waiting () {

    }


    void Win () {

    }
}