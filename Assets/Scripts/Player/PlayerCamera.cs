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
    [SerializeField]
    private float _wallAvoidDist = 0.25f;

    // Update is called once per frame
    void Update()
    {
        if (_focus == null)
        {
            return;
        }

        transform.position = _focus.transform.position + (_focus.transform.up * _height) - (_focus.transform.forward * _distance);
        transform.LookAt(_focus.transform, _focus.transform.up);

        RaycastHit hit;
        if (Physics.Raycast(_focus.transform.position, -transform.forward, out hit, Vector3.Distance(transform.position, _focus.transform.position)))
        {
            transform.position = hit.point + (_focus.transform.forward * _wallAvoidDist);
            transform.LookAt(_focus.transform, _focus.transform.up);
        }
    }
}
