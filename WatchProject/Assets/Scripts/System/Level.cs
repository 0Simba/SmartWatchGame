using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {


    /*===================================
    =            Static Part            =
    ===================================*/

    static public void CollectiblePicked (Transform elem) {
        if (OnCollectibleTaken != null) {
            OnCollectibleTaken(elem);
        }

        instance.collectiblePicked++;
    }

    static public Level instance;

    public delegate void SimpleAction(Transform elem);
    static public event SimpleAction OnCollectibleTaken;



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
        restThrow--;
    }
}