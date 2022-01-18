using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill:MonoBehaviour
{
    public abstract float Damage { get; }
    public abstract bool useSkill(GameObject obj = null, float damage = 0);
    public abstract void cancelSkill();
    public abstract bool skillCompleted();

}
