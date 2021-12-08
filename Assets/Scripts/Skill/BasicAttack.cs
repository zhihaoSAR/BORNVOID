using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour, Skill
{
    [SerializeField]
    BoxCollider atk_area;
    private float damage = 0;
    public float Damage { get => this.damage; }

    public void cancelSkill()
    {
        atk_area.enabled = false;
    }

    public bool skillCompleted()
    {
        return atk_area.enabled;
    }

    public bool useSkill(GameObject obj = null, float damage = 0)
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
