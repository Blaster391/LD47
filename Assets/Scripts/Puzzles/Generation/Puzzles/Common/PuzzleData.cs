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


        public void FillRandomClutter(List<GameObject> _randomObjects, float _minClutterProp, float _maxClutterProp)
        {
            if(_randomObjects.Count == 0)
            {
                return;
            }

            List<PuzzleCell> potentialCell = new List<PuzzleCell>();
            for (int x = 0; x < Width; ++x)
            {
                for (int y = 0; y < Height; ++y)
                {
                    if(m_puzzleCells[x][y].m_prefabToSpawn == null)
                    {
                        potentialCell.Add(m_puzzleCells[x][y]);
                    }
                }
            }

            Shuffle(potentialCell);


            int randomClutterAmount = Mathf.FloorToInt(Random.Range(_minClutterProp, _maxClutterProp) * potentialCell.Count);
            randomClutterAmount = Mathf.Min(randomClutterAmount, potentialCell.Count);

            for (int i = 0; i < randomClutterAmount; ++i)
            {
                int randomObject = Random.Range(0, _randomObjects.Count);
                potentialCell[i].m_prefabToSpawn = _randomObjects[randomObject];
                potentialCell[i].m_prefabRotation = Quaternion.AngleAxis(Random.value * 360, new Vector3(0, 1, 0));
            }
        }

        public void Shuffle<T>(IList<T> ts)
        {
            var count = ts.Count;
            var last = count - 1;
            for (var i = 0; i < last; ++i)
            {
                var r = UnityEngine.Random.Range(i, count);
                var tmp = ts[i];
                ts[i] = ts[r];
                ts[r] = tmp;
            }
        }
    }
}
