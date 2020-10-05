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

            // Place the switchs
            List<PuzzleCell> switches = new List<PuzzleCell>();
            int switchCount = Mathf.CeilToInt(Mathf.Lerp(m_data.m_switchesPresentMinDif, m_data.m_switchesPresentMaxDif, i_difficulty));
            while(switches.Count < Mathf.Min(switchCount, i_width))
            {
                int switchX = Random.Range(0, i_width);
                PuzzleCell switchCell = puzzleData.GetCell(switchX, 0);
                if (switchCell.m_prefabToSpawn == null)
                {
                    switchCell.m_prefabToSpawn = m_data.m_switchPrefab;
                    switches.Add(switchCell);
                }
            }

            // Place the gates
            List<PuzzleCell> gates = new List<PuzzleCell>();
            List<int> gateXs = new List<int>();
            int gateCount = Mathf.CeilToInt(Mathf.Lerp(m_data.m_gatesPresentMinDif, m_data.m_gatesPresentMaxDif, i_difficulty));
            int gateY = puzzleCellLength - 1;
            while (gates.Count < Mathf.Min(gateCount, i_width))
            {
                int gateX = Random.Range(0, i_width);
                PuzzleCell gateCell = puzzleData.GetCell(gateX, gateY);
                if (gateCell.m_prefabToSpawn == null)
                {
                    gateCell.m_prefabToSpawn = m_data.m_gatePrefab;
                    gates.Add(gateCell);
                    gateXs.Add(gateX);
                }
            }

            // Fill with walls
            for(int x = 0; x < i_width; ++x)
            {
                PuzzleCell cell = puzzleData.GetCell(x, gateY);
                if (cell.m_prefabToSpawn == null)
                {
                    cell.m_prefabToSpawn = m_data.m_wallPrefab;
                }
            }

            // Link
            foreach (PuzzleCell switchCell in switches)
            {
                foreach (int gateX in gateXs)
                {
                    switchCell.m_cellsToLinkTo.Add(new Vector2Int(gateX, gateY));
                }
            }

            // Add empty cells at the end so the player has time to get back to their next thingy
            float halfWidth = (i_width - 1) / 2f;
            int gateOffsetFromCenter = Mathf.CeilToInt(halfWidth);
            int extraCellsNeeded = Mathf.CeilToInt(halfWidth) + gateOffsetFromCenter + 1;

            puzzleData.AddRows(extraCellsNeeded);

            return puzzleData;
        }

        private PuzzleDataSwitchDoor m_data;
    }
}
