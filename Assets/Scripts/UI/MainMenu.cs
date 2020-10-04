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

    // Start is called before the first frame update
    void Start()
    {
        _firstButton.Select();
    }

    public void OnPlayButtonSelected()
    {
        _buttons.SetActive(false);

        _inputField.gameObject.SetActive(true);
        _inputField.Select();
    }

    public void OnQuitButtonSelected()
    {
        Application.Quit();
    }

    public void OnNameInputted()
    {
        string playerName = _nameText.text;

        _inputField.gameObject.SetActive(false);

        _vehicleSelect.SetActive(true);
    }

    public void OnVehicleSelected()
    {
        SceneManager.LoadScene("Main");
    }
}
