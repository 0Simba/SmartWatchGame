using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCamera : MonoBehaviour {


    /*==============================
    =            Config            =
    ==============================*/

    public  Transform target;
    public  float     maxSpeed        = 1f;
    public  float     moveSpeedCoef   = 4f;
    public  float     lookAtSpeedCoef = 4f;

    public  HotZone  firstHotZone;
    public  Vector3  normalOffset;


    private Vector3  hopePosition;
    private Vector3  lookPosition;
    private Vector3  lastLookAtPosition;

    public void Start () {
        GetHotZones();
    }


    /*==============================
    =            Update            =
    ==============================*/

    public void Update () {
        hopePosition   = target.position + normalOffset;
        lookPosition   = target.position;
        CheckHotZones();
        Move();
    }


    public void Move () {
        Vector3 deccelerateNextPosition = Vector3.Lerp(transform.position, hopePosition, moveSpeedCoef * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, deccelerateNextPosition, maxSpeed * Time.deltaTime);

        lastLookAtPosition = Vector3.Lerp(lastLookAtPosition, lookPosition, lookAtSpeedCoef * Time.deltaTime);
        transform.LookAt(lastLookAtPosition);
    }


    /*=================================
    =            Hot Zones            =
    =================================*/

    private List<HotZone> hotZones = new List<HotZone>();


    void GetHotZones () {
        hotZones.Add(firstHotZone);
        LoopHotZones();
    }


    void LoopHotZones () {
        HotZone next = hotZones[hotZones.Count - 1].next;

        if (next != null) {
            hotZones.Add(next);
            LoopHotZones();
        }
    }




    void CheckHotZones () {
        for (int i = hotZones.Count - 1; i >= 0 ; --i) {
            HotZone current = hotZones[i];
            float targetHotZoneDistance = ToHotZoneDistance(current);

            if (targetHotZoneDistance < current.maxRadius) {
                ReplaceForHotZone(current);
                return;
            }
        }
    }



    float ToHotZoneDistance (HotZone hotZone) {
        Vector3 playerPosition = target.position - hotZone.transform.position;

        return playerPosition.magnitude;
    }


    void ReplaceForHotZone (HotZone hotZone) {

        float distance = ToHotZoneDistance(hotZone);
        float ratio    = 1;

        if (distance < hotZone.minRadius) {
            ratio = 1;
        }
        else {
            ratio = 1 - (distance - hotZone.minRadius) / hotZone.shift;
        }


        hopePosition = Vector3.Lerp(hopePosition, hotZone.cameraPosition.position, ratio);
        lookPosition = Vector3.Lerp(lookPosition, hotZone.cameraLookAt.position, ratio);
    }

}