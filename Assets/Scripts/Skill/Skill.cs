using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Skill 
{
    bool useSkill(GameObject obj = null);
    void cancelSkill();
    bool skillCompleted();

}
