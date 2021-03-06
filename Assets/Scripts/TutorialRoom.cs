using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialRoom : GenericRoom
{
    public Conversacion tutorial, finalizar;
    [SerializeField]
    Text textArea;
    [SerializeField]
    GameObject panel;

    bool endRead = false;

    public override void roomCompleted()
    {
        base.roomCompleted();
        panel.SetActive(true);
        finalizar.activar(textArea, closePanel);
    }
    public override void playerEnter() {
        StartCoroutine(startTutorial());
    }
    void closePanel()
    {
        panel.SetActive(false);
    }
    
    IEnumerator startTutorial()
    {
        tutorial.activar(textArea, finishRead);
        while (!endRead)
        {
            yield return 1;
        }
        base.playerEnter();
    }

    void finishRead()
    {
        endRead = true;
        closePanel();
    }
}
