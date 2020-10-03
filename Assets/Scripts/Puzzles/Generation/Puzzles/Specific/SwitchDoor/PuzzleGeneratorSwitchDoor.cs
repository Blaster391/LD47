using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    public class PuzzleGeneratorSwitchDoor : IPuzzleGenerator
    {
        public PuzzleGeneratorSwitchDoor(PuzzleDataSwitchDoor i_data)
        {
            m_data = i_data;
        }

        public PuzzleData GeneratePuzzle
        (
            int i_width, 
            float i_difficulty,
            float i_forwardCellsPerSideways
        )
        {
            // Calculate how long we need it to be
            int maxGateOffset = i_width - 1;
            int cellsOfSpaceNeeded = Mathf.CeilToInt(maxGateOffset * i_forwardCellsPerSideways) /*+ 1*/; // You need one extra cell to get out of the switch IF it has walls of some sort you need to go through
            cellsOfSpaceNeeded += Mathf.FloorToInt((1f - i_difficulty) * m_data.m_puzzleCellLengthHandicapMinDifficulty) + Mathf.FloorToInt(i_difficulty * m_data.m_puzzleCellLengthHandicapMaxDifficulty);

            // Switch + space + gate
            int puzzleCellLength = 1 + cellsOfSpaceNeeded + 1;

            // Super simple to start
            PuzzleData puzzleData = new PuzzleData(i_width, puzzleCellLength);
            
            // Place the switch
            int switchX = Random.Range(0, i_width);
            PuzzleCell switchCell = puzzleData.GetCell(switchX, 0);
            switchCell.m_prefabToSpawn = m_data.m_switchPrefab;

            // Place the gate
            int gateX = Random.Range(0, i_width);

            int minDistance = Mathf.FloorToInt((float)i_width / 2f);
            while(Mathf.Abs(gateX - switchX) < minDistance)
            {
                gateX = Random.Range(0, i_width);
            }
            int gateY = puzzleCellLength - 1;
            PuzzleCell gateCell = puzzleData.GetCell(gateX, gateY);
            gateCell.m_prefabToSpawn = m_data.m_gatePrefab;

            // Fill with walls
            for(int x = 0; x < i_width; ++x)
            {
                if(x != gateX)
                {
                    PuzzleCell cell = puzzleData.GetCell(x, gateY);
                    cell.m_prefabToSpawn = m_data.m_wallPrefab;
                }
            }

            // Link
            switchCell.m_cellsToLinkTo.Add(new Vector2Int(gateX, gateY));

            return puzzleData;
        }

        private PuzzleDataSwitchDoor m_data;
    }
}
