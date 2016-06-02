using UnityEngine;
using System.Collections;

public class StartManager : GameSystem {

    public  float        duration    = 3f;
    private float        elapsedTime = 0f;
    private GameObject[] walls;
    private Vector3[]    finalWallsPosition;
    private GameObject[] collectibles;
    private Vector3[]    finalCollectiblesPosition;


    void Start () {
        walls = GameObject.FindGameObjectsWithTag("WallMesh");
        finalWallsPosition = new Vector3[walls.Length];


        for (int i = 0; i < walls.Length; ++i) {
            finalWallsPosition[i] = walls[i].transform.position;
        }

        collectibles              = GameObject.FindGameObjectsWithTag("Collectible");
        finalCollectiblesPosition = new Vector3[collectibles.Length];


        for (int i = 0; i < collectibles.Length; ++i) {
            finalCollectiblesPosition[i] = collectibles[i].transform.position;
        } 
    }



    override public void OnStart () {
        elapsedTime += Time.deltaTime;

        float ratio = Mathf.Min(1, elapsedTime / duration);
        WallsStart(ratio);
        CollectiblesStart(ratio);

        if (ratio == 1) {
            Game.instance.Launch();
        }
    }


    public void WallsStart (float ratio) {
        float      smoothRatio = Mathf.Pow((ratio - 1), 4) * -1 + 1;
        Quaternion rotation    = Quaternion.Euler(smoothRatio * 720, smoothRatio * 360, smoothRatio * 1080);

        for (int i = 0; i < walls.Length; ++i) {
            walls[i].transform.position = finalWallsPosition[i] + (1 - smoothRatio) * new Vector3(i % 5 - 2, i % 20, i % 20);
            walls[i].transform.rotation = rotation;
        }
    }


    public void CollectiblesStart (float ratio) {
        float smoothRatio = Mathf.Pow((ratio - 1), 4) * -1 + 1;

        for (int i = 0; i < collectibles.Length; ++i) {
            collectibles[i].transform.position = finalCollectiblesPosition[i] + (1 - smoothRatio) * new Vector3(i * 3, 10, 15);
        }
    }


}