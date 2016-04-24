using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

    public float   rotateSpeed;
    public Vector3 axis;

    void Update () {
        float angle = rotateSpeed * Time.deltaTime;
        transform.Rotate(axis, angle);
    }
}