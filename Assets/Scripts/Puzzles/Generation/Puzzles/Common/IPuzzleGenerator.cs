using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    public interface IPuzzleGenerator
    {
        PuzzleData GeneratePuzzle(int i_width, float i_difficulty, float i_forwardCellsPerSideways);
    }
}
