using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChessObjectSettings",menuName = "Settings/ChessObjectSettings", order = 1)]
public class ChessObjectSettings : ScriptableObject
{
    public int Value;
    public bool CanMoveThrough;
    public int TileDistance;
    public Vector2[] PossibleMoves;
    public Vector2[] AttackOnlyMoves;
    
}
