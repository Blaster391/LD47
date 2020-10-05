using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    [CreateAssetMenu(fileName = "SimpleSlalomPuzzle", menuName = "ScriptableObjects/Puzzles/SimpleSlalom", order = 1)]
    public class PuzzleAssetSimpleSlalom : PuzzleAsset
    {
        // Values for tweaking this specific puzzle
        public PuzzleDataSimpleSlalom m_data;

        public override IPuzzleGenerator Generator { get { return new PuzzleGeneratorSimpleSlalom(m_data); } }
    }
}
