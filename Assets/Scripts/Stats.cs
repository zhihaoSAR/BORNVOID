using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {
    //Character or Enemy Stats
    public int health;
    public int maxHealth;
    public int damage;
    public int projectileDamage;
    //Only full to 1 so is a continue value from 0 to 1
    public float stamine;
    public int speed;
    public float attackSpeed;
    public float attackTime;
    public float attackTimer;
    //Only public for test
    public bool inAttack;
    
    public float dashTime;
    public float dashTimer;
    //Only public for test.
    public bool inDash;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
