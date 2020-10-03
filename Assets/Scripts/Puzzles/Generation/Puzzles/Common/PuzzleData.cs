using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 *  The data of a generated puzzle.
 *  Used to instantiate the puzzle on the mobius strip
 */
namespace Puzzles
{
    public class PuzzleData
    {
        public PuzzleData(int i_width, int i_height)
        {
            m_puzzleCells = new PuzzleCell[i_width, i_height];
        }

        public int Width { get { return m_puzzleCells.GetLength(0); } }
        public int Height { get { return m_puzzleCells.GetLength(1); } }

        public PuzzleCell GetCell(int i_x, int i_y)
        {
            if(m_puzzleCells[i_x, i_y] == null)
            {
                m_puzzleCells[i_x, i_y] = new PuzzleCell();
            }
            return m_puzzleCells[i_x, i_y];
        }

        private PuzzleCell[,] m_puzzleCells;
    }
}
