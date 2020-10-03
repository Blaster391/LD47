﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceClampedCarController : MonoBehaviour
{

    public Transform m_forcePos;
    public LayerMask m_trackLayer;
    private Rigidbody m_rBody;
    public float speed = 1;
    public Spline m_spline;

    private int m_curSplineIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private bool GetRaycastDownAtNewPosition(Vector3 movementDirection, out RaycastHit hitInfo)
    {
        Ray ray = new Ray(transform.position + movementDirection * speed  , -transform.up);

        if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, m_trackLayer))
        {
            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        RaycastHit hitInfo;

        if (GetRaycastDownAtNewPosition(transform.forward, out hitInfo))
        {

            //Get the desired forward from the spline and update our spline indexes if appropriate
            bool incInd = false;
            Vector3 predPos = hitInfo.point + hitInfo.normal * .5f;
            Vector3 desiredF = m_spline.GetSplineForwardVec(m_curSplineIndex, transform.position, predPos, ref incInd);
            if(incInd)
            {
                Debug.Log("Increment");
                m_curSplineIndex = m_spline.ClampIndex(m_curSplineIndex + 1);
            }


           // Debug.Log(m_curSplineIndex);
            Debug.DrawLine(transform.position, transform.position + 5 * desiredF, Color.cyan);
            Quaternion rot = Quaternion.LookRotation(desiredF, hitInfo.normal);
            transform.rotation = rot;
            transform.position = predPos;
        }

      
    }
}
