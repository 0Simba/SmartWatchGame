using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : GameSystem {

    static public Dictionary<BounceSide, Vector3> bounceToVector;
    static public Player instance;


    public enum BounceSide { left, right, top, down };


    private Vector3 velocity;
    private float   frameVelocityAppliedRatio = 0;

    public  LayerMask      collideMask;
    public  Vector3        gravity;
    public  float          friction;
    public  float          bounceRestitution;
    public  float          minPower       = 0.1f;
    public  float          maxPower       = 1;
    public  float          velocityToStop = 1; 
    public  bool           onFloor        = false;
    public  bool           alreadyCollide = false;
    public  BounceScale    bounceScale;
    public  ParticleSystem throwParticles;

    public  Aim     aim;


    void Start () {
        instance = this;
        bounceToVector = new Dictionary<BounceSide, Vector3>();

        bounceToVector.Add(BounceSide.left,  Vector3.left);
        bounceToVector.Add(BounceSide.right, Vector3.right);
        bounceToVector.Add(BounceSide.top,   Vector3.up);
        bounceToVector.Add(BounceSide.down,  Vector3.down);

        Game.OnThrow += Throw;
    }


    void OnDestroy () {
        Game.OnThrow -= Throw;
    }


    void Throw () {
        float power = aim.ratioPower * (maxPower - minPower) + minPower;
        velocity    = aim.transform.up.normalized * power;
    }


    override public void OnMovement () {
        frameVelocityAppliedRatio = 0;
        onFloor                   = false;
        alreadyCollide            = false;
        velocity                  *= Mathf.Exp(-friction * Time.deltaTime);

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
        float radius = transform.localScale.x * 0.5f;
        if (Physics.SphereCast(transform.position, radius * 0.8f, velocity, out hitInfo, velocity.magnitude * Time.deltaTime + radius * 0.2f, collideMask)) {
            WallPlane wallPlane = hitInfo.collider.gameObject.GetComponent<WallPlane>();

            Vector3 distance   = hitInfo.point - transform.position;
            distance -= distance.normalized * radius;

            transform.position += distance;

            Bounce(wallPlane);
            frameVelocityAppliedRatio = 1;
        }
    }


    public void Bounce (WallPlane wallPlane) {
        Vector3 difference = transform.position - wallPlane.transform.position;
        float   angle      = Mathf.Atan2(difference.y, difference.x);

        if (angle > -Mathf.PI * 0.75f && angle < -Mathf.PI * 0.25f) {
            Bounce(BounceSide.top, wallPlane.restitution);
        }
        else if (angle > -Mathf.PI * 0.25f && angle < Mathf.PI * 0.25f) {
            Bounce(BounceSide.right, wallPlane.restitution);
        }
        else if (angle > Mathf.PI * 0.25f && angle < Mathf.PI * 0.75f) {
            Bounce(BounceSide.down, wallPlane.restitution);
        }
        else {
            Bounce(BounceSide.left, wallPlane.restitution);
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
        bounceScale.Apply(bounceSide, velocity.magnitude);
    }

}