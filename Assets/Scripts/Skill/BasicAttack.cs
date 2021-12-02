using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : Skill
{
    BoxCollider collider;

    public void cancelSkill()
    {
        collider.enabled = false;
    }

    public bool skillCompleted()
    {
        return collider.enabled;
    }

    public bool useSkill(GameObject obj = null)
    {
        if (collider.enabled)
        {
            return false;
        }
        else
        {
            collider.enabled = true;
            return true;
        }
    }
}
