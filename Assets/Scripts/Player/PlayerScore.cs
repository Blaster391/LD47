using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private float _suvivalScore = 1.0f;
    [SerializeField]
    private Stats _hudStats = null;

    private PlayerLife _life = null;
    private float _score = 0.0f;

    public float Score { get { return _score; } }

    // Start is called before the first frame update
    void Start()
    {
        _life = GetComponent<PlayerLife>();
    }

    public void AddScore(float score)
    {
        _score += score;
    }

    // Update is called once per frame
    void Update()
    {
        if(_life.IsAlive)
        {
            _score += Time.deltaTime * _suvivalScore;

            if(_hudStats)
            {
                _hudStats.UpdateScore(Mathf.RoundToInt(_score));
            }
        }
    }
}
