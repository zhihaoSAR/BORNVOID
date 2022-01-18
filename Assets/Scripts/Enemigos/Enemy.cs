using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public abstract Status Status { get; set; }
    public abstract void dead();
    public abstract void pause();
    public abstract void getHit(float damage);
    public abstract void setDeadCallback(Action callback);
    
}
