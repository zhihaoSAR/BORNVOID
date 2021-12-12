using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSkill : Skill {
    
    //Es mala practica ponerlo public pero sirve para debugear y hacer mas rapido el codigo de esta clase.
    SphereCollider spCollider;
    public float speed = 10f;
    public float damage = 0;
    public float duration = 5f;
    public Vector3 direction;
    public bool homming;

    public override float Damage { get => this.damage; }

    public override void cancelSkill()
    {
        spCollider.enabled = false;
    }

    public override bool skillCompleted()
    {
        return spCollider.enabled;
    }

    public override bool useSkill(GameObject obj = null, float damage = 0){return false;}



    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        duration -= Time.deltaTime;
        if (duration <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "enemy") {
            duration = 0.001f;
        }
    }
}
