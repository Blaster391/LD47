using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private GameObject _focus = null;
    [SerializeField]
    private Transform _dummyTransform = null;

    [SerializeField]
    private float _height = 1.0f;
    [SerializeField]
    private float _distance = 5.0f;
    [SerializeField]
    private float _wallAvoidDist = 0.25f;

    [SerializeField]
    private bool _useSmoothing = true;
    [SerializeField]
    private float _maxMoveDelta = 1.0f;
    [SerializeField]
    private float _maxTurnDelta = 1.0f;

    private float _averageSpeed = 0.0f;

    private SurfaceClampedCarController _carController;

    void Start()
    {
        if (_focus == null)
        {
            return;
        }

        _carController = _focus.GetComponent<SurfaceClampedCarController>();

        if(_carController)
        {
            _averageSpeed = _carController.m_curSpeed;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_focus == null)
        {
            return;
        }


        if (_carController)
        {
            _averageSpeed += _carController.m_curSpeed;
            _averageSpeed /= 2.0f;
        }

        _dummyTransform.position = _focus.transform.position + (_focus.transform.up * _height) - (_focus.transform.forward * _distance);
        _dummyTransform.LookAt(_focus.transform.position, _focus.transform.up);


        RaycastHit hit;
        if (Physics.Raycast(_focus.transform.position, -_dummyTransform.forward, out hit, Vector3.Distance(_dummyTransform.position, _focus.transform.position), ~8, QueryTriggerInteraction.Ignore))
        {
            _dummyTransform.position = hit.point + (_focus.transform.forward * _wallAvoidDist);
            _dummyTransform.LookAt(_focus.transform, _focus.transform.up);
        }


        if(_useSmoothing)
        {
            float maxMove = _averageSpeed * _maxMoveDelta * Time.deltaTime;
            float maxTurn = _averageSpeed * _maxTurnDelta * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, _dummyTransform.position, maxMove);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _dummyTransform.rotation, maxTurn);
        }
        else
        {
            transform.position = _dummyTransform.position;
            transform.rotation = _dummyTransform.rotation;
        }
    }
}
