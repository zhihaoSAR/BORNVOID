using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerFake : MonoBehaviour
{
    [SerializeField]
    float speed = 5;
    // Update is called once per frame
    void Update()
    {
        transform.Translate((new Vector3(Input.GetAxis("Horizontal"), 0,
                                        Input.GetAxis("Vertical"))).normalized * speed
                                        * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy_atk"))
        {
            Debug.Log("aaaaa");
        }
    }
}
