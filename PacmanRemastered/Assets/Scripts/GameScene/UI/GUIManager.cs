using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private float initialDelay;

    [SerializeField]
    private Canvas readyCanvas;

    private void Start()
    {
        StartCoroutine("ShowReadScreen");
    }

    IEnumerator ShowReadScreen()
    {
        readyCanvas.enabled = true;
        GameManager.gameState = GameManager.GameState.Init;
        yield return new WaitForSeconds(initialDelay);
        GameManager.gameState = GameManager.GameState.Game;
        readyCanvas.enabled = false;
    }
}
