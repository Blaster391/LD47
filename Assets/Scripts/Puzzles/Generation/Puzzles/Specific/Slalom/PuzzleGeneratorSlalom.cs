using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    public class PuzzleGeneratorSlalom : IPuzzleGenerator
    {
        public PuzzleGeneratorSlalom(PuzzleDataSlalom i_data)
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
            // Slalomy thing
            // Pick how wide it's going to be
            // Calculate how many rows we need between each barrier
            // Calculate how many rows of extra diagonal barriers are needed
            /*
             *                    |       /|      |       /|
             *  |\      /|        |      / |      |\     / |
             *  | \    / |        |\    /  |      | |^--|  |
             *  |  |^-|  |        | |^-|   |      | |^^^|  |
             *  |  |^^|  |        | |^^|   |      | |^^^|  |
             *  |  |-^|  |        | |-^|   |      | |--^|  |
             *  |  |^^|  |        | |^^|   |      | |^^^|  |
             *  |  |^-|  |        | |^-|   |      | |^^^|  |
             *  |  |^^|  |        | |^^|   |      | |^--|  |
             *  |  |-^|  |        | |-^|   |      | |^^^|  |
             *  | /    \ |        |/    \  |      | |^^^|  |
             *  |/      \|        |      \ |      | |--^|  |
             *                    |       \|      |/     \ |
             *                                    |       \|
             */

            // Calculate how long we need it to be

            // First we have to pick where we're going to place this
            int slalomWidth = 2 + Mathf.Max(Mathf.CeilToInt(Mathf.Lerp(m_data.m_extraRoadWidthPropMinDif, m_data.m_extraRoadWidthPropMaxDif, i_difficulty) * (i_width - 2)), 0);
            int slalomLeftLaneX = Random.Range(0, i_width - slalomWidth);

            // Figure out how much extra height we need for the hard shoulders
            int slalomLeftShoulderWidth = slalomLeftLaneX;
            int slalomRightShoulderWidth = i_width - (slalomLeftLaneX + slalomWidth);

            int maxShoulderWidth = Mathf.Max(slalomLeftShoulderWidth, slalomRightShoulderWidth);
            int shoulderDiagonalMaxHeight = maxShoulderWidth;
            int shoulderAdditionalHeight = shoulderDiagonalMaxHeight * 2; // Top and bottom

            // Now figure out how much height we need for the slalom
            int slalomVerticalSpacePerBarrier = Mathf.CeilToInt((slalomWidth - 1) * i_forwardCellsPerSideways) + Mathf.CeilToInt(Mathf.Lerp(m_data.m_extraSlalomHeightPerBarrierMinDif, m_data.m_extraSlalomHeightPerBarrierMaxDif, i_difficulty));
            int slalomHeight = m_data.m_slalomBarriers + (m_data.m_slalomBarriers - 1) * slalomVerticalSpacePerBarrier;

            // Total height
            int puzzleCellHeight = slalomHeight + shoulderAdditionalHeight;

            // Super simple to start
            PuzzleData puzzleData = new PuzzleData(i_width, puzzleCellHeight);


            // Start placing
            Vector2Int slalomBottomLeft = new Vector2Int(slalomLeftShoulderWidth, shoulderDiagonalMaxHeight);
            Vector2Int slalomBottomRight = slalomBottomLeft + new Vector2Int(slalomWidth - 1, 0);
            Vector2Int slalomTopLeft = slalomBottomLeft + new Vector2Int(0, slalomHeight - 1);
            Vector2Int slalomTopRight = slalomBottomRight + new Vector2Int(0, slalomHeight - 1);
            
            void PlaceWall(Vector2Int i_pos, GameObject i_prefab)
            {
                if(i_pos.x >= 0 && i_pos.x < puzzleData.Width && i_pos.y >= 0 && i_pos.y < puzzleData.Height)
                {
                    PuzzleCell wallCell = puzzleData.GetCell(i_pos.x, i_pos.y);
                    wallCell.m_prefabToSpawn = i_prefab;
                }
            }

            // Slalom
            bool isLeft = (Mathf.RoundToInt(Random.Range(0f, 1f)) == 1);
            for(int y = 0; y < slalomHeight; ++y)
            {
                PlaceWall(slalomBottomLeft + new Vector2Int(-1, y), m_data.m_leftWallPrefab);
                PlaceWall(slalomBottomRight + new Vector2Int(1, y), m_data.m_rightWallPrefab);

                if(y == 0 || (y % (slalomVerticalSpacePerBarrier + 1) == 0))
                {
                    // Place a wall
                    for(int x = 0; x < slalomWidth - 1; ++x)
                    {
                        if (isLeft)
                        {
                            PlaceWall(slalomBottomLeft + new Vector2Int(x, y), m_data.m_slalomWallPrefab);
                        }
                        else
                        {
                            PlaceWall(slalomBottomRight + new Vector2Int(-x, y), m_data.m_slalomWallPrefab);
                        }
                    }
                    isLeft = !isLeft;
                }
            }

            // Hard shoulder
            for (int y = 0; y < shoulderDiagonalMaxHeight; ++y)
            {
                // Bottom ones
                PlaceWall(slalomBottomLeft + new Vector2Int(-1, -1) + new Vector2Int(-y, -y), m_data.m_leftDiagonalWallPrefab);
                PlaceWall(slalomBottomRight + new Vector2Int(1, -1) + new Vector2Int(y, -y), m_data.m_rightDiagonalWallPrefab);

                // Top ones
                PlaceWall(slalomTopLeft + new Vector2Int(-1, 1) + new Vector2Int(-y, y), m_data.m_rightDiagonalWallPrefab);
                PlaceWall(slalomTopRight + new Vector2Int(1, 1) + new Vector2Int(y, y), m_data.m_leftDiagonalWallPrefab);
            }

            puzzleData.AddRows(2);

            return puzzleData;
        }

        private PuzzleDataSlalom m_data;
    }
}
