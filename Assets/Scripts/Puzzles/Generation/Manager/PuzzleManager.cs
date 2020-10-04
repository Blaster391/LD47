using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzles
{
    /*
     *  The famed Puzzle Manager, what mysteries doth thine hold.
     *  
     *  
     */
    public class PuzzleManager : MonoBehaviour
    {
        // Editable
        [SerializeField] private Spline m_spline;
        [SerializeField] private SurfaceClampedCarController m_car;

        public List<PuzzleAsset> m_puzzlesToSpawn;

        // Currently assuming car is 1, cells 1.5x that.
        public float m_cellSize = 1.5f;
        public int m_trackWidthInCells = 5;

        public float m_startingDifficulty = 0f;
        public float m_difficultyIncreasePerCell = 0.02f; // 50 cells to max difficulty

        public int m_initialSpaceCellSize = 10;

        // Internal
        private Dictionary<int, GameObject[,]> m_spawnedObjects = new Dictionary<int, GameObject[,]>();

        // State
        private float m_runningDifficulty = 0f;
        private int m_runningDistancePS = 0;

        private float m_trackLengthWS = 0f;
        private float m_trackLengthPS = 0; // Puzzle Space boiiii
        private float m_trackTPerCell = 0;

        // Updated values
        private float m_forwardCellsPerSideways = 1f; // Currently not actually updated as expected. Can just expose and tweak tbh

        void Start()
        {
            if(m_spline == null)
            {
                Debug.LogError("No Spline Referenced in Puzzle Manager Script");
            }

            // Cache some values
            m_trackLengthWS = m_spline.Length;
            m_trackLengthPS = m_trackLengthWS / m_cellSize;
            m_trackTPerCell = m_cellSize / m_spline.Length;

            // For now lets just try and spawn one puzzle
            Generate();
        }

        void Update()
        {

        }

        private void Generate()
        {
            m_runningDifficulty = m_startingDifficulty;

            int puzzleIndex = 0;

            while (puzzleIndex < 3)
            {
                PuzzleAsset selectedPuzzle = SelectPuzzle(m_runningDifficulty);
                if (selectedPuzzle == null)
                {
                    break;
                }

                PuzzleData puzzleData = GeneratePuzzle(puzzleIndex, selectedPuzzle.Generator, (m_runningDistancePS / m_trackLengthPS) % 1f);

                m_runningDistancePS += puzzleData.Height;
                m_runningDifficulty = Mathf.Min(m_runningDifficulty + m_difficultyIncreasePerCell * puzzleData.Height, 1f);

                ++puzzleIndex;
            }
        }

        private PuzzleAsset SelectPuzzle(float i_difficulty)
        {
            List<PuzzleAsset> possiblePuzzles = new List<PuzzleAsset>();
            foreach (PuzzleAsset availablePuzzle in m_puzzlesToSpawn)
            {
                if (i_difficulty >= availablePuzzle.DifficultyMin && i_difficulty <= availablePuzzle.DifficultyMax)
                {
                    possiblePuzzles.Add(availablePuzzle);
                }
            }

            if (possiblePuzzles.Count == 0)
            {
                return null;
            }

            return possiblePuzzles[Random.Range(0, possiblePuzzles.Count)];
        }

        private PuzzleData GeneratePuzzle(int i_puzzleIndex, IPuzzleGenerator i_puzzleGeneratorIF, float i_startingSplineT)
        {
            PuzzleData puzzleData = i_puzzleGeneratorIF.GeneratePuzzle(m_trackWidthInCells, m_runningDifficulty, m_forwardCellsPerSideways);

            Vector2Int gridSize = new Vector2Int(puzzleData.Width, puzzleData.Height);

            GameObject[,] spawnedObjects = new GameObject[gridSize.x, gridSize.y];
            m_spawnedObjects.Add(i_puzzleIndex, spawnedObjects);

            float GetT(int i_y)
            {
                return i_startingSplineT + i_y * m_trackTPerCell;
            }

            // Lets figure out wtf we need to spawn this crap
            for (int y = 0; y < puzzleData.Height; ++y)
            {
                // Every time we go up one we need to find our new stuff
                Vector3 splinePos, splineFwd;
                int segementIndex = 0;
                float splineT = GetT(y);
                m_spline.GetSplineTransform(segementIndex, splineT, out splineFwd, out splinePos);

                for (int x = 0; x < puzzleData.Width; ++x)
                {
                    PuzzleCell cell = puzzleData.GetCell(x, y);
                    if (cell.m_prefabToSpawn != null)
                    {

                        // So we have the center of the spline

                        //spawnedObjects[x, y] = Instantiate(cell.m_prefabToSpawn, i_startingPosition + new Vector3(x * m_cellSize, 0, y * m_cellSize), Quaternion.identity, transform);
                    }
                }
            }

            // Link
            for (int x = 0; x < puzzleData.Width; ++x)
            {
                for (int y = 0; y < puzzleData.Height; ++y)
                {
                    PuzzleCell cell = puzzleData.GetCell(x, y);
                    if (cell.m_cellsToLinkTo != null)
                    {
                        foreach (Vector2Int cellCoords in cell.m_cellsToLinkTo)
                        {
                            GameObject objectToLinkFrom = spawnedObjects[x, y];
                            GameObject objectToLinkTo = spawnedObjects[cellCoords.x, cellCoords.y];
                            foreach (IActivator activator in objectToLinkFrom.GetComponents<IActivator>())
                            {
                                foreach (IActivatee activatee in objectToLinkTo.GetComponents<IActivatee>())
                                {
                                    activator.AssignActivee(activatee);
                                }
                            }
                        }
                    }
                }
            }

            return puzzleData;
        }
    }
}
