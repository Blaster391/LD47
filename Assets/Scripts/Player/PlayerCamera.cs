using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject _focus = null;

    [SerializeField]
    private float _height = 1.0f;
    [SerializeField]
    private float _distance = 5.0f;

    // Update is called once per frame
    void Update()
    {
        if(_focus == null)
        {
            return;
        }

        transform.position = _focus.transform.position + (_focus.transform.up * _height) - (_focus.transform.forward * _distance);

        transform.LookAt(_focus.transform, _focus.transform.up);
    }
}
