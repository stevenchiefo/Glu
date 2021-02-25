using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Material m_TeamWhiteMat;
    [SerializeField] private Material m_TeamBlackMat;

    [SerializeField] private GameObject m_KingPrefab;
    [SerializeField] private GameObject m_QueenPrefab;
    [SerializeField] private GameObject m_PriestPrefab;
    [SerializeField] private GameObject m_KnightPrefab;
    [SerializeField] private GameObject m_TowerPrefab;
    [SerializeField] private GameObject m_PionPrefab;

    [SerializeField] private SpawnSettings m_TeamWhiteSettings;
    [SerializeField] private SpawnSettings m_TeamBlackSettings;
    
    private Board m_Board;



    public void StartSpawning(Board board)
    {
        m_Board = board;
        SpawnPieces(m_PionPrefab, m_TeamWhiteSettings.PionLabels, TeamColor.White);
        SpawnPieces(m_PionPrefab, m_TeamBlackSettings.PionLabels, TeamColor.Black);
        SpawnPieces(m_TowerPrefab, m_TeamWhiteSettings.TowerLabels, TeamColor.White);
        SpawnPieces(m_TowerPrefab, m_TeamBlackSettings.TowerLabels, TeamColor.Black);
        SpawnPieces(m_KnightPrefab, m_TeamWhiteSettings.KnightLabels, TeamColor.White);
        SpawnPieces(m_KnightPrefab, m_TeamBlackSettings.KnightLabels, TeamColor.Black);
    }

    private void SpawnPieces(GameObject prefab, string[] _tileLabes,TeamColor team)
    {
        for (int i = 0; i < _tileLabes.Length; i++)
        {
            ChessObject chessObject = Instantiate(prefab).GetComponent<ChessObject>();
            Tile _tile = m_Board.GetTile(_tileLabes[i]);
            _tile.SetPiece(chessObject);
            chessObject.transform.position = _tile.GetPosition();
            if (team == TeamColor.White)
                chessObject.SetTeam(team, m_TeamWhiteMat);
            else
                chessObject.SetTeam(team, m_TeamBlackMat);
        }
    }


}
