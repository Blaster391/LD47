using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleDataTrafficJam
{
    public int m_jamLength = 6;

    public int m_lanesOfTrafficAtMinDif = 1;
    public int m_lanesWithoutTrafficAtMaxDif = 1;

    //Asset references
    public List<GameObject> m_carPrefabs;
}
