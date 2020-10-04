using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleDataSlalom
{
    public int m_slalomBarriers = 4;

    [Range(0f, 0.5f)] public float m_extraRoadWidthPropMinDif = 0.25f;
    [Range(0f, 0.5f)] public float m_extraRoadWidthPropMaxDif = 0f;

    public int m_extraSlalomHeightPerBarrierMinDif = 0;
    public int m_extraSlalomHeightPerBarrierMaxDif = 2;

    //Asset references
    public GameObject m_slalomWallPrefab;
    public GameObject m_leftWallPrefab;
    public GameObject m_rightWallPrefab;
    public GameObject m_leftDiagonalWallPrefab;
    public GameObject m_rightDiagonalWallPrefab;

}
