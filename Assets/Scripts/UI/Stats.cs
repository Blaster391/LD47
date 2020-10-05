using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct PlayerStats
{
    public int score;
    public int lapsCompleted;
    public float secondsAlive;
}

public class Stats : MonoBehaviour
{
    private const int SECONDS_PER_MINUTE = 60;

    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _lapsText;
    [SerializeField]
    private TextMeshProUGUI _timeText;

    [SerializeField]
    private string _scoreString;
    [SerializeField]
    private string _lapsString;
    [SerializeField]
    private string _timeString;

    private PlayerStats _currentStats;

    public void UpdateStats(PlayerStats newStats)
    {
        _currentStats = newStats;
        UpdateUI();
    }

    public void UpdateScore(int score)
    {
        _currentStats.score = score;
        UpdateUI();
    }

    public void UpdateLaps(int laps)
    {
        _currentStats.lapsCompleted = laps;
        UpdateUI();
    }

    public void UpdateTime(float time)
    {
        _currentStats.secondsAlive = time;
        UpdateUI();
    }

    private void UpdateUI()
    {
        _scoreText.text = _scoreString + _currentStats.score;
        _lapsText.text = _lapsString + _currentStats.lapsCompleted;
        _timeText.text = _timeString + FormatTimeAlive();
    }

    private string FormatTimeAlive()
    {
        string timeAlive = $"{Mathf.Floor(_currentStats.secondsAlive / SECONDS_PER_MINUTE):0}m ";
        timeAlive += $"{_currentStats.secondsAlive % SECONDS_PER_MINUTE:0}s";

        return timeAlive;
    }
}
