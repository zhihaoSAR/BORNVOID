using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSearch : MonoBehaviour
{
    [HideInInspector]
    public GameObject player = null;

    public void findPlayer()
    {
        player = GameObject.FindWithTag("Player");
    }
    
    public bool playerFound()
    {
        return player != null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
        }
    }
}
