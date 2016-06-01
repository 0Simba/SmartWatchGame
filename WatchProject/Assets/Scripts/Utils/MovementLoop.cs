using UnityEngine;
using System.Collections;

public class MovementLoop : MonoBehaviour {

    public AnimationCurve ratioTimeToRatioPosition;
    private Vector3 initialPosition;
    public Vector3 offSet;
    public float duration;
    
    void Start ()
    {
        initialPosition = transform.position; 
    }
    
    void Update ()
    {
        float ratioTime = (Time.time % duration) / duration;
        float ratioPosition = ratioTimeToRatioPosition.Evaluate(ratioTime);
        transform.position = Vector3.Lerp(initialPosition, initialPosition + offSet, ratioPosition);
    } 
}
