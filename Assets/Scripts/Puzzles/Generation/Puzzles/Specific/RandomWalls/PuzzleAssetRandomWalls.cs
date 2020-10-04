using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    [CreateAssetMenu(fileName = "RandomWallsPuzzle", menuName = "ScriptableObjects/Puzzles/RandomWalls", order = 1)]
    public class PuzzleAssetRandomWalls : PuzzleAsset
    {
        // Values for tweaking this specific puzzle
        public PuzzleDataRandomWalls m_data;

        public override IPuzzleGenerator Generator { get { return new PuzzleGeneratorRandomWalls(m_data); } }
    }
}
