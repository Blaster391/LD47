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


        Score score = new Score();
        score.User = PlayerInfo.Instance.Id;

        score.Level = _levelName;
        score.ScoreValue = Mathf.RoundToInt(playerScore.Score);

        score.ExtraData.Add("Username", PlayerInfo.Instance.Username);
        score.ExtraData.Add("Time", playerScore.TotalTime.ToString());
        score.ExtraData.Add("Lap", playerScore.Lap.ToString());

        scoreboardAPI.SubmitResult(score, callback);
        
    }


    private void LapCompleted()
    {
        if(PlayerInfo.Instance.Username == "")
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
        score.ExtraData.Add("Lap", playerScore.Lap.ToString());

        scoreboardAPI.SubmitResult(score, callback);

    }
}
