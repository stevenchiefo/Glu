using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private int SizeX;
    [SerializeField] private int SizeY;
    [SerializeField] private string[] m_xLabels;
    [SerializeField] private string[] m_yLabels;
    [SerializeField] private GameObject m_TilePrefab;

    [Header("Colors")]
    [SerializeField] private Material m_FirstMat;

    [SerializeField] private Material m_SecondMat;
    private Tile[,] m_Grid;

    private void Start()
    {
        LoadGrid();
        GetComponent<SpawnManager>().StartSpawning(this);
    }

    public bool CanPlaceChessPiece(Tile _CurrentTile, Tile _targetTile)
    {
        Vector2[] _pointers = _CurrentTile.CurrentChessPiece.Settings.PossibleMoves;
        if (_CurrentTile.CurrentChessPiece.TeamColor == TeamColor.Black)
        {
            for (int i = 0; i < _pointers.Length; i++)
            {
                if (_CurrentTile.XPointer + -_pointers[i].x == _targetTile.XPointer && _CurrentTile.YPointer + -_pointers[i].y == _targetTile.YPointer)
                {
                    return true;
                }
            }
        }
        for (int i = 0; i < _pointers.Length; i++)
        {
            if (_CurrentTile.XPointer + _pointers[i].x == _targetTile.XPointer && _CurrentTile.YPointer + _pointers[i].y == _targetTile.YPointer)
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckIfCanMoveThrough(bool _canmove, Tile _currentTile, Tile _TargetTile)
    {
        if (_canmove == false)
            return false;

        return false;
    }

    public bool CanPlaceChessPieceAttack(Tile _CurrentTile, Tile _targetTile)
    {
        Vector2[] _pointers = _CurrentTile.CurrentChessPiece.Settings.AttackOnlyMoves;
        if (_CurrentTile.CurrentChessPiece.TeamColor == TeamColor.Black)
        {
            for (int i = 0; i < _pointers.Length; i++)
            {
                if (_CurrentTile.XPointer + -_pointers[i].x == _targetTile.XPointer && _CurrentTile.YPointer + -_pointers[i].y == _targetTile.YPointer)
                {
                    return true;
                }
            }
        }
        for (int i = 0; i < _pointers.Length; i++)
        {
            if (_CurrentTile.XPointer + _pointers[i].x == _targetTile.XPointer && _CurrentTile.YPointer + _pointers[i].y == _targetTile.YPointer)
            {
                return true;
            }
        }
        return false;
    }

    public Tile GetTile(string _Label)
    {
        for (int x = 0; x < m_Grid.GetLength(0); x++)
        {
            for (int y = 0; y < m_Grid.GetLength(1); y++)
            {
                if (m_Grid[x, y].Label == _Label)
                {
                    return m_Grid[x, y];
                }
            }
        }
        Debug.Log($"Failed to find Tile:{_Label}");
        return null;
    }

    private void LoadGrid()
    {
        m_Grid = new Tile[SizeX, SizeY];

        Material[] _mats = { m_FirstMat, m_SecondMat };
        int _matIndex = 0;

        Collider _col = m_TilePrefab.GetComponent<Collider>();
        float _Offsetx = _col.transform.localScale.x;
        float _Offsety = _col.transform.localScale.z;
        Vector3 offset = new Vector3(_Offsetx, 0f, _Offsety);
        for (int x = 0; x < SizeY; x++)
        {
            for (int y = 0; y < SizeX; y++)
            {
                Vector3 pos = new Vector3(x * offset.x, 0f, y * offset.z);
                GameObject _obj = Instantiate(m_TilePrefab, transform);
                Tile _tile = _obj.GetComponent<Tile>();
                string _label = m_xLabels[x] + m_yLabels[y];
                _tile.AssignTile(_label, pos, _mats[_matIndex]);
                _tile.XPointer = x;
                _tile.YPointer = y;

                m_Grid[x, y] = _tile;

                if (_matIndex == 1)
                    _matIndex = 0;
                else
                    _matIndex = 1;
            }
            if (_matIndex == 1)
                _matIndex = 0;
            else
                _matIndex = 1;
        }
    }
}