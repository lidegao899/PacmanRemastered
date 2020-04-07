using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCanvas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentScore;


    void Update()
    {
        currentScore.text = GameManager.Score.ToString();
    }
}
