using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Conversacion : MonoBehaviour
{
    [SerializeField]
    string[] frases;

    Text textArea;

    int i = 0;
    bool activado = false;
    Action finisheCallback;


    public void activar(Text area, Action callback = null)
    {
        i = 1;
        textArea = area;
        textArea.text = frases[0];
        finisheCallback = callback;
        activado = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (activado)
        {
            if(Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Return))
            {
                if(i < frases.Length)
                {
                    textArea.text = frases[i];
                    i++;
                }
                else
                {
                    finisheCallback();
                    this.enabled = false;
                }
            }
            
        }
    }
}
