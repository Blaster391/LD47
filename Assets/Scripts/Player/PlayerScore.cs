using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerScore : MonoBehaviour
{
    public UnityEvent OnLapComplete;

    [SerializeField]
    private float _suvivalScore = 1.0f;
    [SerializeField]
    private Stats _hudStats = null;

    private PlayerLife _life = null;
    private SurfaceClampedCarController _car = null;
    private float _score = 0.0f;
    private float _previousLapScore = 0.0f;
    private float _currentLapScore = 0.0f;

    private float _currentLapTime = 0.0f;
    public float TotalTime { get; private set; }
    public float PreviousLapTime { get; private set; }

    public int Lap { get; private set; } = 0;
    public float Score { get { return _score; } }
    public float PreviousLapScore { get { return _previousLapScore; } }

    // Start is called before the first frame update
    void Start()
    {
        _life = GetComponent<PlayerLife>();
        _car = GetComponent<SurfaceClampedCarController>();
    }

    public void AddScore(float score)
    {
        _score += score;
        _currentLapScore += score;
    }

    public void CompleteLap()
    {
        _previousLapScore = _currentLapScore;
        PreviousLapTime = _currentLapTime;

        _currentLapScore = 0.0f;
        _currentLapTime = 0.0f;
        Lap++;

        if (_hudStats)
        {
            _hudStats.UpdateLaps(Lap);
        }


        OnLapComplete.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if(_life.IsAlive)
        {
            AddScore(Time.deltaTime * _suvivalScore * _car.m_curSpeed);
            TotalTime += Time.deltaTime;
            _currentLapTime += Time.deltaTime;

            if (_hudStats)
            {
                _hudStats.UpdateScore(Mathf.RoundToInt(_score));
                _hudStats.UpdateTime(TotalTime);
            }


        }
    }
}
