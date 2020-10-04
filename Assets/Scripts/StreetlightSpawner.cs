using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetlightSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject _streetlightPrefab = null;

    [SerializeField]
    private float _minDistance = 50.0f;

    [SerializeField]
    private Spline _spline = null;

    void Start()
    {
        if(_spline)
        {
            
            Vector3 lastPlacedPosition = _spline.m_splinePoints[0].position;

            for(int i = 1; i < _spline.m_splinePoints.Length - 1; ++i)
            {
                var currentPoint = _spline.m_splinePoints[i];
                float lastPlacedDistance = Vector3.Distance(lastPlacedPosition, currentPoint.position);

                if(lastPlacedDistance > _minDistance)
                {
                    lastPlacedPosition = currentPoint.position;

                    var upLamp = Instantiate(_streetlightPrefab);
                    upLamp.transform.position = currentPoint.position;
                    upLamp.transform.rotation = currentPoint.rotation;

                    var downLamp = Instantiate(_streetlightPrefab);
                    downLamp.transform.position = currentPoint.position;
                    downLamp.transform.rotation = currentPoint.rotation;

                    downLamp.transform.Rotate(new Vector3(1, 0, 0), 180);
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
