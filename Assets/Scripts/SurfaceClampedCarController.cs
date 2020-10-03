using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceClampedCarController : MonoBehaviour
{
    public Transform m_forcePos;
    public LayerMask m_trackLayer;
    private Rigidbody m_rBody;
    public float m_accel = 0.2f;
    public float m_brakeAccel = 1.25f;
    public float m_maxSpeed = 1.5f;
    public float m_minSpeed = 0.1f;
    public float m_strafe = 0.05f;
    public Spline m_spline;

    private int m_curSplineIndex = 0;
    public float m_curSpeed;

    private void Start()
    {
        m_curSpeed = m_minSpeed;
    }

    private void FixedUpdate()
    {
        float accel = 0;
        if(Input.GetAxis("Vertical") > 0)
        {
            accel = m_accel;
        }
        else if(Input.GetAxis("Vertical") < 0)
        {
            accel = m_brakeAccel;
        }
        m_curSpeed = Mathf.Clamp(m_curSpeed + Input.GetAxis("Vertical") * accel * Time.deltaTime, m_minSpeed, m_maxSpeed);

        Vector3 minFwdSpeed = m_curSpeed * transform.forward;
        Vector3 moveDir = minFwdSpeed + transform.right * Input.GetAxis("Horizontal") * m_strafe;
        RaycastHit hitInfo;
        if (Physics.Raycast(new Ray(transform.position + moveDir, -transform.up), out hitInfo, float.PositiveInfinity, m_trackLayer))  
        {
            //Get the desired forward from the spline and update our spline indexes if appropriate
            bool incInd = false;
            Vector3 predPos = hitInfo.point + hitInfo.normal * .5f;
            Vector3 desiredF = m_spline.GetSplineForwardVec(m_curSplineIndex, transform.position, predPos, ref incInd);
            if(incInd)
            {
                m_curSplineIndex = m_spline.ClampIndex(m_curSplineIndex + 1);
            }

            Debug.DrawLine(transform.position, transform.position + 5 * desiredF, Color.cyan);
            Quaternion rot = Quaternion.LookRotation(desiredF, hitInfo.normal);
            float rotSpeed = 50000;
            float movSpeed = 75;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime * rotSpeed);
            transform.position = Vector3.MoveTowards(transform.position, predPos, Time.deltaTime * movSpeed);
        }
    }
}
