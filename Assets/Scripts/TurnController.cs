using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public PieceColor currentPlayer;

    public void Begin()
    {
        currentPlayer = PieceColor.White;
    }

    public void SwitchPlayer()
    {
        if(currentPlayer == PieceColor.White)
        {
            currentPlayer = PieceColor.Black;
        }
        else
        {
            currentPlayer = PieceColor.White;
        }
        GameplayController.Instance.uiController.currentPlayerText.text = currentPlayer.ToString();
    }
}
