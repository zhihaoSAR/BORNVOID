using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton_big :Skeleton_normal
{
    [SerializeField]
    float skillProbability = 0.2f;

    [SerializeField]
    Skill burst;

    new void Start()
    {
        base.Start();
        burst = GetComponent<Burst>();
        state = idleState;
    }
    protected override void chaseState()
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
            Debug.Log(status);
            if (!status.buffs.ContainsKey("Burst"))
            {
                if (Random.Range(0f, 1f) < skillProbability)
                {
                    state = burstState;
                    exitChase();
                    actState = State.SKILL;
                    return;
                }
            }
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

    protected void burstState()
    {
        if (nextState == State.DEAD)
        {
            state = deadState;
            burst.cancelSkill();
            actState = State.DEAD;
            nextState = State.VOID;
            return;
        }
        if (coroutine == null)
        {
            if (!coroutineFinished)
            {
                anim.SetTrigger(anim_skill);
                coroutine = timeOut(2);
                StartCoroutine(coroutine);
                burst.useSkill(gameObject);
            }
            else
            {
                state = chaseState;
                actState = State.CHASE;
                previousState = State.SKILL;
                nextState = State.VOID;
                coroutineFinished = false;
            }
        }
    }
}
