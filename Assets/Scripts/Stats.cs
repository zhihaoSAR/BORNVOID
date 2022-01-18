using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {
    //Character or Enemy Stats
    public float health;
    public int maxHealth;
    public int damage;
    public int projectileDamage;
    //Only full to 1 so is a continue value from 0 to 1
    public float stamine;
    public int speed;
    public float attackSpeed;
    //Time until next attack
    public float attackTimeP;
    public float attackTimer;
    //Only public for test
    public bool inAttack;
    //timers for check if the player is during attack
    public float attackTime;
    public float durationAttack;


    
    //Dash Time es el maximo tiempo que puede estar en el dash, left Time es el tiempo que queda en el dash.
    public float dashTime;
    public float dashLeftTime;
    //Dash Cooldown es el tiempo, timer la variable que mantiene el track del tiempo hasta la carga
    public float dashCooldown;
    public float dashTimer;
    //Only public for test.
    public bool inDash;
    //Dash Speed Multiplier es el multiplicador de velocidad del dash
    public float dashSpeedMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        stamine = (dashCooldown - dashTimer) / dashCooldown;
    }

    public void getDamage(float damage) {
        //Takes in damage and subtracts it from health, if health is less than 0, set health to 0
        if (!inDash) {
            health -= damage;
            if (health < 0) {
                health = 0;
            }
        }
    }

    public bool isDead() {
        //Returns true if health is 0
        return health == 0;
    }
}
