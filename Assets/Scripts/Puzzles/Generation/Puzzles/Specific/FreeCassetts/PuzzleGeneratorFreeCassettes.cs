using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    public class PuzzleGeneratorFreeCassettes : IPuzzleGenerator
    {
        public PuzzleGeneratorFreeCassettes(PuzzleDataFreeCassettes i_data)
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
            int length = Mathf.CeilToInt(Mathf.Lerp(m_data.m_lengthAtMinDif, m_data.m_lengthAtMaxDif, i_difficulty));

            PuzzleData puzzleData = new PuzzleData(i_width, length);

            int currentX = Random.Range(1, i_width - 1);
            int direction = Random.Range(0f, 1f) > 0.5f ? 1 : -1;

            for(int y = 0; y < length; ++y)
            {
                puzzleData.GetCell(currentX, y).m_prefabToSpawn = m_data.m_cassettePrefab;
                if(currentX == 0 || currentX == i_width - 1)
                {
                    direction = -direction;
                }
                currentX += direction;
            }

            puzzleData.AddRows(2);

            return puzzleData;
        }

        private PuzzleDataFreeCassettes m_data;
    }
}
