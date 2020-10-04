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
    private float _slowestSpeed = 1.0f;
    [SerializeField]
    private float _fastestSpeed = 1.0f;

    [SerializeField]
    private float _slowestHeight = 1.0f;
    [SerializeField]
    private float _fastestHeight = 1.0f;

    [SerializeField]
    private float _slowestDistance = 5.0f;
    [SerializeField]
    private float _fastestDistance = 5.0f;

    [SerializeField]
    private float _wallAvoidDist = 0.25f;
    [SerializeField]
    private float _forwardTilt = 10.0f;


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

        float currentSpeed = _carController.m_curSpeed;
        currentSpeed = Mathf.Min(currentSpeed, _fastestSpeed);
        currentSpeed = Mathf.Max(currentSpeed, _slowestSpeed);

        float speedDiff = _fastestSpeed - _slowestSpeed;

        float speedProp = (currentSpeed - _slowestSpeed) / speedDiff;
        float height = Mathf.Lerp(_slowestHeight, _fastestHeight, speedProp);
        float distance = Mathf.Lerp(_slowestDistance, _fastestDistance, speedProp);

        if (_carController)
        {
            _averageSpeed += _carController.m_curSpeed;
            _averageSpeed /= 2.0f;
        }

        _dummyTransform.position = _focus.transform.position + (_focus.transform.up * height) - (_focus.transform.forward * distance);
        _dummyTransform.LookAt(_focus.transform.position, _focus.transform.up);


        RaycastHit hit;
        if (Physics.Raycast(_focus.transform.position + _focus.transform.up, -_dummyTransform.forward, out hit, Vector3.Distance(_dummyTransform.position, _focus.transform.position), ~9, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.collider.gameObject);
            _dummyTransform.position = hit.point + (_focus.transform.forward * _wallAvoidDist);
            _dummyTransform.LookAt(_focus.transform, _focus.transform.up);
        }

        _dummyTransform.Rotate(new Vector3(-1, 0, 0), _forwardTilt);

        if (_useSmoothing)
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
