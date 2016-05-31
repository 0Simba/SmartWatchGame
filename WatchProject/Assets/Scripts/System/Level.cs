using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {


    /*===================================
    =            Static Part            =
    ===================================*/

    static public void CollectiblePicked () {
        instance.collectiblePicked++;
    }

    static public Level instance;




    /*=====================================
    =            Instance Part            =
    =====================================*/

    public int maxThrow;
    
    [HideInInspector] public int maxCollectible;
    [HideInInspector] public int collectiblePicked;
    [HideInInspector] public int restThrow; 


    private void Start () {
        instance       = this;
        restThrow      = maxThrow;
        maxCollectible = GameObject.FindGameObjectsWithTag("Collectible").Length;
        Game.OnThrow   += OnThrow;
    }


    private void OnThrow () {
        maxThrow--;
    }

}