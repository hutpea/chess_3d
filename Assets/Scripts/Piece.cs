using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector2Int position;

    public PieceColor pieceColor;
    public PieceType pieceType;
    private OutlineShader outline;

    public void Awake()
    {
        outline = GetComponent<OutlineShader>();
    }

    private void Start()
    {
        outline.enabled = false;
    }

    public void ToggleOutline(bool value) {
        outline.enabled = value;
        if (value)
        {
            GameplayController.Instance.board.currentSelectedPiece = this;
            ToggleMovement();
        }
    }

    public void ToggleMovement()
    {
        GameplayController.Instance.board.HighlightMovement(this);
    }
}
