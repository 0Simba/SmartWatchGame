using UnityEngine;
using System.Collections;

public class ForOnePage : MonoBehaviour {

    public float speed = 1;


    void Update () {
        transform.position += new Vector3(0.5f, 0.5f, 0) * Time.deltaTime * speed;
    }


}