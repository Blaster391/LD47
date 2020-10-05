using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LBDataRow : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _rankText;
    [SerializeField]
    private TMP_Text _nameText;
    [SerializeField]
    private TMP_Text _scoreText;

    private UIScoreData _rowData;

    public void SetRowData(UIScoreData lbRowData)
    {
        _rowData = lbRowData;

        _rankText.text = _rowData.Ranking.ToString();
        _nameText.text = _rowData.Username;
        _scoreText.text = _rowData.ScoreValue.ToString();
    }
}
