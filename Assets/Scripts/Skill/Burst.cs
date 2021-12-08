using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour, Skill
{
    public float Damage => 0;

    [SerializeField]
    float increment = 5;
    [SerializeField]
    float duration = 10;

    IEnumerator coroutine;

    Status status = null;

    public void cancelSkill()
    {
        Debug.Log("no effect");
        return;
    }

    public bool skillCompleted()
    {
        return true;
    }

    public bool useSkill(GameObject obj = null, float damage = 0)
    {
        if(obj == null)
        {
            return false;
        }
        status = obj.GetComponent<Enemy>().Status;
        Debug.Log(status);
        int stack = -1;
        
        if (!status.buffs.TryGetValue("Burst", out stack))
        {
            status.atk += increment;
            status.buffs.Add("Burst", 1);
        }
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = timeout();
        StartCoroutine(coroutine);
        return true;
    }

    IEnumerator timeout()
    {
        yield return new WaitForSeconds(duration);
        status.atk -= increment;
        status.buffs.Remove("Burst");
    }

}
