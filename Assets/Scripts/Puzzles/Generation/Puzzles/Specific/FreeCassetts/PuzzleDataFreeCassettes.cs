using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PuzzleDataFreeCassettes
{
    public int m_lengthAtMinDif = 6;
    public int m_lengthAtMaxDif = 12;

    public float m_cassettesPropAtMinDif = 0.1f;
    public float m_cassettesPropAtMaxDif = 0.4f;

    //Asset references
    public GameObject m_cassettePrefab;
}
