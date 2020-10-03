using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    private string _username = "";
    private string _id = "";
    private GameObject _selectedCar = null;

    // Start is called before the first frame update
    void Start()
    {
        Guid guid = Guid.NewGuid();
        _id = PlayerPrefs.GetString("Player_ID", guid.ToString());
        _username = PlayerPrefs.GetString("Player_Name", _username);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
