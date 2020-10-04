using Scoreboard.Unity;
using ScoreboardCore.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScoreManager : MonoBehaviour
{
    [SerializeField]
    private string _levelName = "";

    [SerializeField]
    private float _totalUpdateRate = 30.0f;

    [SerializeField]
    private LiveScores _liveScores = null;

    private int _totalLaps = -1;
    public int TotalLaps { 
        get
        {
            return _totalLaps;
        }
        private set
        {
            _totalLaps = value;

            if(_liveScores)
            {
                _liveScores.SetLaps(_totalLaps);
            }
        } 
    }

    private bool _totalRequestSent = false;
    private float _totalRequestResendTime = 0.0f;

    void Start()
    {
        if (PlayerLife.Instance != null)
        {
            PlayerLife.Instance.OnDeath.AddListener(OnDeath);

            if (PlayerLife.Instance.GetComponent<PlayerScore>() != null)
            {
                PlayerLife.Instance.GetComponent<PlayerScore>().OnLapComplete.AddListener(LapCompleted);
            }
        }
    }

    private void Update()
    {
        if(!_totalRequestSent)
        {
            _totalRequestResendTime -= Time.deltaTime;

            if(_totalRequestResendTime < 0.0f)
            {
                GetTotalLaps();
            }
        }


    }

    private void OnDeath(Vector3 deathPos)
    {
        if (PlayerInfo.Instance.Username == "")
        {
            return;
        }

        var scoreboardAPI = PlayerInfo.Instance?.GetComponent<ScoreboardComponent>();
        var playerScore = PlayerLife.Instance.GetComponent<PlayerScore>();

        Func<bool, string, bool> callback = (success, result) =>
        {
            if (success)
            {
                Debug.Log("SUCCESS");
            }
            else
            {
                Debug.Log("FAIL");
            }

            return true;
        };


        Score score = MakePlayerScore();

        scoreboardAPI.SubmitResult(score, callback);
        
    }


    private void LapCompleted()
    {
        TotalLaps++;

        if (PlayerInfo.Instance.Username == "")
        {
            return;
        }

        var scoreboardAPI = PlayerInfo.Instance?.GetComponent<ScoreboardComponent>();
        var playerScore = PlayerLife.Instance.GetComponent<PlayerScore>();

        Func<bool, string, bool> callback = (success, result) =>
        {
            if (success)
            {
                Debug.Log("SUCCESS");
            }
            else
            {
                Debug.Log("FAIL");
            }

            return true;
        };


        Score score = new Score();
        score.User = PlayerInfo.Instance.Id;

        score.Level = _levelName + "_LAP";
        score.ScoreValue = Mathf.RoundToInt(playerScore.PreviousLapScore);

        score.ExtraData.Add("Username", PlayerInfo.Instance.Username);
        score.ExtraData.Add("Time", playerScore.PreviousLapTime.ToString());
        score.ExtraData.Add("CompletedLaps", playerScore.Lap.ToString());
        scoreboardAPI.SubmitResult(score, callback);

    }

    private void GetTotalLaps()
    {
        if (PlayerInfo.Instance.Username == "")
        {
            return;
        }

        _totalRequestSent = true;

        var scoreboardAPI = PlayerInfo.Instance?.GetComponent<ScoreboardComponent>();

        Func<int, bool, bool> callback = (value, result) =>
        {
            _totalRequestSent = false;
            _totalRequestResendTime = _totalUpdateRate;

            if (value > -1)
            {
                Debug.Log("SUCCESS");
                TotalLaps = value;
            }
            else
            {
                Debug.Log("FAIL");
            }

            return true;
        };

        scoreboardAPI.GetTotalForLevel(callback, _levelName + "_LAP");

    }

    private void GetScores()
    {
        if(PlayerInfo.Instance.Username == "")
        {
            return;
        }

        Score localScore = MakePlayerScore();

        var scoreboardAPI = PlayerInfo.Instance?.GetComponent<ScoreboardComponent>();

    }

    private Score MakePlayerScore()
    {

        Score score = new Score();
        var playerScore = PlayerLife.Instance.GetComponent<PlayerScore>();
        if(!playerScore)
        {
            return score;
        }

        score.User = PlayerInfo.Instance.Id;

        score.Level = _levelName;
        score.ScoreValue = Mathf.RoundToInt(playerScore.Score);

        score.ExtraData.Add("Username", PlayerInfo.Instance.Username);
        score.ExtraData.Add("Time", playerScore.TotalTime.ToString());
        score.ExtraData.Add("Lap", playerScore.Lap.ToString());

        return score;
    }
}
