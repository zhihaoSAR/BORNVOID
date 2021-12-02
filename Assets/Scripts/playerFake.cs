using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
