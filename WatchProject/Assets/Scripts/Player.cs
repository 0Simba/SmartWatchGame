using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : GameSystem {

    static public Dictionary<BounceSide, Vector3> bounceToVector;



    public enum BounceSide { left, right, top, down };


    private Vector3 velocity;
    private float   frameVelocityAppliedRatio = 0;
    private Vector3 ballExtremity;

    public  LayerMask collideMask;
    public  Vector3   gravity;
    public  float     friction;
    public  float     bounceRestitution;
    public  float     minPower       = 0.1f;
    public  float     maxPower       = 1;
    public  float     velocityToStop = 1; 
    public  bool      onFloor        = false;
    public  bool      alreadyCollide = false;

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
        ballExtremity  = transform.position + transform.localScale.x * velocity.normalized * 0.5f;
        onFloor        = false;
        alreadyCollide = false;
        velocity       *= Mathf.Exp(-friction * Time.deltaTime);

        CheckCollision();
        ApplyMovement();
        CheckIsEndMovement();
    }


    void CheckIsEndMovement () {
        if (velocityToStop >= velocity.magnitude && onFloor) {
            Game.instance.EndMovement();
        }
    }


    void ApplyMovement () {
        if (!onFloor) {
            velocity += gravity;            
        }

        transform.position += velocity * Time.deltaTime * (1 - frameVelocityAppliedRatio);
    }


    void CheckCollision () {
        RaycastHit hitInfo;
        if (Physics.Raycast(ballExtremity, velocity, out hitInfo, velocity.magnitude * Time.deltaTime, collideMask)) {
            WallPlane wallPlane = hitInfo.collider.gameObject.GetComponent<WallPlane>();
            Bounce(wallPlane.bounceSide, wallPlane.restitution);

            Vector3 distance          = hitInfo.point - ballExtremity;
            transform.position        -= distance;
            //frameVelocityAppliedRatio = 1;
            //frameVelocityAppliedRatio = distance.magnitude / (velocity.magnitude * Time.deltaTime);
        }
    }



    public void Bounce (BounceSide bounceSide, float wallRestitution) {
        if (bounceSide == BounceSide.left || bounceSide == BounceSide.right) {
            velocity.x *= -1;
        }
        else {
            velocity.y *= -1;
        }

        if (bounceSide == BounceSide.down) {
            onFloor = true;
        }
        velocity *= bounceRestitution * wallRestitution;
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