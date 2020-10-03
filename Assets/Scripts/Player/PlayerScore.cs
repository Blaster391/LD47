using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private float _suvivalScore = 1.0f;

    private PlayerLife _life = null;
    private float _score = 0.0f;

    public float Score { get { return _score; } }

    // Start is called before the first frame update
    void Start()
    {
        _life = GetComponent<PlayerLife>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_life.IsAlive)
        {
            _score += Time.deltaTime * _suvivalScore;
        }
    }
}
