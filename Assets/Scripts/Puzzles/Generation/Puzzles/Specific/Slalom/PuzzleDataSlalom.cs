using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleDataSlalom
{
    public int m_slalomBarriers = 4;

    [Range(0f, 0.5f)] public float m_extraRoadWidthPropMinDif = 0.25f;
    [Range(0f, 0.5f)] public float m_extraRoadWidthPropMaxDif = 0f;
    
    //Asset references
    public GameObject m_wallPrefab;
    public GameObject m_leftDiagonalWallPrefab;
    public GameObject m_rightDiagonalWallPrefab;

}
