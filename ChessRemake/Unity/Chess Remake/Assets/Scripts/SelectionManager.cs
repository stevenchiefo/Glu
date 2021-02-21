using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

    [SerializeField] private TeamColor CurrentTurn;


    [SerializeField] private Tile m_FirstSelectedTile;
    [SerializeField] private Tile m_SecondSelectedTile;
    private Board m_Board;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        CurrentTurn = TeamColor.White;
        m_Board = GetComponent<Board>();
    }

    public void SelectTile(Tile _tile)
    {
        if (m_FirstSelectedTile == null)
        {
            if (_tile.CurrentChessPiece != null)
            {
                if (_tile.CurrentChessPiece.TeamColor == CurrentTurn)
                    m_FirstSelectedTile = _tile;
            }


        }
        else if (m_SecondSelectedTile == null)
        {
            if (m_Board.CanPlaceChessPiece(m_FirstSelectedTile, _tile) && _tile.CurrentChessPiece == null)
            {
                m_SecondSelectedTile = _tile;
                MoveToTile(m_FirstSelectedTile, m_SecondSelectedTile);

                m_FirstSelectedTile.RemovePiece();

                m_FirstSelectedTile = null;
                m_SecondSelectedTile = null;

                if (CurrentTurn == TeamColor.White)
                    CurrentTurn = TeamColor.Black;
                else
                    CurrentTurn = TeamColor.White;

            }
            else if (m_Board.CanPlaceChessPieceAttack(m_FirstSelectedTile, _tile))
            {
                if (_tile.CurrentChessPiece != null)
                {
                    if (_tile.CurrentChessPiece.TeamColor != CurrentTurn)
                    {


                        m_SecondSelectedTile = _tile;
                        MoveToTile(m_FirstSelectedTile, m_SecondSelectedTile);
                        m_FirstSelectedTile.RemovePiece();
                        Destroy(_tile.CurrentChessPiece.gameObject);
                        m_SecondSelectedTile = null;
                        m_FirstSelectedTile = null;
                        if (CurrentTurn == TeamColor.White)
                            CurrentTurn = TeamColor.Black;
                        else
                            CurrentTurn = TeamColor.White;
                    }
                }



            }
            else if (m_FirstSelectedTile.Postion != _tile.Postion)
            {
                if (_tile.CurrentChessPiece != null)
                {
                    if (m_FirstSelectedTile.CurrentChessPiece.TeamColor == _tile.CurrentChessPiece.TeamColor)
                    {
                        m_FirstSelectedTile = _tile;
                    }

                }
            }

        }


    }

    private void MoveToTile(Tile _startTile, Tile _targetTile)
    {
        ChessObject _selectedObject = _startTile.CurrentChessPiece;
        _selectedObject.MoveToTile(_targetTile);

    }
}
