using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleDataSimpleSlalom
{
    public int m_slalomBarriers = 4;

    public int m_extraEntryWidthMinDif = 2;
    public int m_extraEntryWidthMaxDif = 0;

    public int m_extraHeightPerBarrierMinDif = 0;
    public int m_extraHeightPerBarrierMaxDif = 2;

    //Asset references
    public GameObject m_wallPrefab;
}
