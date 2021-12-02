using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Skeleton_normal : MonoBehaviour, Enemy
{
    [SerializeField]
    Status status;
    [SerializeField]
    Animator anim;
    Action state;

    enum State { IDLE, PATROLL, DEAD, PREIDLE, CHASE, ATTACK };
    State actState;
    State nextState;

    NavMeshAgent agent;

    public Status Status { get {
            return status;
        }
        set {
            this.status = value;
        }
    }

    public void dead()
    {
        nextState = State.DEAD;
    }

    public void getHit(float damage)
    {
        status.health -= damage;
        if(status.health <= 0)
        {
            dead();
        }
    }

    public void pause()
    {
        Debug.Log("No effect");
    }

    // Start is called before the first frame update
    void Start()
    {
        state = idleState;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        state();
    }

    public void idleState()
    {

    }

}
