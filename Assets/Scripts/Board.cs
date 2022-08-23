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
    public Material redMaterial;

    private float cellDistance;

    public Transform[,] boardTransformMatrix;
    public List<Piece> pieces;

    public Piece currentSelectedPiece = null;
    public List<Vector2Int> currentMovementPositions;
    public List<Vector2Int> currentAttackPositions;

    public void Begin()
    {
        boardTransformMatrix = new Transform[9, 9];
        cellDistance = 1f;
        CreateBoard();
    }

    private void CreateBoard()
    {
        pieces = new List<Piece>();
        currentMovementPositions = new List<Vector2Int>();
        currentAttackPositions = new List<Vector2Int>();
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
        currentMovementPositions.Clear();
        foreach (var piece in pieces)
        {
            piece.ToggleOutline(false);
        }
        ResetAllCells();
    }

    public void HighlightMovement(Piece piece) //hien cac o ma quan co "piece" di duoc
    {
        currentMovementPositions.Clear();
        //Tat ca vi tri
        Vector2Int currentPosition = piece.position;
        List<Vector2Int> allPositions = new List<Vector2Int>();
        switch (piece.pieceType)
        {
            case PieceType.Pawn:
                {
                    if(piece.pieceColor == PieceColor.White)
                    {
                        allPositions.Add(currentPosition + new Vector2Int(1, 0));
                        if(currentPosition.x == 2)
                            allPositions.Add(currentPosition + new Vector2Int(2, 0));
                    }
                    else
                    {
                        allPositions.Add(currentPosition + new Vector2Int(-1, 0));
                        if(currentPosition.x == 7)
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
            case PieceType.Bishop:
                {
                    //cheo tren trai
                    for(int i = currentPosition.x + 1; i <= 8; i++)
                    {
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y - (i - currentPosition.x)))) {
                            break;
                        }
                        allPositions.Add(new Vector2Int(i, currentPosition.y - (i - currentPosition.x)));
                    }
                    //cheo tren phai
                    for (int i = currentPosition.x + 1; i <= 8; i++)
                    {
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y + (i - currentPosition.x))))
                        {
                            break;
                        }
                        allPositions.Add(new Vector2Int(i, currentPosition.y + (i - currentPosition.x)));
                    }
                    //cheo duoi trai
                    for (int i = currentPosition.x - 1; i >= 1; i--)
                    {
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y - (i - currentPosition.x))))
                        {
                            break;
                        }
                        allPositions.Add(new Vector2Int(i, currentPosition.y - (i - currentPosition.x)));
                    }
                    //cheo duoi phai
                    for (int i = currentPosition.x - 1; i >= 1; i--)
                    {
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y + (i - currentPosition.x))))
                        {
                            break;
                        }
                        allPositions.Add(new Vector2Int(i, currentPosition.y + (i - currentPosition.x)));
                    }
                    break;
                }
            case PieceType.Queen:
                {
                    //Duyet len phia tren
                    for (int i = currentPosition.x + 1; i <= 8; i++)
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
                    //cheo tren trai
                    for (int i = currentPosition.x + 1; i <= 8; i++)
                    {
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y - (i - currentPosition.x))))
                        {
                            break;
                        }
                        allPositions.Add(new Vector2Int(i, currentPosition.y - (i - currentPosition.x)));
                    }
                    //cheo tren phai
                    for (int i = currentPosition.x + 1; i <= 8; i++)
                    {
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y + (i - currentPosition.x))))
                        {
                            break;
                        }
                        allPositions.Add(new Vector2Int(i, currentPosition.y + (i - currentPosition.x)));
                    }
                    //cheo duoi trai
                    for (int i = currentPosition.x - 1; i >= 1; i--)
                    {
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y - (i - currentPosition.x))))
                        {
                            break;
                        }
                        allPositions.Add(new Vector2Int(i, currentPosition.y - (i - currentPosition.x)));
                    }
                    //cheo duoi phai
                    for (int i = currentPosition.x - 1; i >= 1; i--)
                    {
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y + (i - currentPosition.x))))
                        {
                            break;
                        }
                        allPositions.Add(new Vector2Int(i, currentPosition.y + (i - currentPosition.x)));
                    }
                    break;
                }
            case PieceType.King:
                {
                    for(int i = currentPosition.x + 1; i >= currentPosition.x - 1; i--)
                    {
                        for(int j = currentPosition.y - 1; j <= currentPosition.y + 1; j++)
                        {
                            if(CheckThisPositionHasPiece(new Vector2Int(i, j))){
                                continue;
                            }
                            allPositions.Add(new Vector2Int(i, j));
                        }
                    }
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
            currentMovementPositions.Add(i);
            boardTransformMatrix[i.x, i.y].GetComponent<MeshRenderer>().material = greenMaterial;
        }
    }

    public void HighlightAttack(Piece piece) //hien cac co ma quan co "piece" co the an duoc quan doi phuong
    {
        currentAttackPositions.Clear();
        Vector2Int currentPosition = piece.position;
        List<Vector2Int> allPositions = new List<Vector2Int>();
        switch (piece.pieceType)
        {
            case PieceType.Pawn:
                {
                    if (piece.pieceColor == PieceColor.White)
                    {
                        if(CheckIfThisCellHasOpponentPiece(piece, currentPosition + new Vector2Int(1, -1)) )
                            allPositions.Add(currentPosition + new Vector2Int(1, -1));
                        if(CheckIfThisCellHasOpponentPiece(piece, currentPosition + new Vector2Int(1, 1)))
                            allPositions.Add(currentPosition + new Vector2Int(1, 1));
                    }
                    else
                    {
                        if(CheckIfThisCellHasOpponentPiece(piece, currentPosition + new Vector2Int(-1, -1)))
                            allPositions.Add(currentPosition + new Vector2Int(-1, -1));
                        if (CheckIfThisCellHasOpponentPiece(piece, currentPosition + new Vector2Int(-1, 1)))
                            allPositions.Add(currentPosition + new Vector2Int(-1, 1));
                    }
                    break;
                }
                case PieceType.Rook:
                    {
                        //Duyet len phia tren
                        for (int i = currentPosition.x + 1; i <= 8; i++)
                        {
                            if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, currentPosition.y)))
                            {
                                allPositions.Add(new Vector2Int(i, currentPosition.y));
                                break;
                            }
                            if(CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y)))
                            {
                                break;
                            }
                            continue;
                        }
                        //Duyet len phia duoi
                        for (int i = currentPosition.x - 1; i >= 1; i--)
                        {
                            if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, currentPosition.y)))
                            {
                                allPositions.Add(new Vector2Int(i, currentPosition.y));
                                break;
                            }
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y)))
                        {
                            break;
                        }
                        continue;
                        }
                        //Duyet sang ben trai
                        for (int i = currentPosition.y - 1; i >= 1; i--)
                        {
                            if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(currentPosition.x, i)))
                            {
                                allPositions.Add(new Vector2Int(currentPosition.x, i));
                                break;
                            }
                        if (CheckThisPositionHasPiece(new Vector2Int(currentPosition.x, i)))
                        {
                            break;
                        }
                        continue;
                        }
                        //Duyet sang ben phai
                        for (int i = currentPosition.y + 1; i <= 8; i++)
                        {
                            if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(currentPosition.x, i)))
                            {
                                allPositions.Add(new Vector2Int(currentPosition.x, i));
                                break;
                            }
                        if (CheckThisPositionHasPiece(new Vector2Int(currentPosition.x, i)))
                        {
                            break;
                        }
                        continue;
                        }
                        break;
                    }
                case PieceType.Knight:
                    {
                    if(CheckIfThisCellHasOpponentPiece(piece, currentPosition + new Vector2Int(-1, 2)))
                        allPositions.Add(currentPosition + new Vector2Int(-1, 2));
                    if (CheckIfThisCellHasOpponentPiece(piece, currentPosition + new Vector2Int(1, 2)))
                        allPositions.Add(currentPosition + new Vector2Int(1, 2));
                    if (CheckIfThisCellHasOpponentPiece(piece, currentPosition + new Vector2Int(-1, -2)))
                        allPositions.Add(currentPosition + new Vector2Int(-1, -2));
                    if (CheckIfThisCellHasOpponentPiece(piece, currentPosition + new Vector2Int(1, -2)))
                        allPositions.Add(currentPosition + new Vector2Int(1, -2));
                    if (CheckIfThisCellHasOpponentPiece(piece, currentPosition + new Vector2Int(-2, -1)))
                        allPositions.Add(currentPosition + new Vector2Int(-2, -1));
                    if (CheckIfThisCellHasOpponentPiece(piece, currentPosition + new Vector2Int(-2, 1)))
                        allPositions.Add(currentPosition + new Vector2Int(-2, 1));
                    if (CheckIfThisCellHasOpponentPiece(piece, currentPosition + new Vector2Int(2, -1)))
                        allPositions.Add(currentPosition + new Vector2Int(2, -1));
                    if (CheckIfThisCellHasOpponentPiece(piece, currentPosition + new Vector2Int(2, 1)))
                        allPositions.Add(currentPosition + new Vector2Int(2, 1));
                        break;
                    }
                case PieceType.Bishop:
                    {
                        //cheo tren trai
                        for (int i = currentPosition.x + 1; i <= 8; i++)
                        {
                            if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, currentPosition.y - (i - currentPosition.x))))
                            {
                            allPositions.Add(new Vector2Int(i, currentPosition.y - (i - currentPosition.x)));
                            break;
                            }
                            if(CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y - (i - currentPosition.x))))
                            {
                                break;
                            }
                        continue;
                        }
                        //cheo tren phai
                        for (int i = currentPosition.x + 1; i <= 8; i++)
                        {
                            if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, currentPosition.y + (i - currentPosition.x))))
                            {
                            allPositions.Add(new Vector2Int(i, currentPosition.y + (i - currentPosition.x)));
                            break;
                            }
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y + (i - currentPosition.x))))
                        {
                            break;
                        }
                        continue;
                        }
                        //cheo duoi trai
                        for (int i = currentPosition.x - 1; i >= 1; i--)
                        {
                            if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, currentPosition.y - (i - currentPosition.x))))
                            {
                            allPositions.Add(new Vector2Int(i, currentPosition.y - (i - currentPosition.x)));
                            break;
                            }
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y - (i - currentPosition.x))))
                        {
                            break;
                        }
                        continue;
                        }
                        //cheo duoi phai
                        for (int i = currentPosition.x - 1; i >= 1; i--)
                        {
                            if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, currentPosition.y + (i - currentPosition.x))))
                            {
                            allPositions.Add(new Vector2Int(i, currentPosition.y + (i - currentPosition.x)));
                            break;
                            }
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y + (i - currentPosition.x))))
                        {
                            break;
                        }
                        continue;
                        }
                        break;
                    }
                case PieceType.Queen:
                    {
                    //Duyet len phia tren
                    for (int i = currentPosition.x + 1; i <= 8; i++)
                    {
                        if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, currentPosition.y)))
                        {
                            allPositions.Add(new Vector2Int(i, currentPosition.y));
                            break;
                        }
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y)))
                        {
                            break;
                        }
                        continue;
                    }
                    //Duyet len phia duoi
                    for (int i = currentPosition.x - 1; i >= 1; i--)
                    {
                        if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, currentPosition.y)))
                        {
                            allPositions.Add(new Vector2Int(i, currentPosition.y));
                            break;
                        }
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y)))
                        {
                            break;
                        }
                        continue;
                    }
                    //Duyet sang ben trai
                    for (int i = currentPosition.y - 1; i >= 1; i--)
                    {
                        if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(currentPosition.x, i)))
                        {
                            allPositions.Add(new Vector2Int(currentPosition.x, i));
                            break;
                        }
                        if (CheckThisPositionHasPiece(new Vector2Int(currentPosition.x, i)))
                        {
                            break;
                        }
                        continue;
                    }
                    //Duyet sang ben phai
                    for (int i = currentPosition.y + 1; i <= 8; i++)
                    {
                        if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(currentPosition.x, i)))
                        {
                            allPositions.Add(new Vector2Int(currentPosition.x, i));
                            break;
                        }
                        if (CheckThisPositionHasPiece(new Vector2Int(currentPosition.x, i)))
                        {
                            break;
                        }
                        continue;
                    }
                    //cheo tren trai
                    for (int i = currentPosition.x + 1; i <= 8; i++)
                    {
                        if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, currentPosition.y - (i - currentPosition.x))))
                        {
                            allPositions.Add(new Vector2Int(i, currentPosition.y - (i - currentPosition.x)));
                            break;
                        }
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y - (i - currentPosition.x))))
                        {
                            break;
                        }
                        continue;
                    }
                    //cheo tren phai
                    for (int i = currentPosition.x + 1; i <= 8; i++)
                    {
                        if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, currentPosition.y + (i - currentPosition.x))))
                        {
                            allPositions.Add(new Vector2Int(i, currentPosition.y + (i - currentPosition.x)));
                            break;
                        }
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y + (i - currentPosition.x))))
                        {
                            break;
                        }
                        continue;
                    }
                    //cheo duoi trai
                    for (int i = currentPosition.x - 1; i >= 1; i--)
                    {
                        if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, currentPosition.y - (i - currentPosition.x))))
                        {
                            allPositions.Add(new Vector2Int(i, currentPosition.y - (i - currentPosition.x)));
                            break;
                        }
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y - (i - currentPosition.x))))
                        {
                            break;
                        }
                        continue;
                    }
                    //cheo duoi phai
                    for (int i = currentPosition.x - 1; i >= 1; i--)
                    {
                        if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, currentPosition.y + (i - currentPosition.x))))
                        {
                            allPositions.Add(new Vector2Int(i, currentPosition.y + (i - currentPosition.x)));
                            break;
                        }
                        if (CheckThisPositionHasPiece(new Vector2Int(i, currentPosition.y + (i - currentPosition.x))))
                        {
                            break;
                        }
                        continue;
                    }
                    break;
                    }
                case PieceType.King:
                    {
                        for (int i = currentPosition.x + 1; i >= currentPosition.x - 1; i--)
                        {
                            for (int j = currentPosition.y - 1; j <= currentPosition.y + 1; j++)
                            {
                                if (CheckIfThisCellHasOpponentPiece(piece, new Vector2Int(i, j)))
                                {
                                    allPositions.Add(new Vector2Int(i, j));
                                    continue;
                                }
                            }
                        }
                        break;
                    }
        }
        //Highlight
        foreach (var i in allPositions)
        {
            currentAttackPositions.Add(i);
            boardTransformMatrix[i.x, i.y].GetComponent<MeshRenderer>().material = redMaterial;
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
        //Tim vi tri da co trong list chua
        bool isFindTargetInCurrentList = false;
        foreach(var i in currentMovementPositions)
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
                    GameplayController.Instance.turnController.SwitchPlayer();
                }
        );
    }

    public void CurrentSelectedPieceAttackTo(Vector2Int targetPosition) //x, y 1-1 -> 8, 8
    {
        //Tim vi tri da co trong list chua
        bool isFindTargetInCurrentList = false;
        foreach (var i in currentAttackPositions)
        {
            if (targetPosition == i)
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
                    DeletePieceAtPosition(targetPosition);
                    currentSelectedPiece.position = targetPosition;
                    GameplayController.Instance.raycastController.isEnableSelect = true;
                    GameplayController.Instance.turnController.SwitchPlayer();
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
    public bool CheckThisPositionHasPiece(Vector2Int position)
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

    private bool CheckIfThisCellHasOpponentPiece(Piece piece, Vector2Int position)
    {
        foreach (var j in pieces)
        {
            if (j.position == position)
            {
                if(piece.pieceColor == j.pieceColor)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                break;
            }
        }
        return false;
    }

    private void DeletePieceAtPosition(Vector2Int position)
    {
        foreach (var j in pieces)
        {
            if (j.position == position)
            {
                if(j.pieceType == PieceType.King)
                {
                    Debug.LogError("King !");
                    if(GameplayController.Instance.turnController.currentPlayer == PieceColor.White)
                    {
                        //Trang thang
                        GameplayController.Instance.uiController.ShowGameEndPopup("White");
                    }
                    else
                    {
                        //Den thang
                        GameplayController.Instance.uiController.ShowGameEndPopup("Black");
                    }
                    return;
                }
                pieces.Remove(j);
                Destroy(j.gameObject);
                break;
            }
        }
    }
}
