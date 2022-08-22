using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Board : MonoBehaviour
{
    public Transform whiteKing;
    public Transform whiteQueen;
    public Transform whiteBishop;
    public Transform whiteKnight;
    public Transform whiteRook;
    public Transform whitePawn;
    public Transform blackKing;
    public Transform blackQueen;
    public Transform blackBishop;
    public Transform blackKnight;
    public Transform blackRook;
    public Transform blackPawn;

    public GameObject cellPrefab;
    public Material whiteMaterial;
    public Material blackMaterial;
    public Material greenMaterial;

    private float cellDistance;

    public Transform[,] boardTransformMatrix;
    public List<Piece> pieces;

    public Piece currentSelectedPiece = null;
    public List<Vector2Int> currentHighlightPositions;

    public void Begin()
    {
        boardTransformMatrix = new Transform[9, 9];
        cellDistance = 1f;
        CreateBoard();
    }

    private void CreateBoard()
    {
        pieces = new List<Piece>();
        currentHighlightPositions = new List<Vector2Int>();
        int run = 0;
        for(int i = 1; i <= 8; i++)
        {
            for(int j = 1; j <= 8; j++)
            {
                run++;
                //Sinh ra o co
                GameObject obj = Instantiate(cellPrefab, Vector3.zero, Quaternion.Euler(new Vector3(90, 0, 0)));
                obj.name = i.ToString() + " " + j.ToString();
                obj.transform.position = new Vector3(j * cellDistance, 0f,i * cellDistance);
                obj.GetComponent<MeshRenderer>().material = (run % 2 == 0) ? blackMaterial : whiteMaterial;
                obj.GetComponent<Cell>().position = new Vector2Int(i, j);
                boardTransformMatrix[i, j] = obj.transform;

                //Sinh con co
                //Sinh ra tot
                Transform pieceTransform = null;
                //Trang
                if(i == 2)
                {
                    pieceTransform = Instantiate(whitePawn, boardTransformMatrix[i, j].position, Quaternion.identity);
                    pieceTransform.GetComponent<Piece>().pieceColor = PieceColor.White;
                    pieceTransform.GetComponent<Piece>().pieceType = PieceType.Pawn;
                }
                if(i == 1 && (j == 1 || j == 8))
                {
                    pieceTransform = Instantiate(whiteRook, boardTransformMatrix[i, j].position, Quaternion.identity);
                    pieceTransform.GetComponent<Piece>().pieceColor = PieceColor.White;
                    pieceTransform.GetComponent<Piece>().pieceType = PieceType.Rook;
                }
                if (i == 1 && (j == 2 || j == 7))
                {
                    pieceTransform = Instantiate(whiteKnight, boardTransformMatrix[i, j].position, Quaternion.identity);
                    pieceTransform.GetComponent<Piece>().pieceColor = PieceColor.White;
                    pieceTransform.GetComponent<Piece>().pieceType = PieceType.Knight;
                }
                if (i == 1 && (j == 3 || j == 6))
                {
                    pieceTransform = Instantiate(whiteBishop, boardTransformMatrix[i, j].position, Quaternion.identity);
                    pieceTransform.GetComponent<Piece>().pieceColor = PieceColor.White;
                    pieceTransform.GetComponent<Piece>().pieceType = PieceType.Bishop;
                }
                if (i == 1 && j == 4)
                {
                    pieceTransform = Instantiate(whiteQueen, boardTransformMatrix[i, j].position, Quaternion.identity);
                    pieceTransform.GetComponent<Piece>().pieceColor = PieceColor.White;
                    pieceTransform.GetComponent<Piece>().pieceType = PieceType.Queen;
                }
                if(i == 1 && j == 5)
                {
                    pieceTransform = Instantiate(whiteKing, boardTransformMatrix[i, j].position, Quaternion.identity);
                    pieceTransform.GetComponent<Piece>().pieceColor = PieceColor.White;
                    pieceTransform.GetComponent<Piece>().pieceType = PieceType.King;
                }
                //Den
                if (i == 7)
                {
                    pieceTransform = Instantiate(blackPawn, boardTransformMatrix[i, j].position, Quaternion.identity);
                    pieceTransform.GetComponent<Piece>().pieceColor = PieceColor.Black;
                    pieceTransform.GetComponent<Piece>().pieceType = PieceType.Pawn;
                }
                if (i == 8 && (j == 1 || j == 8))
                {
                    pieceTransform = Instantiate(blackRook, boardTransformMatrix[i, j].position, Quaternion.identity);
                    pieceTransform.GetComponent<Piece>().pieceColor = PieceColor.Black;
                    pieceTransform.GetComponent<Piece>().pieceType = PieceType.Rook;
                }
                if (i == 8 && (j == 2 || j == 7))
                {
                    pieceTransform = Instantiate(blackKnight, boardTransformMatrix[i, j].position, Quaternion.identity);
                    pieceTransform.GetComponent<Piece>().pieceColor = PieceColor.Black;
                    pieceTransform.GetComponent<Piece>().pieceType = PieceType.Knight;
                }
                if (i == 8 && (j == 3 || j == 6))
                {
                    pieceTransform = Instantiate(blackBishop, boardTransformMatrix[i, j].position, Quaternion.identity);
                    pieceTransform.GetComponent<Piece>().pieceColor = PieceColor.Black;
                    pieceTransform.GetComponent<Piece>().pieceType = PieceType.Bishop;
                }
                if (i == 8 && j == 5)
                {
                    pieceTransform = Instantiate(blackQueen, boardTransformMatrix[i, j].position, Quaternion.identity);
                    pieceTransform.GetComponent<Piece>().pieceColor = PieceColor.Black;
                    pieceTransform.GetComponent<Piece>().pieceType = PieceType.Queen;
                }
                if (i == 8 && j == 4)
                {
                    pieceTransform = Instantiate(blackKing, boardTransformMatrix[i, j].position, Quaternion.identity);
                    pieceTransform.GetComponent<Piece>().pieceColor = PieceColor.Black;
                    pieceTransform.GetComponent<Piece>().pieceType = PieceType.King;
                }

                //Neu ma pieceTransform khac null => la quan co
                if(pieceTransform != null)
                {
                    pieceTransform.GetComponent<Piece>().position = new Vector2Int(i, j);
                    pieces.Add(pieceTransform.GetComponent<Piece>());
                }
            }
            run++;
        }
    }

    public void DeselectAllPieces()
    {
        currentHighlightPositions.Clear();
        foreach (var piece in pieces)
        {
            piece.ToggleOutline(false);
        }
        ResetAllCells();
    }

    public void HighlightMovement(Piece piece) //hien cac o ma quan co "piece" di duoc
    {
        currentHighlightPositions.Clear();
        //Tat ca vi tri
        Vector2Int currentPosition = piece.position;
        Debug.Log(currentPosition);
        List<Vector2Int> allPositions = new List<Vector2Int>();
        switch (piece.pieceType)
        {
            case PieceType.Pawn:
                {
                    if(piece.pieceColor == PieceColor.White)
                    {
                        allPositions.Add(currentPosition + new Vector2Int(1, 0));
                        allPositions.Add(currentPosition + new Vector2Int(2, 0));
                    }
                    else
                    {
                        allPositions.Add(currentPosition + new Vector2Int(-1, 0));
                        allPositions.Add(currentPosition + new Vector2Int(-2, 0));
                    }
                    break;
                }
            case PieceType.Rook:
                {
                    //Duyet len phia tren
                    for(int i = currentPosition.x + 1; i <= 8; i++)
                    {
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y)))
                        {
                            break;
                        }
                        allPositions.Add(new Vector2Int(i, currentPosition.y));
                    }
                    //Duyet len phia duoi
                    for (int i = currentPosition.x - 1; i >= 1; i--)
                    {
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y)))
                        {
                            break;
                        }
                        allPositions.Add(new Vector2Int(i, currentPosition.y));
                    }
                    //Duyet sang ben trai
                    for (int i = currentPosition.y - 1; i >= 1; i--)
                    {
                        if (CheckThisPositionHasPiece(new Vector2Int(currentPosition.x, i)))
                        {
                            break;
                        }
                        allPositions.Add(new Vector2Int(currentPosition.x, i));
                    }
                    //Duyet sang ben phai
                    for (int i = currentPosition.y + 1; i <= 8; i++)
                    {
                        if (CheckThisPositionHasPiece(new Vector2Int(currentPosition.x, i)))
                        {
                            break;
                        }
                        allPositions.Add(new Vector2Int(currentPosition.x, i));
                    }
                    break;
                }
            case PieceType.Knight:
                {
                    allPositions.Add(currentPosition + new Vector2Int(-1, 2));
                    allPositions.Add(currentPosition + new Vector2Int(1, 2));
                    allPositions.Add(currentPosition + new Vector2Int(-1, -2));
                    allPositions.Add(currentPosition + new Vector2Int(1, -2));
                    allPositions.Add(currentPosition + new Vector2Int(-2, -1));
                    allPositions.Add(currentPosition + new Vector2Int(-2, 1));
                    allPositions.Add(currentPosition + new Vector2Int(2, -1));
                    allPositions.Add(currentPosition + new Vector2Int(2, 1));
                    break;
                }
        }
        //Kiem tra tinh hop le
        List<Vector2Int> validPositions = new List<Vector2Int>();
        foreach (var i in allPositions)
        {
            Debug.Log(i);
            if (!CheckThisPositionOnBoard(i))
            {
                continue;
            }
            if (!CheckThisPositionHasPiece(i))
            {
                validPositions.Add(i);
            }
        }
        //Highlight
        foreach (var i in validPositions)
        {
            currentHighlightPositions.Add(i);
            boardTransformMatrix[i.x, i.y].GetComponent<MeshRenderer>().material = greenMaterial;
        }
    }

    public void ResetAllCells()
    {
        int run = 0;
        for (int i = 1; i <= 8; i++)
        {
            for (int j = 1; j <= 8; j++)
            {
                run++;
                boardTransformMatrix[i, j].GetComponent<MeshRenderer>().material = (run % 2 == 0) ? blackMaterial : whiteMaterial;
            }
            run++;
        }
    }

    public void MoveCurrentSelectedPieceTo(Vector2Int targetPosition) //x, y 1-1 -> 8, 8
    {
        bool isFindTargetInCurrentList = false;
        foreach(var i in currentHighlightPositions)
        {
            if(targetPosition == i)
            {
                isFindTargetInCurrentList = true;
                break;
            }
        }
        if (!isFindTargetInCurrentList) return;

        GameplayController.Instance.raycastController.isEnableSelect = false;
        DeselectAllPieces();
        currentSelectedPiece.transform.DOJump(boardTransformMatrix[targetPosition.x, targetPosition.y].position, 1.25f, 1, 1f).OnComplete(
                () => {
                    currentSelectedPiece.position = targetPosition;
                    GameplayController.Instance.raycastController.isEnableSelect = true;
                }
        );
    }

    //Check vi tri co nam tren ban co hay khong
    private bool CheckThisPositionOnBoard(Vector2Int position) { 
        if(position.x < 1 || position.x > 8 || position.y < 1 || position.y > 8)
        {
            return false;
        }
        return true;
    }

    //Check vi tri nay da co quan co nao hay chua
    private bool CheckThisPositionHasPiece(Vector2Int position)
    {
        bool isFind = false;
        foreach (var j in pieces)
        {
            if (j.position == position)
            {
                isFind = true;
                break;
            }
        }
        return isFind;
    }
}
