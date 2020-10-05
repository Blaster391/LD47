using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinComponent : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 10.0f;

    [SerializeField]
    private float _bobSpeed = 1.0f;

    [SerializeField]
    private float _bobAmount = 1.0f;


    private float _currentBob = 0.0f;
    private Vector3 _intialPosition = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(new Vector3(0, 1, 0), Random.value * 360);
        _currentBob = Mathf.Sin(Random.value * 2.0f - 1.0f) * _bobAmount;
        _intialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), _rotateSpeed * Time.deltaTime);


        if(GetComponent<DestructionFX>()._constructed)
        {
            _currentBob += _bobSpeed * Time.deltaTime;
            transform.position = _intialPosition + transform.up * Mathf.Sin(_currentBob) * _bobAmount;
        }
    }
}
