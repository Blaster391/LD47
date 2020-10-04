using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleDataRandomWalls
{
    public int m_puzzleLength = 8;
    public float m_wallCellPercentageAtMinDif = 0.05f;
    public float m_wallCellPercentageAtMaxDif = 0.15f;

    //Asset references
    public GameObject m_wallPrefab;
}
