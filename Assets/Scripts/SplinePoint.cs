using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplinePoint : MonoBehaviour
{
    [SerializeField] private bool m_suitableForPuzzles = true;

    public bool SupportsPuzzles { get { return m_suitableForPuzzles; } }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = SupportsPuzzles ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, 1);
    }
}
