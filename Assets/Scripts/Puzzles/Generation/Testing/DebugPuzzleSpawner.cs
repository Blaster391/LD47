using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Puzzles
{
    /*
     *  Used testing the spawning of puzzles. Just in world space from wherever this object happens to be.
     */

    public class DebugPuzzleSpawner : MonoBehaviour
    {
        public List<PuzzleAsset> m_puzzlesToSpawn;

        // Currently assuming car is 1, cells 1.5x that.
        public float m_cellSize = 1.5f;
        public int m_width = 5;
        public float m_forwardCellsPerSideways = 1f;

        public float m_startingDifficulty = 0f;
        public float m_difficultyIncreasePerCell = 0.02f; // 50 cells to max difficulty

        public int m_emulatedDistanceInCells = 30;

        // Umm
        public bool m_regenerateNew = false;
        public bool m_regenerateOld = false;

        void Start()
        {
            Generate(true);
        }

        public void OnValidate()
        {
            if (Application.isPlaying)
            {
                if (m_spawnedObjects.Count > 0)
                {
                    foreach (KeyValuePair<int, GameObject[,]> puzzleObjects in m_spawnedObjects)
                    {
                        // Delete old objects
                        for (int x = 0; x < puzzleObjects.Value.GetLength(0); ++x)
                        {
                            for (int y = 0; y < puzzleObjects.Value.GetLength(1); ++y)
                            {
                                if (puzzleObjects.Value[x, y] != null)
                                {
                                    Destroy(puzzleObjects.Value[x, y]);
                                }
                            }
                        }
                    }
                    m_spawnedObjects.Clear();

                    // Spawn new ones
                    Generate(m_regenerateOld ? false : true);

                    m_regenerateNew = false;
                    m_regenerateOld = false;
                }
            }
        }

        private void Generate(bool i_newSeed)
        {
            m_emulatedState.Reset();

            if (i_newSeed)
            {
                m_cachedSeed = System.Environment.TickCount;
            }
            Random.InitState(m_cachedSeed);

            m_emulatedState.m_difficulty = m_startingDifficulty;

            int puzzleIndex = 0;
            while (m_emulatedState.m_distance < m_emulatedDistanceInCells)
            {
                PuzzleAsset selectedPuzzle = SelectPuzzle();
                if(selectedPuzzle == null)
                {
                    break;
                }

                PuzzleData puzzleData = GeneratePuzzle(puzzleIndex, selectedPuzzle.Generator, transform.position + new Vector3(0f, 0f, m_emulatedState.m_distance * m_cellSize));

                m_emulatedState.m_distance += puzzleData.Height;
                m_emulatedState.m_difficulty = Mathf.Min(m_emulatedState.m_difficulty + m_difficultyIncreasePerCell * puzzleData.Height, 1f);

                ++puzzleIndex;
            }
        }

        private PuzzleAsset SelectPuzzle()
        {
            List<PuzzleAsset> possiblePuzzles = new List<PuzzleAsset>();
            foreach(PuzzleAsset availablePuzzle in m_puzzlesToSpawn)
            {
                if(m_emulatedState.m_difficulty >= availablePuzzle.DifficultyMin && m_emulatedState.m_difficulty <= availablePuzzle.DifficultyMax)
                {
                    possiblePuzzles.Add(availablePuzzle);
                }
            }

            if(possiblePuzzles.Count == 0)
            {
                return null;
            }

            return possiblePuzzles[Random.Range(0, possiblePuzzles.Count)];
        }

        private PuzzleData GeneratePuzzle(int i_puzzleIndex, IPuzzleGenerator i_puzzleGeneratorIF, Vector3 i_startingPosition)
        {
            PuzzleData puzzleData = i_puzzleGeneratorIF.GeneratePuzzle(m_width, m_emulatedState.m_difficulty, m_forwardCellsPerSideways);

            Vector2Int gridSize = new Vector2Int(puzzleData.Width, puzzleData.Height);

            GameObject[,] spawnedObjects = new GameObject[gridSize.x, gridSize.y];
            m_spawnedObjects.Add(i_puzzleIndex, spawnedObjects);

            for (int x = 0; x < puzzleData.Width; ++x)
            {
                for (int y = 0; y < puzzleData.Height; ++y)
                {
                    PuzzleCell cell = puzzleData.GetCell(x, y);
                    if (cell.m_prefabToSpawn != null)
                    {
                        spawnedObjects[x, y] = Instantiate(cell.m_prefabToSpawn, i_startingPosition + new Vector3(x * m_cellSize, 0, y * m_cellSize), Quaternion.identity, transform);
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

        Dictionary<int, GameObject[,]> m_spawnedObjects = new Dictionary<int, GameObject[,]>();

        // Current emulated 'race state'
        public class EmulatedState
        {
            public float m_difficulty = 0f;
            public float m_distance = 0f;

            public void Reset()
            {
                m_difficulty = 0f;
                m_distance = 0f;
            }
        }
        EmulatedState m_emulatedState = new EmulatedState();

        int m_cachedSeed = 0;
    }
}