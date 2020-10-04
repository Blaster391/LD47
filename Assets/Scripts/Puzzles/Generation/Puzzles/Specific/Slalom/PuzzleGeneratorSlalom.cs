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


            // Super simple to start
            PuzzleData puzzleData = new PuzzleData(i_width, 1);// puzzleCellLength);
            
            // Place the switch
            //int switchX = Random.Range(0, i_width);
            //PuzzleCell switchCell = puzzleData.GetCell(switchX, 0);
            //switchCell.m_prefabToSpawn = m_data.m_switchPrefab;

            //// Place the gate
            //int gateX = Random.Range(0, i_width);

            //int minDistance = Mathf.FloorToInt((float)i_width / 2f);
            //while(Mathf.Abs(gateX - switchX) < minDistance)
            //{
            //    gateX = Random.Range(0, i_width);
            //}
            //int gateY = puzzleCellLength - 1;
            //PuzzleCell gateCell = puzzleData.GetCell(gateX, gateY);
            //gateCell.m_prefabToSpawn = m_data.m_gatePrefab;

            //// Fill with walls
            //for(int x = 0; x < i_width; ++x)
            //{
            //    if(x != gateX)
            //    {
            //        PuzzleCell cell = puzzleData.GetCell(x, gateY);
            //        cell.m_prefabToSpawn = m_data.m_wallPrefab;
            //    }
            //}

            //// Link
            //switchCell.m_cellsToLinkTo.Add(new Vector2Int(gateX, gateY));

            //// Add empty cells at the end so the player has time to get back to their next thingy
            //float halfWidth = (i_width - 1) / 2f;
            //int gateOffsetFromCenter = Mathf.CeilToInt(Mathf.Abs(gateX - halfWidth));
            //int extraCellsNeeded = Mathf.CeilToInt(halfWidth) + gateOffsetFromCenter + 1;

            //puzzleData.AddRows(extraCellsNeeded);

            return puzzleData;
        }

        private PuzzleDataSlalom m_data;
    }
}
