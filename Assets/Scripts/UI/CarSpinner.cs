using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarSpinner : MonoBehaviour
{
    [SerializeField]
    private float _rotationSpeed = 20.0f;
    [SerializeField]
    private TMP_Text _nameText;
    [SerializeField]
    private List<GameObject> _carList = new List<GameObject>();
    [SerializeField]
    private float _startingRotation;

    private int _currentCar = 0;

    public int CurrentCar
    {
        get { return _currentCar; }
    }

    private void Start()
    {
        _startingRotation = transform.rotation.eulerAngles.y;
        if (_carList[_currentCar].GetComponent<NameComponent>())
        {
            _nameText.text = _carList[_currentCar].GetComponent<NameComponent>().PublicName;
        }
        else
        {
            _nameText.text = _carList[_currentCar].name;
        }
    }

    private void OnEnable()
    {
        ChangeCar(0);
    }

    private void OnDisable()
    {
        ResetRot();
    }

    private void Update()
    {
        transform.Rotate(transform.up, _rotationSpeed * Time.deltaTime);
    }

    public void ChangeCar(int moveDir)
    {
        if (!isActiveAndEnabled) { return; }

        _carList[_currentCar].SetActive(false);
        ResetRot();

        _currentCar += moveDir;
        _currentCar = (int)Mathf.Repeat(_currentCar, _carList.Count);
        _carList[_currentCar].SetActive(true);

        if(_carList[_currentCar].GetComponent<NameComponent>())
        {
            _nameText.text = _carList[_currentCar].GetComponent<NameComponent>().PublicName;
        }
        else
        {
            _nameText.text = _carList[_currentCar].name;
        }
    }

    private void ResetRot()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.y = _startingRotation;
        transform.eulerAngles = rotation;
    }
}
