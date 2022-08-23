using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Transform canvas;
    public Transform gameEndPrefab;

    public Text currentPlayerText;
    public void Begin()
    {
        currentPlayerText.text = GameplayController.Instance.turnController.currentPlayer.ToString();
    }

    public void ShowGameEndPopup(string winnerName)
    {
        Transform popUp = Instantiate(gameEndPrefab, canvas);

        popUp.GetComponent<GameEndUI>().winnerText.text = winnerName + " win !";
    }
}
