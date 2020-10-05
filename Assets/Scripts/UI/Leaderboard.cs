using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    [SerializeField]
    private Transform _parentContainer;
    [SerializeField]
    private GameObject _rowPrefab;
    
    private List<LBDataRow> _rows = new List<LBDataRow>();
    
    public void UpdateScores(List<UIScoreData> leaderboardData)
    {
        SetupRows(leaderboardData.Count);

        for (int i = 0; i < _rows.Count; ++i)
        {
            _rows[i].SetRowData(leaderboardData[i]);
        }
    }

    private void SetupRows(int amount)
    {
        if (_rows.Count != amount)
        {
            if (_rows.Count < amount)
            {
                while (_rows.Count != amount)
                {
                    GameObject obj = Instantiate(_rowPrefab);
                    obj.transform.SetParent(_parentContainer);
                    obj.transform.localScale = Vector3.one;

                    _rows.Add(obj.GetComponent<LBDataRow>());
                }
            }
            else if (_rows.Count > amount)
            {
                while (_rows.Count != amount)
                {
                    Destroy(_rows[_rows.Count - 1].gameObject);
                    _rows.RemoveAt(_rows.Count - 1);
                }
            }
        }
    }
}
