using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinalRoom : GenericRoom
{
    public Conversacion finalizar;
    [SerializeField]
    Text textArea;
    [SerializeField]
    GameObject panel;

    bool endRead = false;

    public override void roomCompleted()
    {
        base.roomCompleted();
        panel.SetActive(true);
        finalizar.activar(textArea, finishRead);
    }
    void closePanel()
    {
        panel.SetActive(false);
    }

    void finishRead()
    {
        endRead = true;
        SceneManager.LoadScene(0);
    }
}
