using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    [SerializeField]
    private RectTransform _fillTransform;
    [SerializeField]
    private Image _speedoFill;
    [SerializeField]
    private float _maxSpeed;
    [SerializeField]
    private float _maxFillWidth;

    [SerializeField]
    private Color _startColor;
    [SerializeField]
    private Color _endColor;

    private float _currentSpeed;
    
    public float MaxSpeed
    {
        get { return _maxSpeed; }
        set { _maxSpeed = value; UpdateSpeedo(); }
    }

    public float CurrentSpeed
    {
        get { return _currentSpeed; }
        set { _currentSpeed = value; UpdateSpeedo(); }
    }

    private void UpdateSpeedo()
    {
        Vector2 fillSize = _fillTransform.sizeDelta;
        fillSize.x = (_currentSpeed / _maxSpeed) * _maxFillWidth;
        _fillTransform.sizeDelta = fillSize;

        _speedoFill.color = Color.Lerp(_startColor, _endColor, (_currentSpeed / _maxSpeed));
    }
}
