using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModifyTitleText : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI titleText;

    private void OnMouseEnter()
    {
        switch (name)
        {
            case "Pac-Man":
                titleText.color = Color.yellow;
                break;
            case "Blinky":
                titleText.color = Color.red;
                break;
            case "Inky":
                titleText.color = Color.cyan;
                break;
            case "Pinky":
                titleText.color = new Color(254f / 255f, 152f / 255f, 203f / 255f);
                break;
            case "Clyde":
                titleText.color = new Color(254f / 255f, 203f / 255f, 51f / 255f);
                break;
            default:
                break;
        }
        titleText.text = name;
    }

    private void OnMouseExit()
    {
        titleText.color = Color.white;
        titleText.text = "Pac-Man Remastered";
    }
}
