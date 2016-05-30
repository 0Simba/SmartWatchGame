using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainCamera : MonoBehaviour {


    /*==============================
    =            Config            =
    ==============================*/

    public  Transform target;

    public  HotZone  firstHotZone;
    public  Vector3  normalOffset;

    public void Start () {
        GetHotZones();
    }


    /*==============================
    =            Update            =
    ==============================*/

    public void Update () {
        transform.position = target.position + normalOffset;
        CheckHotZones();
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


        transform.position = Vector3.Lerp(transform.position, hotZone.cameraPosition.position, ratio);
        transform.LookAt(Vector3.Lerp(target.position, hotZone.cameraLookAt.position, ratio));

    }

}