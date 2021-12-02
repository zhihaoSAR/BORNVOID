using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Enemy
{
    Status Status { get; set; }
    void dead();
    void pause();
    void getHit(float damage);
    
}
