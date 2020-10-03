using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    [CreateAssetMenu(fileName = "SwitchDoorPuzzle", menuName = "ScriptableObjects/Puzzles/SwitchDoor", order = 1)]
    public class PuzzleAssetSwitchDoor : PuzzleAsset
    {
        // Values for tweaking this specific puzzle
        public PuzzleDataSwitchDoor m_data;

        public override IPuzzleGenerator Generator { get { return new PuzzleGeneratorSwitchDoor(m_data); } }
    }
}
