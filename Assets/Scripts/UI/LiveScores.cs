using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public struct UIScoreData
{
    public int Ranking;
    public string Username;
    public int ScoreValue;
}

public class LiveScores : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _totalLapsText;
    [SerializeField]
    private TextMeshProUGUI _highScoresText;
    [SerializeField]
    private TextMeshProUGUI _scoresSeparatorText;
    [SerializeField]
    private TextMeshProUGUI _relativeScoresText;
    
    private List<UIScoreData> _highScores = new List<UIScoreData>();
    private List<UIScoreData> _relativeScores = new List<UIScoreData>();

    public void SetLaps(int laps)
    {
        if(laps > -1)
        {
            _totalLapsText.text = $"Global Loops: {laps}";
        }
        else
        {
            _totalLapsText.text = "";
        }
    }

    public void SetHighScores(List<UIScoreData> scores)
    {
        _highScores = scores;
    }

    public void SetRelativeScores(List<UIScoreData> scores)
    {
        _relativeScores = scores;
    }

    private void Start()
    {
        if (!_relativeScoresText)
        {
            return;
        }

        _totalLapsText.text = "";
        _highScoresText.text = "";
        _relativeScoresText.text = "";
        _scoresSeparatorText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(!_relativeScoresText)
        {
            return;
        }

        _relativeScoresText.text = "";
        if (_relativeScores.Count > 0)
        {
            foreach(var score in _relativeScores)
            {
                if(_relativeScoresText.text != "")
                {
                    _relativeScoresText.text += '\n';
                }

                _relativeScoresText.text += FormatScore(score);
            }
        }

        _highScoresText.text = "";
        if (_highScores.Count > 0)
        {
            foreach (var score in _highScores)
            {
                if (_highScoresText.text != "")
                {
                    _highScoresText.text += '\n';
                }

                _highScoresText.text += FormatScore(score);
            }
        }


        if (_highScores.Count != 0 && _relativeScores.Count != 0)
        {
            _scoresSeparatorText.text = "-----------------------";
        }
        else
        {
            _scoresSeparatorText.text = "";
        }
    }

    private string FormatScore(UIScoreData _score)
    {
        return $"{_score.Ranking}. {_score.Username} - {_score.ScoreValue}";
    }
}
