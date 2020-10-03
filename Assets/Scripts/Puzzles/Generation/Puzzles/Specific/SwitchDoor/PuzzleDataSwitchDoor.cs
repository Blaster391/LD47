using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleDataSwitchDoor
{
    public int m_puzzleCellLengthHandicapMinDifficulty = 3;
    public int m_puzzleCellLengthHandicapMaxDifficulty = 0;

    //Asset references
    public GameObject m_switchPrefab;
    public GameObject m_gatePrefab;
    public GameObject m_wallPrefab;
}
