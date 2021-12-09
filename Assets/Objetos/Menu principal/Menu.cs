using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    GameObject credits;
    [SerializeField]
    GameObject mainMenu;
    public static void Play()
    {
        SceneManager.LoadScene(1);
    }
    public static void Exit()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
        {
            Application.Quit();
        }
    }
    public void Credits()
    {
        credits.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void backToMain()
    {
        credits.SetActive(false);
        mainMenu.SetActive(true);
    }
}
