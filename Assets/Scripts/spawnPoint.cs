using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnPoint : MonoBehaviour
{
    //Get the Prefab of the Enemy to spawn
    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start() {
        //Spawn the Enemy
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
