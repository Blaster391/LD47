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
            for(int x = 0; x < i_width; ++x)
            {
                m_puzzleCells.Add(new List<PuzzleCell>());

                for(int y = 0; y < i_height; ++y)
                {
                    m_puzzleCells[x].Add(new PuzzleCell());
                }
            }
        }

        public int Width { get { return m_puzzleCells.Count; } }
        public int Height { get { return m_puzzleCells[0].Count; } }

        public PuzzleCell GetCell(int i_x, int i_y)
        {
            return m_puzzleCells[i_x][i_y];
        }
        public void AddRow()
        {
            for (int x = 0; x < Width; ++x)
            {
                m_puzzleCells[x].Add(new PuzzleCell());
            }
        }
        public void AddRows(int i_rows)
        {
            for (int i = 0; i < i_rows; ++i)
            {
                AddRow();
            }
        }

        private List<List<PuzzleCell>> m_puzzleCells = new List<List<PuzzleCell>>();
    }
}
