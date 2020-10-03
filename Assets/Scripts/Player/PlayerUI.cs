using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _player = null;
    [SerializeField]
    private TextMeshProUGUI _scoreText = null;

    private PlayerLife _playerLife;
    private PlayerScore _playerScore;

    // Start is called before the first frame update
    void Start()
    {
        _playerLife = _player.GetComponent<PlayerLife>();
        _playerScore = _player.GetComponent<PlayerScore>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_player == null)
        {
            return;
        }

        _scoreText.text = $"Score: {_playerScore.Score:0}";
    }
}
