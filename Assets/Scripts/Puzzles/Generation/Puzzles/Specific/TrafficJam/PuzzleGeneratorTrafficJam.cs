using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    public class PuzzleGeneratorTrafficJam : IPuzzleGenerator
    {
        public PuzzleGeneratorTrafficJam(PuzzleDataTrafficJam i_data)
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
            PuzzleData puzzleData = new PuzzleData(i_width, m_data.m_jamLength);

            HashSet<int> fullLanes = new HashSet<int>();

            // Figure out how many lanes to fill up
            int minLanes = m_data.m_lanesOfTrafficAtMinDif;
            int maxLanes = i_width - m_data.m_lanesWithoutTrafficAtMaxDif;
            int numLanes = Mathf.FloorToInt(Mathf.Lerp(minLanes, maxLanes, i_difficulty)); // Num lanes of traffic

            // Pick our lanes
            while(fullLanes.Count < numLanes)
            {
                fullLanes.Add(Random.Range(0, i_width));
            }

            // Spawn our cars
            foreach(int laneIndex in fullLanes)
            {
                for(int y = 0; y < m_data.m_jamLength; ++y)
                {
                    PuzzleCell carCell = puzzleData.GetCell(laneIndex, y);
                    carCell.m_prefabToSpawn = m_data.m_carPrefabs[Random.Range(0, m_data.m_carPrefabs.Count)];
                }
            }

            int extraCellsPostJam = Mathf.CeilToInt(2 * i_forwardCellsPerSideways); // Bit of padding on the exit
            puzzleData.AddRows(extraCellsPostJam);

            return puzzleData;
        }

        private PuzzleDataTrafficJam m_data;
    }
}
