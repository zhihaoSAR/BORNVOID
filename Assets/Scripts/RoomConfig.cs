using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomConfig", menuName = "RoomConfig")]
public class RoomConfig : ScriptableObject
{
    public Enemy[] Enemies;
    public float[] InitProbability;
    public int MaxEnemieNumAtTime;
    public int InitEnemyNum;
    public float SpawnTime;
}
