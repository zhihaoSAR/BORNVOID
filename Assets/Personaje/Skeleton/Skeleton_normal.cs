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

    

    //habilidades
    Skill basicAttack;

    enum State { IDLE, DAMAGED, DEAD, PREIDLE, CHASE, ATTACK, VOID };
    State actState;
    State nextState;
    State previousState;
    //animation Parameter
    int anim_dead, anim_attack, anim_damaged, anim_run, anim_skill;

    NavMeshAgent agent;

    IEnumerator coroutine = null;
    bool coroutineFinished = false;

    bool invincible = false;

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
        if(damage == 0)
        {
            return;
        }
        if (!invincible)
        {
            status.health -= damage;
            if (status.health <= 0)
            {
                dead();
            }
            else
            {
                nextState = State.DAMAGED;
            }
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
        actState = State.IDLE;
        desactiveInvincible();
        agent = GetComponent<NavMeshAgent>();
        basicAttack = GetComponent<BasicAttack>();
        anim_attack = Animator.StringToHash("attack");
        anim_damaged = Animator.StringToHash("damaged");
        anim_dead = Animator.StringToHash("dead");
        anim_run = Animator.StringToHash("run");
        anim_skill = Animator.StringToHash("skill");
    }

    // Update is called once per frame
    void Update()
    {
        state();
    }

    void exitIdle()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutineFinished = false;
        agent.SetDestination(transform.position);
        anim.SetBool(anim_run, false);
        previousState = State.IDLE;
    }

    void idleState()
    {
        if (nextState == State.DEAD)
        {
            state = deadState;
            exitIdle();
            actState = State.DEAD;
            nextState = State.VOID;
            return;
        }

        if (nextState == State.DAMAGED)
        {
            state = damagedState;
            exitIdle();
            actState = State.DAMAGED;
            nextState = State.VOID;
            return;
        }

        if(Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
        {
            anim.SetBool(anim_run, false);
            if(coroutine == null)
            {
                if (coroutineFinished)
                {
                    Vector3 move = UnityEngine.Random.insideUnitSphere * 10;
                    move.y = 0;
                    agent.SetDestination(move + transform.position);
                    anim.SetBool(anim_run, true);
                    coroutineFinished = false;
                }
                else
                {
                    coroutine = timeOut(UnityEngine.Random.Range(1f, 3f));
                    StartCoroutine(coroutine);
                }
            }
        }
    }

    void damagedState()
    {
        if (coroutine == null)
        {
            if (!coroutineFinished)
            {
                anim.SetTrigger(anim_damaged);
                coroutine = timeOut(1);
                StartCoroutine(coroutine);
                activeInvincible();
            }
            else
            {
                if(previousState == State.IDLE)
                {
                    state = idleState;
                    previousState = State.DAMAGED;
                    actState = State.IDLE;
                    desactiveInvincible();
                    coroutineFinished = false;
                }
            }
        }
        
    }

    void deadState()
    {
        if (coroutine == null)
        {
            if (!coroutineFinished)
            {
                anim.SetTrigger(anim_dead);
                coroutine = timeOut(4);
                StartCoroutine(coroutine);
                activeInvincible();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator timeOut(float time)
    {
        yield return new WaitForSeconds(time);
        coroutineFinished = true;
        coroutine = null;
    }
    IEnumerator timeOutWithAction(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
    void activeInvincible()
    {
        invincible = true;
    }
    void desactiveInvincible()
    {
        invincible = false;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player_atk"))
        {
            Skill skill = other.GetComponent<Skill>();
            if (skill != null)
            {
                getHit(skill.Damage);
            }
        }
    }
}
