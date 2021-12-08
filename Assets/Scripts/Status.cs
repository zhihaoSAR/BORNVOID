using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Status
{
    public float health;
    public float speed;
    public float atk;
    public Dictionary<string, int> buffs = new Dictionary<string, int>();
}
