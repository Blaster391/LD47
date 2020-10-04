using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    [CreateAssetMenu(fileName = "SlalomPuzzle", menuName = "ScriptableObjects/Puzzles/Slalom", order = 1)]
    public class PuzzleAssetSlalom : PuzzleAsset
    {
        // Values for tweaking this specific puzzle
        public PuzzleDataSlalom m_data;

        public override IPuzzleGenerator Generator { get { return new PuzzleGeneratorSlalom(m_data); } }
    }
}
