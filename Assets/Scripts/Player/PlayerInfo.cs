using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private string _username = "";
    private string _id = "";

    public int CarModel { get; set; } = 0;
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

    //public GameObject SelectedCar { get; set; }
    public int SelectedCar
    {
        get { return _selectedCar; }
        set { _selectedCar = value; }
    }

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

        if(!PlayerPrefs.HasKey("Player_ID"))
        {
            Guid guid = Guid.NewGuid();
            PlayerPrefs.SetString("Player_ID", guid.ToString());
        }

        if(!PlayerPrefs.HasKey("Car"))
        {
            PlayerPrefs.SetInt("Car", 0);
        }

        CarModel = PlayerPrefs.GetInt("Car");
        _id = PlayerPrefs.GetString("Player_ID");
 

        _username = PlayerPrefs.GetString("Player_Name", _username);
        PlayerPrefs.Save();
    }
}
