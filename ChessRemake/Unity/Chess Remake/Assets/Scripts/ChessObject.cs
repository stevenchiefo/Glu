using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TeamColor
{
    Black,
    White,
}
public class ChessObject : MonoBehaviour
{
    [Header("Settings")]
    public ChessObjectSettings Settings;

    private MeshRenderer m_MeshRenderer;

    public TeamColor TeamColor { get; private set; }



    public void SetTeam(TeamColor _color, Material material)
    {
        TeamColor = _color;
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer item in meshRenderers)
        {
            item.material = material;
        }
    }

    public void MoveToTile(Tile _targetTile)
    {
        StartCoroutine(Move(_targetTile));
    }

    private IEnumerator Move(Tile _targetTile)
    {
        float distance = Vector3.Distance(_targetTile.transform.position, transform.position);
        while (distance > DataManager.Instance.MovementSettings.LockRange)
        {
            distance = Vector3.Distance(_targetTile.transform.position, transform.position);
            transform.position = Vector3.Lerp(transform.position, _targetTile.GetPosition(), DataManager.Instance.MovementSettings.GeneralSpeed * Time.deltaTime);
            yield return null;
            
        }
        transform.position = _targetTile.GetPosition();
        _targetTile.SetPiece(this);
    }

}
