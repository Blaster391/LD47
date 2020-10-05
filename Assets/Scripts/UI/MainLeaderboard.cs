using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainLeaderboard : MonoBehaviour
{
    [SerializeField]
    private GameObject _trackSelecter;
    [SerializeField]
    private TMP_Text _trackNameText;
    [SerializeField]
    private Leaderboard _leaderboard;

    public void GetPreviousTrack()
    {

    }

    public void GetNextTrack()
    {

    }

    public void UpdateLeaderboard(string trackName, List<UIScoreData> leaderboardData)
    {
        _trackSelecter.SetActive(true);
        _trackNameText.text = trackName;

        _leaderboard.UpdateScores(leaderboardData);
    }
}
