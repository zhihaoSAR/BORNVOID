using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomManager: MonoBehaviour
{
    public abstract void playerEnter();
    public abstract void playerExit();
    public abstract void enemyDead();
    public abstract void roomCompleted();
}
