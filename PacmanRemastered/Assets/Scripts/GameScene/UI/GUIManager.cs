using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private float initialDelay;

    [SerializeField]
    private Canvas readyCanvas;
    
    [SerializeField]
    private Canvas gameOverCanvas;
    
    [SerializeField]
    List<Image> lives = new List<Image>(3);

    private void Start()
    {
        StartCoroutine("ShowReadScreen");
        gameOverCanvas.enabled = false;
    }

    public void ShowReadyScreen()
    {
        StartCoroutine("ShowReadScreen");
    }

    public void ShowGameOverScreen()
    {
        StartCoroutine("ShowGameOver");
    }

    IEnumerator ShowReadScreen()
    {
        readyCanvas.enabled = true;
        GameManager.gameState = GameManager.GameState.Init;
        yield return new WaitForSeconds(initialDelay);
        GameManager.gameState = GameManager.GameState.Game;
        readyCanvas.enabled = false;
    }

    IEnumerator ShowGameOver()
    {
        gameOverCanvas.enabled = true;
        yield return new WaitForSeconds(3);

        LoadMenu();
    }

    public void LoadMenu()
    {
        Application.LoadLevel("menu");
        Time.timeScale = 1f;

        GameManager.DestroySelf();
    }

    public void UpdateLife(int lifeCount)
    {
        for (int i = 0; i < 3; i++)
        {
            lives[i].enabled = i < lifeCount;
        }
    }
}
