using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Skill
{
    [SerializeField]
    BoxCollider atk_area;
    private float damage = 0;
    public override float Damage { get => this.damage; }

    public override void cancelSkill()
    {
        atk_area.enabled = false;
    }

    public override bool skillCompleted()
    {
        return atk_area.enabled;
    }

    public override bool useSkill(GameObject obj = null, float damage = 0)
    {
        this.damage = damage;
        if (atk_area.enabled)
        {
            return false;
        }
        else
        {
            atk_area.enabled = true;
            return true;
        }
    }
}
