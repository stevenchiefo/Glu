using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveData")]
public class WaveData : ScriptableObject
{
    public int Waves;
    public List<int> m_RunnerAmmount;
    public List<int> m_FlyerAmmount;
    
}
