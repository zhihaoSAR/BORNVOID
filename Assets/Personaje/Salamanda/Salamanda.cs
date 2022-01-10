using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Salamanda : Enemy
{
    [SerializeField]
    protected Status status;
    [SerializeField]
    protected Animator anim;
    protected Action state;
    [SerializeField]
    protected PlayerSearch searcher;

    [SerializeField]
    protected float atkDist = 2.5f;

    [SerializeField]
    protected AudioSource running_sound;
    [SerializeField]
    protected BasicAttack basicAttack;
    //habilidades


    protected enum State { IDLE, DAMAGED, DEAD, CHASE, ATTACK, VOID, SKILL };
    protected State actState;
    protected State nextState;
    protected State previousState;
    //animation Parameter
    protected int anim_dead, anim_attack, anim_damaged, anim_run, anim_skill;

    protected NavMeshAgent agent;

    protected IEnumerator coroutine = null;
    protected bool coroutineFinished = false;

    protected bool invincible = false;

    protected Action deadCallback = null;

    public override Status Status
    {
        get
        {
            return status;
        }
        set
        {
            this.status = value;
        }
    }

    public override void dead()
    {
        nextState = State.DEAD;
    }

    public override void getHit(float damage)
    {
        if (damage == 0)
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
                activeInvincible();
                nextState = State.DAMAGED;
                searcher.findPlayer();
                StartCoroutine(timeOutWithAction(1, desactiveInvincible));
            }
        }

    }

    public override void pause()
    {
        Debug.Log("No effect");
    }

    // Start is called before the first frame update
    protected void Start()
    {
        state = idleState;
        actState = State.IDLE;
        desactiveInvincible();
        agent = GetComponent<NavMeshAgent>();
        anim_attack = Animator.StringToHash("attack");
        anim_damaged = Animator.StringToHash("damaged");
        anim_dead = Animator.StringToHash("dead");
        anim_run = Animator.StringToHash("run");
        anim_skill = Animator.StringToHash("skill");
        running_sound.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        state();
    }

    protected void exitIdle()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutineFinished = false;
        agent.SetDestination(transform.position);
        anim.SetBool(anim_run, false);
        running_sound.Pause();
        previousState = State.IDLE;
    }

    protected void idleState()
    {
        if (searcher.playerFound())
        {
            state = chaseState;
            exitIdle();
            actState = State.CHASE;
            return;
        }

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

        if (Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
        {
            anim.SetBool(anim_run, false);
            running_sound.Pause();
            if (coroutine == null)
            {
                if (coroutineFinished)
                {
                    Vector3 move = UnityEngine.Random.insideUnitSphere * 6;
                    move.y = 0;
                    agent.SetDestination(move + transform.position);
                    anim.SetBool(anim_run, true);
                    running_sound.UnPause();
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

    protected void damagedState()
    {
        if (coroutine == null)
        {
            if (!coroutineFinished)
            {
                anim.SetTrigger(anim_damaged);
                coroutine = timeOut(1);
                StartCoroutine(coroutine);
            }
            else
            {
                if (previousState == State.CHASE)
                {
                    state = chaseState;
                    actState = State.CHASE;
                }
                else
                {
                    state = idleState;
                    actState = State.IDLE;
                }
                previousState = State.DAMAGED;
                coroutineFinished = false;
            }
        }

    }

    protected void deadState()
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
                deadCallback();
            }
        }
    }

    protected void exitChase()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutineFinished = false;
        agent.SetDestination(transform.position);
        anim.SetBool(anim_run, false);
        running_sound.Pause();
        previousState = State.CHASE;
    }

    protected virtual void chaseState()
    {
        GameObject player = searcher.player;
        if (nextState == State.DEAD)
        {
            state = deadState;
            exitChase();
            actState = State.DEAD;
            nextState = State.VOID;
            return;
        }

        if (nextState == State.DAMAGED)
        {
            state = damagedState;
            exitChase();
            actState = State.DAMAGED;
            nextState = State.VOID;
            return;
        }
        if (Vector3.Distance(transform.position, player.transform.position) <= atkDist)
        {
            state = basicAtkState;
            exitChase();
            actState = State.ATTACK;
        }
        else
        {
            anim.SetBool(anim_run, true);
            running_sound.UnPause();
            agent.SetDestination(player.transform.position);
        }
    }

    protected void exitBasicAtk()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutineFinished = false;
        previousState = State.ATTACK;
        basicAttack.cancelSkill();
    }

    protected void basicAtkState()
    {
        if (nextState == State.DEAD)
        {
            state = deadState;
            exitBasicAtk();
            actState = State.DEAD;
            nextState = State.VOID;
            return;
        }

        if (nextState == State.DAMAGED)
        {
            state = damagedState;
            exitBasicAtk();
            actState = State.DAMAGED;
            nextState = State.VOID;
            return;
        }

        transform.LookAt(searcher.player.transform.position);
        if (coroutine == null)
        {
            if (!coroutineFinished)
            {
                anim.SetTrigger(anim_attack);
                coroutine = timeOut(2);
                StartCoroutine(coroutine);
                basicAttack.useSkill(damage: status.atk);
            }
            else
            {
                state = chaseState;
                actState = State.CHASE;
                previousState = State.ATTACK;
                basicAttack.cancelSkill();
                coroutineFinished = false;
            }
        }
    }


    protected IEnumerator timeOut(float time)
    {
        yield return new WaitForSeconds(time);
        coroutineFinished = true;
        coroutine = null;
    }
    protected IEnumerator timeOutWithAction(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }
    protected void activeInvincible()
    {
        invincible = true;
    }
    protected void desactiveInvincible()
    {
        invincible = false;
    }
    protected void OnTriggerEnter(Collider other)
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

    public override void setDeadCallback(Action callback)
    {
        deadCallback = callback;
    }
}
