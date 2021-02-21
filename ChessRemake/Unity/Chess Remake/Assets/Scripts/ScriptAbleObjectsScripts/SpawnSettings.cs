using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnSettings",menuName = "Settings/SpawnSettings",order = 2)]
public class SpawnSettings : ScriptableObject
{
    public string[] KingLabels;
    public string[] QueenLabels;
    public string[] PriestLabels;
    public string[] KnightLabels;
    public string[] TowerLabels;
    public string[] PionLabels;
}
