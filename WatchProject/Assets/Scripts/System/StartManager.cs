using UnityEngine;
using System.Collections;

public class StartManager : GameSystem {

    public  float        duration    = 3f;
    private float        elapsedTime = 0f;
    private GameObject[] walls;
    private Vector3[]    finalPosition;


    void Start () {
        walls = GameObject.FindGameObjectsWithTag("Wall");
        finalPosition = new Vector3[walls.Length];


        for (int i = 0; i < walls.Length; ++i) {
            finalPosition[i] = walls[i].transform.position;
        } 
    }



    override public void OnStart () {
        elapsedTime += Time.deltaTime;

        float      ratio       = Mathf.Min(1, elapsedTime / duration);
        float      smoothRatio = Mathf.Pow((ratio - 1), 4) * -1 + 1;
        Quaternion rotation    = Quaternion.Euler(smoothRatio * 720, smoothRatio * 360, smoothRatio * 1080);

        for (int i = 0; i < walls.Length; ++i) {
            walls[i].transform.position = finalPosition[i] + (1 - smoothRatio) * new Vector3(i % 5 - 2, i % 20, i % 20);
            walls[i].transform.rotation = rotation;
        }

        if (ratio == 1) {
            Game.instance.Launch();
        }
    }


}