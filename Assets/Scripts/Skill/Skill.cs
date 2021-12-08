using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Skill
{
    float Damage { get; }
    bool useSkill(GameObject obj = null, float damage = 0);
    void cancelSkill();
    bool skillCompleted();

}
