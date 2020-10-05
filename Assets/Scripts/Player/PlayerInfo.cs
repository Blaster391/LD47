using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private string _username = "";
    private string _id = "";

    public string Id { get { return _id; } }
    public string Username { get { return _username; }
        set
        {
            _username = value.Trim();
            PlayerPrefs.SetString("Player_Name", _username);
            PlayerPrefs.Save();
        }
    }

    private int _selectedCar;

    public int SelectedCar
    {
        get { return _selectedCar; }
        set 
        { 
            _selectedCar = value; 
            PlayerPrefs.SetInt("Car", _selectedCar); 
            PlayerPrefs.Save();
        }
    }

    private static PlayerInfo m_instance = null;
    public static PlayerInfo Instance { get { return m_instance; } }

    Resolution _initialRes;
    void Awake()
    {
        _initialRes = Screen.currentResolution;
        Screen.SetResolution(_initialRes.width, _initialRes.height, FullScreenMode.FullScreenWindow);

        if (m_instance == null)
        {
            m_instance = this;
            DontDestroyOnLoad(this.gameObject);
            return;
        }
        Destroy(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

        if(!PlayerPrefs.HasKey("Player_ID"))
        {
            Guid guid = Guid.NewGuid();
            PlayerPrefs.SetString("Player_ID", guid.ToString());
        }

        if(!PlayerPrefs.HasKey("Car"))
        {
            PlayerPrefs.SetInt("Car", 0);
        }

        _selectedCar = PlayerPrefs.GetInt("Car");
        _id = PlayerPrefs.GetString("Player_ID");
 

        _username = PlayerPrefs.GetString("Player_Name", _username);
        PlayerPrefs.Save();
    }


    bool _lockedTo1080 = false;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
        {
            if(_initialRes.width < 1920)
            {
                return;
            }
            
            if (!_lockedTo1080)
            {
                Screen.SetResolution(1920, 1080, FullScreenMode.ExclusiveFullScreen);
            }
            else
            {
                Screen.SetResolution(_initialRes.width, _initialRes.height, FullScreenMode.FullScreenWindow);
            }

            _lockedTo1080 = !_lockedTo1080;
        }

    }
}
