using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

        [SerializeField] private List<GameObject> m_clutter = new List<GameObject>();
        [SerializeField] private int m_clutterMin = 0;
        [SerializeField] private int m_clutterMax = 10;

        public List<PuzzleAsset> m_puzzlesToSpawn;

        // Currently assuming car is 1, cells 1.5x that.
        public float m_cellSize = 1.5f;
        public float m_modelScaler = 1f;
        public int m_trackWidthInCells = 5;

        public float m_startingDifficulty = 0f;
        public float m_difficultyIncreasePerCell = 0.02f; // 50 cells to max difficulty

        public int m_initialSpaceCellSize = 10;

        public float m_upwardProjectionLengthBeforeDownwardRaycast = 1f;
        public float m_downwardRaycastLength = 2f;
        public LayerMask m_trackLayer;

        // Visuals
        [Header("Visuals")]
        public int m_puzzlesSpawnedAtATime = 4;
        public int m_puzzlesInfront = 2;

        // Internal

        private struct SpawnedPuzzle
        {
            public GameObject[,] m_spawnedObjects;
            public int m_startPuzzleHeight;
            public int m_endPuzzleHeight;
            public PuzzleData m_puzzleData;
            public SplineTransformData m_startTransformData;
        }
        [SerializeField] private Queue<SpawnedPuzzle> m_spawnedPuzzles = new Queue<SpawnedPuzzle>();

        // State
        [Header("Running State - Dont Edit")]
        [SerializeField] private float m_runningDifficulty = 0f;
        [SerializeField] private int m_runningDistancePS = 0;

        private float m_trackLengthWS = 0f;
        private float m_trackLengthPS = 0; // Puzzle Space boiiii
        private float m_trackTPerCell = 0;

        // Updated values
        [Header("Misc")]
        public float m_forwardCellsPerSideways = 2f; // Currently not actually updated as expected. Can just expose and tweak tbh

        private bool m_generatingPuzzle = false;

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

            m_runningDifficulty = m_startingDifficulty;
            m_runningDistancePS = m_initialSpaceCellSize;

            // For now lets just try and spawn one puzzle
            StartCoroutine(GeneratePuzzlesToMax());
        }

        List<SplineTransformData> m_splineTransformsCalulated = new List<SplineTransformData>();

        void Update()
        {
            if (!m_generatingPuzzle)
            {
                float currentDistanceTravelledSS = m_car.GetTotalSplineTraveled();
                int currentDistanceTravelledPS = Mathf.FloorToInt(currentDistanceTravelledSS * m_trackLengthPS);

                // What we going for here...
                // If we've started the last puzzle in the queue we spawn the next one, destroy the previous
                if (m_spawnedPuzzles.Count < 3)
                {
                    return;
                }

                if (currentDistanceTravelledPS >= m_spawnedPuzzles.ElementAt(m_spawnedPuzzles.Count - m_puzzlesInfront).m_startPuzzleHeight)
                {
                    RemovePuzzle();
                    StartCoroutine(GeneratePuzzlesToMax());
                }
            }
        }

        // Must destroy a puzzle first to spawn a new one
        private IEnumerator GeneratePuzzlesToMax()
        {
            m_generatingPuzzle = true;
            while (m_spawnedPuzzles.Count < m_puzzlesSpawnedAtATime)
            {
                PuzzleAsset selectedPuzzle = SelectPuzzle(m_runningDifficulty);
                if (selectedPuzzle == null)
                {
                    break;
                }

                float totalTrackT = m_runningDistancePS / m_trackLengthPS;
                yield return GeneratePuzzle(selectedPuzzle.Generator, totalTrackT, totalTrackT % 1f);

                int puzzleHeight = m_spawnedPuzzles.Last().m_puzzleData.Height;
                m_runningDistancePS += puzzleHeight;
                m_runningDifficulty = Mathf.Min(m_runningDifficulty + m_difficultyIncreasePerCell * puzzleHeight, 1f);
            }
            m_generatingPuzzle = false;
        }

        private void RemovePuzzle()
        {
            SpawnedPuzzle puzzleToRemove = m_spawnedPuzzles.Dequeue();

            // Delete old objects
            for (int x = 0; x < puzzleToRemove.m_spawnedObjects.GetLength(0); ++x)
            {
                for (int y = 0; y < puzzleToRemove.m_spawnedObjects.GetLength(1); ++y)
                {
                    if (puzzleToRemove.m_spawnedObjects[x, y] != null)
                    {
                        Destroy(puzzleToRemove.m_spawnedObjects[x, y]);
                    }
                }
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

        private void AddRandomStuff(PuzzleData puzzleData)
        {
            puzzleData.FillRandomClutter(m_clutter, m_clutterMin, m_clutterMax);
        }

        private IEnumerator GeneratePuzzle(IPuzzleGenerator i_puzzleGeneratorIF, float i_startingSplineT, float i_startingSplineTClamped)
        {
            PuzzleData puzzleData = i_puzzleGeneratorIF.GeneratePuzzle(m_trackWidthInCells, m_runningDifficulty, m_forwardCellsPerSideways);
            AddRandomStuff(puzzleData);
            SplineTransformData puzzleStartTransformData = m_spline.CalculateAproxSplineTransformData(i_startingSplineTClamped);

            Vector2Int gridSize = new Vector2Int(puzzleData.Width, puzzleData.Height);

            SpawnedPuzzle spawnedPuzzleData = new SpawnedPuzzle();
            spawnedPuzzleData.m_spawnedObjects = new GameObject[gridSize.x, gridSize.y];
            spawnedPuzzleData.m_startPuzzleHeight = m_runningDistancePS;
            spawnedPuzzleData.m_endPuzzleHeight = m_runningDistancePS + puzzleData.Height;
            spawnedPuzzleData.m_puzzleData = puzzleData;
            spawnedPuzzleData.m_startTransformData = puzzleStartTransformData;
            m_spawnedPuzzles.Enqueue(spawnedPuzzleData);

            float GetT(int i_y)
            {
                return (i_startingSplineTClamped + i_y * m_trackTPerCell) % 1f;
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

                        spawnedPuzzleData.m_spawnedObjects[x, y] = Instantiate(cell.m_prefabToSpawn, positionToSpawn, cell.m_prefabRotation * rotationToSpawn, transform);
                        spawnedPuzzleData.m_spawnedObjects[x, y].transform.localScale *= m_modelScaler;
                    }
                }

                yield return 0;
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
                            GameObject objectToLinkFrom = spawnedPuzzleData.m_spawnedObjects[x, y];
                            GameObject objectToLinkTo = spawnedPuzzleData.m_spawnedObjects[cellCoords.x, cellCoords.y];
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

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                // Draw em
                Gizmos.color = Color.cyan;
                foreach (SpawnedPuzzle spawnedPuzzle in m_spawnedPuzzles)
                {
                    Vector3 puzzleOnTrackPos = spawnedPuzzle.m_startTransformData.m_worldPos;
                    Gizmos.DrawLine(puzzleOnTrackPos, puzzleOnTrackPos + spawnedPuzzle.m_startTransformData.m_worldUp * 5f);
                }

                // Draw us
                //Gizmos.color = Color.green;
                //SplineTransformData carSplineData = m_spline.CalculateAproxSplineTransformData(m_car.GetClampedSpineTraveled());
                //Vector3 carOnTrackPos = carSplineData.m_worldPos;
                //Gizmos.DrawLine(carOnTrackPos, carOnTrackPos + carSplineData.m_worldUp * 5f);

                Gizmos.color = Color.white;
            }
        }
    }
}
