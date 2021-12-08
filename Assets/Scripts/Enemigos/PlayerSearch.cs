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
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player = collision.gameObject;
        }
    }
}
