using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private enum EMenuStage
    {
        Main,
        NameEntry,
        VroomVrooms,
        LoopDeLoop
    }

    [Header("Stage 1 - Menu")]
    [SerializeField]
    private GameObject _buttons;
    [SerializeField]
    private Button _firstButton;

    [Header("Stage 2 - Name Entry")]
    [SerializeField]
    private GameObject _nameEntry;
    [SerializeField]
    private TMP_InputField _inputField;
    [SerializeField]
    private TMP_Text _nameText;

    [Header("Stage 3 - Vehicle")]
    [SerializeField]
    private GameObject _vehicleSelect;
    [SerializeField]
    private GameObject _carSpins;
    [SerializeField]
    private TMP_Text _titleText;
    [SerializeField]
    private string _vehicleTitle;

    [Header("Stage 4 - Track")]
    [SerializeField]
    private string _trackTitle;
    [SerializeField]
    private GameObject _trackSpins;

    [Header("Stage 1.1 - Leaderboard")]
    [SerializeField]
    private GameObject _leaderboard;

    [Header("Control Buttons")]
    [SerializeField]
    private GameObject _controlButs;
    [SerializeField]
    private Button _submitButton;

    private EMenuStage _leStage = EMenuStage.Main;

    public void OnPlayButtonSelected()
    {
        _leStage = EMenuStage.NameEntry;
        _buttons.SetActive(false);

        _nameEntry.SetActive(true);
        _controlButs.SetActive(true);
        _inputField.SetTextWithoutNotify(PlayerInfo.Instance.Username);
        _nameText.text = PlayerInfo.Instance.Username;
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

    public void OnNameChanged()
    {
        if (_inputField.text.Length == 0)
        {
            _submitButton.interactable = false;
        }
        else
        {
            _submitButton.interactable = true;
        }
    }

    public void OnGenericSubmit()
    {
        switch (_leStage)
        {
            case EMenuStage.NameEntry:
                PlayerInfo.Instance.Username = _nameText.text;
                _leStage = EMenuStage.VroomVrooms;
                _nameEntry.SetActive(false);
                _vehicleSelect.SetActive(true);
                break;
            case EMenuStage.VroomVrooms:
                _titleText.text = _trackTitle;
                _leStage = EMenuStage.LoopDeLoop;
                _carSpins.SetActive(false);
                _trackSpins.SetActive(true);
                break;
            case EMenuStage.LoopDeLoop:
                PlayerInfo.Instance.SelectedCar = _carSpins.GetComponent<CarSpinner>().CurrentCar;
                switch (_trackSpins.GetComponent<CarSpinner>().CurrentCar)
                {
                    case 0:
                        SceneManager.LoadScene("TrackC");
                        break;
                    case 1:
                        SceneManager.LoadScene("TrackB");
                        break;

                }
                break;
        }
    }

    public void OnGenericBack()
    {
        switch (_leStage)
        {
            case EMenuStage.NameEntry:
                _nameEntry.SetActive(false);
                _controlButs.SetActive(false);
                _buttons.SetActive(true);
                _leStage = EMenuStage.Main;
                break;
            case EMenuStage.VroomVrooms:
                _vehicleSelect.SetActive(false);
                _nameEntry.SetActive(true);
                _leStage = EMenuStage.NameEntry;
                break;
            case EMenuStage.LoopDeLoop:
                _titleText.text = _vehicleTitle;
                _leStage = EMenuStage.VroomVrooms;
                _trackSpins.SetActive(false);
                _carSpins.SetActive(true);
                break;
        }
    }
}
