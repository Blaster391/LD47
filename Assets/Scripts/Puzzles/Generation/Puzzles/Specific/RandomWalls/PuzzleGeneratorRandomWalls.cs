using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    public class PuzzleGeneratorRandomWalls : IPuzzleGenerator
    {
        public PuzzleGeneratorRandomWalls(PuzzleDataRandomWalls i_data)
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
            PuzzleData puzzleData = new PuzzleData(i_width, m_data.m_puzzleLength);

            // Place some walls (W) and removed the ability to place walls in places nearby that would block the player from getting through (x)
            // May need to go up with (x) as many cells as necessary to go sideways one block
            /*
             *    x   x
             *    x W x
             * 
             */

            bool[,] takenCells = new bool[i_width, m_data.m_puzzleLength];

            // Figure out how many blocks we're going to spawn
            int totalCells = i_width * m_data.m_puzzleLength;
            int blocksToSpawn = Mathf.FloorToInt(totalCells * Mathf.Lerp(m_data.m_wallCellPercentageAtMinDif, m_data.m_wallCellPercentageAtMaxDif, i_difficulty));
            int verticalCellsToBlock = Mathf.CeilToInt(i_forwardCellsPerSideways);

            // Super bad and simple stuff
            int attempts = 0;
            while(blocksToSpawn > 0 && attempts < blocksToSpawn * 10)
            {
                Vector2Int spawnCoords = new Vector2Int(Random.Range(0, i_width), Random.Range(0, m_data.m_puzzleLength));
                if(takenCells[spawnCoords.x, spawnCoords.y] == false)
                {
                    // Spawn our wall
                    PuzzleCell wallCell = puzzleData.GetCell(spawnCoords.x, spawnCoords.y);
                    wallCell.m_prefabToSpawn = m_data.m_wallPrefab;

                    // Block the cells around it
                    void BlockCell(Vector2Int i_coords)
                    {
                        if(i_coords.x >= 0 && i_coords.x < i_width)
                        {
                            if(i_coords.y >= 0 && i_coords.y < m_data.m_puzzleLength)
                            {
                                takenCells[i_coords.x, i_coords.y] = true;
                            }
                        }
                    }

                    BlockCell(spawnCoords + new Vector2Int(0, 0));
                    BlockCell(spawnCoords + new Vector2Int(-1, 0));
                    BlockCell(spawnCoords + new Vector2Int(1, 0));
                    for (int i = 1; i <= verticalCellsToBlock; ++i)
                    {
                        BlockCell(spawnCoords + new Vector2Int(0, -i));
                        BlockCell(spawnCoords + new Vector2Int(0, i));
                        BlockCell(spawnCoords + new Vector2Int(-1, -i));
                        BlockCell(spawnCoords + new Vector2Int(-1, i));
                        BlockCell(spawnCoords + new Vector2Int(1, -i));
                        BlockCell(spawnCoords + new Vector2Int(1, i));
                    }

                    --blocksToSpawn;
                }
                ++attempts;
            }


            puzzleData.AddRows(1);

            return puzzleData;
        }

        private PuzzleDataRandomWalls m_data;
    }
}
