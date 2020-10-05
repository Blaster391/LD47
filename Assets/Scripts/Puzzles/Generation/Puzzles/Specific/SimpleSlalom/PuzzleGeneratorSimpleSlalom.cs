using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    public class PuzzleGeneratorSimpleSlalom : IPuzzleGenerator
    {
        public PuzzleGeneratorSimpleSlalom(PuzzleDataSimpleSlalom i_data)
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
            // SimpleSlalomy thing
            // Pick how wide it's going to be
            // Calculate how many rows we need between each barrier
            // Calculate how many rows of extra diagonal barriers are needed
            /*
             *  |  |^-|  |
             *  |  |^^|  |
             *  |  |-^|  |
             *  |  |^^|  |
             *  |  |^-|  |
             *  |  |^^|  |
             *  |---- ---|
             */

            int openingGapWidth = 1 + Mathf.CeilToInt(Mathf.Lerp(m_data.m_extraEntryWidthMinDif, m_data.m_extraEntryWidthMaxDif, i_difficulty));

            // Now figure out how much height we need for the SimpleSlalom
            int slalomVerticalSpacePerBarrier = Mathf.CeilToInt((i_width - openingGapWidth) * i_forwardCellsPerSideways) + Mathf.CeilToInt(Mathf.Lerp(m_data.m_extraHeightPerBarrierMinDif, m_data.m_extraHeightPerBarrierMaxDif, i_difficulty));
            int slalomHeight = m_data.m_slalomBarriers + (m_data.m_slalomBarriers - 1) * slalomVerticalSpacePerBarrier;

            // Super simple to start
            PuzzleData puzzleData = new PuzzleData(i_width, slalomHeight);

            // Start placing
            
            void PlaceWall(Vector2Int i_pos)
            {
                PuzzleCell wallCell = puzzleData.GetCell(i_pos.x, i_pos.y);
                wallCell.m_prefabToSpawn = m_data.m_wallPrefab;
            }

            // SimpleSlalom
            bool isLeft = (Random.Range(0f, 1f) > 0.5f) ? true : false;
            for(int barrierIndex = 0; barrierIndex < m_data.m_slalomBarriers; ++barrierIndex)
            {
                int gapStartIndex = (isLeft ? 0 : i_width - openingGapWidth);
                for(int x = 0; x < i_width; ++x)
                {
                    if(x >= gapStartIndex && x < gapStartIndex + openingGapWidth)
                    {
                        // Gap
                    }
                    else
                    {
                        PlaceWall(new Vector2Int(x, barrierIndex * (slalomVerticalSpacePerBarrier + 1))); // Add on 1 for the barrier row itself
                    }
                }
                isLeft = !isLeft;
            }

            puzzleData.AddRows(slalomVerticalSpacePerBarrier);

            return puzzleData;
        }

        private PuzzleDataSimpleSlalom m_data;
    }
}
