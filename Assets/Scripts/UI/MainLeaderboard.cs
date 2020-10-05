using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Scoreboard.Unity;
using System;

public class MainLeaderboard : MonoBehaviour
{
    [SerializeField]
    private GameObject _trackSelecter;
    [SerializeField]
    private TMP_Text _trackNameText;
    [SerializeField]
    private Leaderboard _leaderboard;

    public class LeadboardData
    {
        public string Name = "";
        public List<UIScoreData> Data = new List<UIScoreData>();
    }
    private List<LeadboardData> m_leaderboards = new List<LeadboardData>();
    private int m_index = 0;


    private void Start()
    {
        ScoreboardComponent scoreboard = PlayerInfo.Instance?.GetComponent<ScoreboardComponent>();
        if(scoreboard)
        {
            Func<List<ScoreboardCore.Data.ScoreResult>, bool, bool> callbackB = (results, success) =>
            {
                if (success)
                {
                    List<UIScoreData> scoreData = ParseResultsData(results);
                   
                    AddLeaderboard("NAME HERE B", scoreData);
                   
                }

                return true;
            };

            Func<List<ScoreboardCore.Data.ScoreResult>, bool, bool> callbackC = (results, success) =>
            {
                if (success)
                {
                    List<UIScoreData> scoreData = ParseResultsData(results);
                    AddLeaderboard("NAME HERE C", scoreData);
                }

                return true;
            };

            scoreboard.GetHighscores(callbackB, "TrackB", true, 10);

            scoreboard.GetHighscores(callbackC, "TrackC", true, 10);
        }
    }

    private void AddLeaderboard(string name, List<UIScoreData> data)
    {
        LeadboardData ld = new LeadboardData();
        ld.Data = data;
        ld.Name = name;
        m_leaderboards.Add(ld);


        if (m_leaderboards.Count == 1)
        {
            UpdateLeaderboard();
        }
        
    }

    private List<UIScoreData> ParseResultsData(List<ScoreboardCore.Data.ScoreResult> results)
    {
        List<UIScoreData> scoreData = new List<UIScoreData>();

        foreach (var r in results)
        {
            UIScoreData data = new UIScoreData();
            data.Ranking = r.Ranking;
            data.ScoreValue = r.Score.ScoreValue;

            if (r.Score.ExtraData.ContainsKey("Username"))
            {
                data.Username = r.Score.ExtraData["Username"];
            }
            else
            {
                data.Username = "Unknown";
            }

            scoreData.Add(data);
        }

        return scoreData;
    }

    public void GetPreviousTrack()
    {
        m_index++;
        m_index = m_index % m_leaderboards.Count;
        UpdateLeaderboard();
    }

    public void GetNextTrack()
    {
        m_index++;
        m_index = m_index % m_leaderboards.Count;
        UpdateLeaderboard();
    }

    private void Update()
    {
        _trackSelecter.SetActive(m_leaderboards.Count > 1);
    }

    public void UpdateLeaderboard()
    {
        if (m_leaderboards.Count > 0)
        {
            _trackNameText.text = m_leaderboards[m_index].Name;
            _leaderboard.UpdateScores(m_leaderboards[m_index].Data);
        }
    }
}
