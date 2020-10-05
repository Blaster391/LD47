using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    [CreateAssetMenu(fileName = "FreeCassettesPuzzle", menuName = "ScriptableObjects/Puzzles/FreeCassettes", order = 1)]
    public class PuzzleAssetFreeCassettes : PuzzleAsset
    {
        // Values for tweaking this specific puzzle
        public PuzzleDataFreeCassettes m_data;

        public override IPuzzleGenerator Generator { get { return new PuzzleGeneratorFreeCassettes(m_data); } }
    }
}
