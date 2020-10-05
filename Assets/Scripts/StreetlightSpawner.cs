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

    [SerializeField]
    private float _offsetSize = 8.5f;

    void Start()
    {
        if(_spline)
        {
            
            Vector3 lastPlacedPosition = _spline.m_splinePoints[0].transform.position;

            bool left = false;
            for (int i = 1; i < _spline.m_splinePoints.Length - 1; ++i)
            {
                var currentPoint = _spline.m_splinePoints[i];
                var nextPosition = _spline.m_splinePoints[i + 1];
                float lastPlacedDistance = Vector3.Distance(lastPlacedPosition, currentPoint.transform.position);

                if(lastPlacedDistance > _minDistance)
                {
                    Vector3 forward = (nextPosition.transform.position - currentPoint.transform.position);
                    Vector3 up = currentPoint.transform.up;
                    Vector3 offset = Vector3.Cross(forward.normalized, up.normalized);
                    offset *= _offsetSize;

                    lastPlacedPosition = currentPoint.transform.position;

                    //var upLamp = Instantiate(_streetlightPrefab);
                    //upLamp.transform.position = currentPoint.position;
                    //upLamp.transform.rotation = currentPoint.rotation;
                    //upLamp.transform.position += offset;

                    if (left)
                    {
                        offset = -offset;
                    }


                    var downLamp = Instantiate(_streetlightPrefab);
                    downLamp.transform.position = currentPoint.transform.position;
                    downLamp.transform.position += offset;
                    downLamp.transform.rotation = Quaternion.LookRotation(forward, -up);

                   // downLamp.transform.Rotate(new Vector3(1, 0, 0), 180);

                    left = !left;
                }
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
