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

        public float m_upwardProjectionLengthBeforeDownwardRaycast = 1f;
        public float m_downwardRaycastLength = 2f;
        public LayerMask m_trackLayer;

        // Internal
        private Dictionary<int, GameObject[,]> m_spawnedObjects = new Dictionary<int, GameObject[,]>();

        // State
        private float m_runningDifficulty = 0f;
        private int m_runningDistancePS = 0;

        private float m_trackLengthWS = 0f;
        private float m_trackLengthPS = 0; // Puzzle Space boiiii
        private float m_trackTPerCell = 0;

        // Updated values
        private float m_forwardCellsPerSideways = 2f; // Currently not actually updated as expected. Can just expose and tweak tbh

        void Start()
        {
            if(m_spline == null)
            {
                Debug.LogError("No Spline Referenced in Puzzle Manager Script");
            }

            // Cache some values
            m_trackLengthWS = m_spline.Length;
            m_trackLengthPS = m_trackLengthWS / m_cellSize;
            m_trackTPerCell = m_cellSize / m_trackLengthWS;

            // For now lets just try and spawn one puzzle
            Generate();
        }

        List<SplineTransformData> m_splineTransformsCalulated = new List<SplineTransformData>();
        void Update()
        {
            foreach(SplineTransformData std in m_splineTransformsCalulated)
            {
                Debug.DrawLine(std.m_worldPos, std.m_worldPos + std.m_worldUp * 5, Color.green);
            }
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

                float totalTrackT = m_runningDistancePS / m_trackLengthPS;
                PuzzleData puzzleData = GeneratePuzzle(puzzleIndex, selectedPuzzle.Generator, totalTrackT, totalTrackT % 1f);

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

        private PuzzleData GeneratePuzzle(int i_puzzleIndex, IPuzzleGenerator i_puzzleGeneratorIF, float i_startingSplineT, float i_startingSplineTClamped)
        {
            PuzzleData puzzleData = i_puzzleGeneratorIF.GeneratePuzzle(m_trackWidthInCells, m_runningDifficulty, m_forwardCellsPerSideways);

            Vector2Int gridSize = new Vector2Int(puzzleData.Width, puzzleData.Height);

            GameObject[,] spawnedObjects = new GameObject[gridSize.x, gridSize.y];
            m_spawnedObjects.Add(i_puzzleIndex, spawnedObjects);

            float GetT(int i_y)
            {
                return i_startingSplineTClamped + i_y * m_trackTPerCell;
            }

            // Lets figure out wtf we need to spawn this crap
            for (int y = 0; y < puzzleData.Height; ++y)
            {
                // Every time we go up one we need to find our new transform info
                float splineT = GetT(y);
                SplineTransformData splineTransformData = m_spline.CalculateAproxSplineTransformData(splineT);

                int timesAroundTrack = Mathf.FloorToInt(i_startingSplineT);
                if (timesAroundTrack % 2 == 1) // If odd, flip
                {
                    splineTransformData.m_worldUp = -splineTransformData.m_worldUp;
                }
                m_splineTransformsCalulated.Add(splineTransformData);

                Vector3 puzzleLeftPos = splineTransformData.m_worldPos - (Vector3.Cross(splineTransformData.m_worldUp.normalized, splineTransformData.m_worldFwd.normalized) * m_cellSize * (m_trackWidthInCells - 1) / 2f);
                Quaternion objectRotation = Quaternion.LookRotation(splineTransformData.m_worldFwd.normalized, splineTransformData.m_worldUp.normalized);

                for (int x = 0; x < puzzleData.Width; ++x)
                {
                    PuzzleCell cell = puzzleData.GetCell(x, y);
                    if (cell.m_prefabToSpawn != null)
                    {
                        Vector3 positionToSpawn = puzzleLeftPos + (objectRotation * new Vector3(x * m_cellSize, 0, 0));
                        Quaternion rotationToSpawn = objectRotation;

                        // Adjust because the track isn't actually flat all the way along

                        // Lift above the track
                        Vector3 raycastFrom = positionToSpawn + splineTransformData.m_worldUp * m_upwardProjectionLengthBeforeDownwardRaycast;

                        if(Physics.Raycast(new Ray(raycastFrom, -splineTransformData.m_worldUp), out RaycastHit hit, m_downwardRaycastLength, m_trackLayer))
                        {
                            // Adjust
                            positionToSpawn = hit.point;
                            rotationToSpawn = Quaternion.LookRotation(splineTransformData.m_worldFwd.normalized, hit.normal.normalized);
                        }

                        spawnedObjects[x, y] = Instantiate(cell.m_prefabToSpawn, positionToSpawn, rotationToSpawn, transform);
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
