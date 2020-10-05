using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct GameOverData
{
    public string TrackName;
    public string UserName;
    public int Score;
    public int Laps;
    public float TimeAlive;
    public bool NewRank;
    public int RankNumber;
}

public class GameOverDataCont : MonoBehaviour
{
    private const int SECONDS_PER_MINUTE = 60;

    [SerializeField]
    private TMP_Text _trackName;
    [SerializeField]
    private TMP_Text _userName;
    [SerializeField]
    private TMP_Text _score;
    [SerializeField]
    private TMP_Text _laps;
    [SerializeField]
    private TMP_Text _timeAlive;
    [SerializeField]
    private TMP_Text _newRank;
    [SerializeField]
    private TMP_Text _rank;

    private GameOverData _goData;
    public GameOverData GameOverData
    {
        get { return _goData; }
    }

    public void SetData(GameOverData data)
    {
        _trackName.text = data.TrackName;
        _userName.text = data.UserName;
        _score.text = data.Score.ToString();
        _laps.text = data.Laps.ToString();
        _timeAlive.text = FormatTimeAlive(data.TimeAlive);

        if (data.NewRank)
        {
            _newRank.gameObject.SetActive(true);
            _rank.gameObject.SetActive(true);

            _rank.text = data.RankNumber.ToString();
        }
        else
        {
            _newRank.gameObject.SetActive(false);
            _rank.gameObject.SetActive(false);
        }
    }

    private string FormatTimeAlive(float time)
    {
        string timeAlive = $"{time / SECONDS_PER_MINUTE:0}m ";
        timeAlive += $"{time % SECONDS_PER_MINUTE:0}s";

        return timeAlive;
    }
}
