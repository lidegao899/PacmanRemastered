using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public void MainMenu()
    {
        Application.LoadLevel("menu");
    }

    public void Play()
    {
        Application.LoadLevel("game");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
