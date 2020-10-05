using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private GameObject _HUD;
    [SerializeField]
    private GameObject _deadedScreen;

    private void Start()
    {
        PlayerLife.Instance.OnDeath.AddListener(OnPlayerDeaded);   
    }

    public void OnPlayAgainSelected()
    {

    }

    public void OnQuitSelected()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnPlayerDeaded(Vector3 uselessDeathLocation)
    {
        _HUD.SetActive(false);
        _deadedScreen.SetActive(true);
    }
}