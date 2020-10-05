using Scoreboard.Unity;
using ScoreboardCore.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelScoreManager : MonoBehaviour
{
    [SerializeField]
    private string _levelName = "";

    [SerializeField]
    private float _totalUpdateRate = 2.5f;
    [SerializeField]
    private float _scoreUpdateRate = 5.0f;

    [SerializeField]
    private LiveScores _liveScores = null;

    private List<ScoreResult> _scores = new List<ScoreResult>();

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


    private bool _scoreRequestSent = false;
    private float _scoreRequestResendTime = 0.0f;
    private int _maxResultsCount = 100;

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

        if(!_scoreRequestSent)
        {
            _scoreRequestResendTime -= Time.deltaTime;

            if (_scoreRequestResendTime < 0.0f)
            {
                GetScores();
            }
        }

        ProcessScores();

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

        _scoreRequestSent = true;
        

        Score localScore = MakePlayerScore();

        var scoreboardAPI = PlayerInfo.Instance?.GetComponent<ScoreboardComponent>();


        Func<List<ScoreboardCore.Data.ScoreResult>, bool, bool> callback = (results, success) =>
        {
            _scoreRequestSent = false;
            _scoreRequestResendTime = _scoreUpdateRate;

            if (success)
            {
                _scores = results;
                ProcessScores();

                //m_scoreboardTitleText.text = $"Leaderboard ({m_playerStats.Level})";

                //m_scoreboardText.text = "";
                //for (int i = 0; i < results.Count; ++i)
                //{
                //    ScoreResult result = results[i];
                //    m_scoreboardText.text += $"{i + 1}. {result.Score.User} : {result.Score.ScoreValue} \n";
                //}

                //m_scoreboardText.gameObject.SetActive(true);
                //m_scoreboardTitleText.gameObject.SetActive(true);
            }

            return true;
        };

        scoreboardAPI.GetHighscores(callback, _levelName, true, _maxResultsCount);

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


    private void ProcessScores()
    {
        if(_scores.Count == 0)
        {
            return;
        }

        var localScore = MakePlayerScore();
        var user = localScore.User;
        var myResult = _scores.Where(x => x.Score.User == user).FirstOrDefault();
        if (myResult != null)
        {
            if (myResult.Score.ScoreValue < localScore.ScoreValue)
            {
                _scores.Remove(myResult);
                ScoreResult result = new ScoreResult();
                result.Ranking = -1;
                result.Score = localScore;
                _scores.Add(result);

                _scores = _scores.OrderByDescending(x => x.Score.ScoreValue).ToList();
            }
        }
        else
        {
            ScoreResult result = new ScoreResult();
            result.Ranking = -1;
            result.Score = localScore;
            _scores.Add(result);

            _scores = _scores.OrderByDescending(x => x.Score.ScoreValue).ToList();
        }


        int index = 0;
        for (index = 0; index < _scores.Count; ++index)
        {
            if (_scores[index].Score.User == user)
            {
                break;
            }
        }

        var resultScore = new List<LiveScores.Score>();
        for (int i = 0; i < 3; ++i)
        {
            if (i < _scores.Count)
            {
                LiveScores.Score s = new LiveScores.Score();
                if (_scores[i].Score.ExtraData.ContainsKey("Username"))
                {
                    s.Username = _scores[i].Score.ExtraData["Username"];
                }
                else
                {
                    s.Username = "Unknown";
                }

                s.ScoreValue = _scores[i].Score.ScoreValue;
                s.Ranking = i + 1;
                resultScore.Add(s);
            }
            else
            {
                break;
            }

            
        }
        _liveScores.SetHighScores(resultScore);

        resultScore = new List<LiveScores.Score>();
        if (index < _maxResultsCount - 2 && index > 3)
        {
            
            for (int i = index - 1; i < index + 2; ++i)
            {
                if (i < _scores.Count)
                {
                    LiveScores.Score s = new LiveScores.Score();
                    if (_scores[i].Score.ExtraData.ContainsKey("Username"))
                    {
                        s.Username = _scores[i].Score.ExtraData["Username"];
                    }
                    else
                    {
                        s.Username = "Unknown";
                    }

                    s.ScoreValue = _scores[i].Score.ScoreValue;
                    s.Ranking = i + 1;
                    resultScore.Add(s);
                }
                else
                {
                    break;
                }


            }

            _liveScores.SetRelativeScores(resultScore);
        }
        else
        {
            _liveScores.SetRelativeScores(new List<LiveScores.Score>());
        }
    }
}
