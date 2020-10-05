using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    [CreateAssetMenu(fileName = "TrafficJamPuzzle", menuName = "ScriptableObjects/Puzzles/TrafficJam", order = 1)]
    public class PuzzleAssetTrafficJam : PuzzleAsset
    {
        // Values for tweaking this specific puzzle
        public PuzzleDataTrafficJam m_data;

        public override IPuzzleGenerator Generator { get { return new PuzzleGeneratorTrafficJam(m_data); } }
    }
}
