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
            _username = value;
            PlayerPrefs.SetString("Player_Name", _username);
            PlayerPrefs.Save();
        }
    }

    public GameObject SelectedCar { get; set; }


    private static PlayerInfo m_instance = null;
    public static PlayerInfo Instance { get { return m_instance; } }

    void Awake()
    {
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
        Guid guid = Guid.NewGuid();
        _id = PlayerPrefs.GetString("Player_ID", guid.ToString());
        _username = PlayerPrefs.GetString("Player_Name", _username);
        PlayerPrefs.Save();
    }
}
