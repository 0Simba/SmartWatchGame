using UnityEngine;
using System.Collections;

public class HotZone : MonoBehaviour {

    public float        minRadius      = 1f;
    public float        maxRadius      = 2f;
    public Transform    cameraPosition;
    public Transform    cameraLookAt;
    public bool         ignoreY        = false;
    public HotZone      next;


    [HideInInspector] public Vector2 position2D;
    [HideInInspector] public float   shift;

    void Start () {
        shift         = maxRadius - minRadius;
        transform.tag = "HotZone";

        position2D = new Vector2(transform.position.x, transform.position.y);
    }


    void OnDrawGizmos () {
        Gizmos.color = new Color(1f, 0f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position, maxRadius);

        Gizmos.color = new Color(0f, 1f, 0f, 1f);
        Gizmos.DrawWireSphere(transform.position, minRadius);


        if (cameraPosition) {
            Gizmos.DrawLine(transform.position, cameraPosition.position);

            Gizmos.color = new Color(0f, 0.7f, 0.7f, 0.8f);
            Gizmos.DrawSphere(cameraPosition.position, 0.5f);


            if (cameraLookAt) {
                Gizmos.color = new Color(0f, 0f, 0.7f, 0.8f);
                Gizmos.DrawSphere(cameraLookAt.position, 0.25f);

                Gizmos.DrawLine(cameraPosition.position, cameraLookAt.position);
            }
        }

    }

}