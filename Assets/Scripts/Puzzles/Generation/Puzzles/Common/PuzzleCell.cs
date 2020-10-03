using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  One cell of the puzzle.
 */
namespace Puzzles
{
    public class PuzzleCell
    {
        public GameObject m_prefabToSpawn = null;
        public List<Vector2Int> m_cellsToLinkTo = new List<Vector2Int>();
    }
}