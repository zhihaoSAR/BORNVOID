using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonCircle : MonoBehaviour
{
    [SerializeField]
    float duration = 2;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timeOut());
    }

    IEnumerator timeOut()
    {
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }
}
