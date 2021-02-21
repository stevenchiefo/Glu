using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public ChessObject CurrentChessPiece { get; private set; }
    public Vector3 Postion { get => transform.position; }
    [SerializeField] private Vector3 m_Offset;

    public string Label;
    public int XPointer, YPointer;
    public TileMeshHolder m_MeshHolder;

    
    public bool CanPlace()
    {
        return CurrentChessPiece == null;
    }

    public void SetPiece(ChessObject chessObject)
    {
        if (CurrentChessPiece == null)
            CurrentChessPiece = chessObject;
        else
            Debug.LogError("Cant Place Chess Piece");
    }

    public void RemovePiece()
    {
        CurrentChessPiece = null;
    }

    public void AssignTile(string _label, Vector3 _Pos, Material _mat)
    {
        Label = _label;
        transform.position = _Pos;
        m_MeshHolder = new TileMeshHolder(GetComponent<MeshRenderer>());
        m_MeshHolder.ChangeMaterial(_mat);
    }

    public Vector3 GetPosition()
    {
        return transform.position + m_Offset;
    }

    private void OnMouseDown()
    {
        SelectionManager.Instance.SelectTile(this);
    }
}

public class TileMeshHolder
{
    public MeshRenderer Renderer { get; private set; }
    public TileMeshHolder(MeshRenderer _Renderer)
    {
        Renderer = _Renderer;
    }

    public void ChangeMaterial(Material _mat)
    {
        Renderer.material = _mat;
    }
}
