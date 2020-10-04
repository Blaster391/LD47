using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button _firstButton;

    // Start is called before the first frame update
    void Start()
    {
        _firstButton.Select();
    }

    public void OnPlayButtonSelected()
    {
        Debug.Log("Play Game");
    }

    public void OnQuitButtonSelected()
    {
        Debug.Log("Quit Game");
    }
}
