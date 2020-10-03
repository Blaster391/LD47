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
        public PuzzleAsset m_puzzleToSpawn;

        // Currently assuming car is 1, cells 1.5x that.
        public float m_cellSize = 1.5f;
        public int m_width = 5;
        public float m_difficulty = 0f;
        public float m_forwardCellsPerSideways = 1f;

        // Umm
        public bool m_refresh = false;

        void Start()
        {
            Generate();
        }

        public void OnValidate()
        {
            if (Application.isPlaying)
            {
                if (spawnedObjects != null)
                {
                    // Delete old objects
                    for (int x = 0; x < spawnedObjects.GetLength(0); ++x)
                    {
                        for (int y = 0; y < spawnedObjects.GetLength(1); ++y)
                        {
                            if (spawnedObjects[x, y] != null)
                            {
                                Destroy(spawnedObjects[x, y]);
                            }
                        }
                    }
                    spawnedObjects = null;

                    // Spawn new ones
                    Generate();

                    m_refresh = false;
                }
            }
        }

        private void Generate()
        {
            // Spawn stuff
            IPuzzleGenerator generator = m_puzzleToSpawn.Generator;
            PuzzleData puzzleData = generator.GeneratePuzzle(m_width, m_difficulty, m_forwardCellsPerSideways);

            Vector2Int gridSize = new Vector2Int(puzzleData.Width, puzzleData.Height);

            spawnedObjects = new GameObject[gridSize.x, gridSize.y];

            for (int x = 0; x < puzzleData.Width; ++x)
            {
                for (int y = 0; y < puzzleData.Height; ++y)
                {
                    PuzzleCell cell = puzzleData.GetCell(x, y);
                    if (cell.m_prefabToSpawn != null)
                    {
                        spawnedObjects[x, y] = Instantiate(cell.m_prefabToSpawn, transform.position + new Vector3(x * m_cellSize, 0, y * m_cellSize), Quaternion.identity, transform);
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
                        foreach(Vector2Int cellCoords in cell.m_cellsToLinkTo)
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
        }

        GameObject[,] spawnedObjects;
    }
}