using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Stage 1 - Menu")]
    [SerializeField]
    private GameObject _buttons;
    [SerializeField]
    private Button _firstButton;

    [Header("Stage 2 - Name Entry")]
    [SerializeField]
    private TMP_InputField _inputField;
    [SerializeField]
    private TMP_Text _nameText;

    [Header("Stage 3 - Vehicle")]
    [SerializeField]
    private GameObject _vehicleSelect;

    [Header("Stage 4 - Leaderboard")]
    [SerializeField]
    private GameObject _leaderboard;
    
    public void OnPlayButtonSelected()
    {
        _buttons.SetActive(false);

        _inputField.gameObject.SetActive(true);
        _inputField.SetTextWithoutNotify(PlayerInfo.Instance.Username);
        _inputField.Select();
    }

    public void OnLeaderboardSelected()
    {
        _buttons.SetActive(false);
        _leaderboard.SetActive(true);
    }

    public void OnLeaderboardClosed()
    {
        _leaderboard.SetActive(false);
        _buttons.SetActive(true);
    }

    public void OnQuitButtonSelected()
    {
        Application.Quit();
    }

    public void OnNameInputted()
    {
        string playerName = _nameText.text;

        PlayerInfo.Instance.Username = playerName;

        _inputField.gameObject.SetActive(false);

        _vehicleSelect.SetActive(true);
    }

    public void OnVehicleSelected()
    {
        SceneManager.LoadScene("Main");
    }
}
