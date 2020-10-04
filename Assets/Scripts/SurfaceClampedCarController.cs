using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceClampedCarController : MonoBehaviour
{
    public LayerMask m_trackLayer;
    public float accel = 0.001f;
    public float m_minSpeed = 0.02f;
    public float m_maxSpeed = 0.05f;
    public float m_strafeDelta = 0.1f;
    public float m_strafeMax = 1.5f;
    public Spline m_spline;
    public float m_curSpeed = 0.0f;
    public float m_floatDistanceMult = 0.5f;
    public uint m_smoothingFramesU = 10;
    public uint m_smoothingFramesF = 10;

    public float m_rayOffset = 1.25f;

    private SlidingAverageBuffer m_avgUpBuffer;
    private SlidingAverageBuffer m_avgFwdBuffer;
    private int m_curSplineIndex = 0;
    private float m_curSplineT = 0;
    private float m_curStrafe;
    private bool m_active = true;
    private int lapCount = 0;

    private void Start()
    {
        m_curSpeed = m_minSpeed;
        m_avgUpBuffer = new SlidingAverageBuffer(m_smoothingFramesU);
        m_avgFwdBuffer = new SlidingAverageBuffer(m_smoothingFramesF);

        //Place car in correct start position
        m_spline.GetSplineTransform(0, 0, out Vector3 fwd, out Vector3 pos);
        UpdateTransform(pos, fwd);

        var life = GetComponent<PlayerLife>();
        if (life)
        {
            life.OnDeath.AddListener(OnDeath);
        }
    }


    void UpdateTransform(Vector3 i_desiredPos, Vector3 i_desiredF)
    {
        //Apply strafing
        m_curStrafe = Mathf.Clamp(m_curStrafe + -Input.GetAxis("Horizontal") * m_strafeDelta, -m_strafeMax, m_strafeMax);

        //Calculate the right vector at the desired positon based on world up and desired f
        Vector3 right = Vector3.Normalize( Vector3.Cross(i_desiredF, transform.up)); //TODO, acount or dont allow forward to equal up
        i_desiredPos += m_curStrafe * right;

        //We have a desired position, now lets raycast down from there to see where the track is 
        Physics.Raycast(new Ray(i_desiredPos + transform.up * m_rayOffset, -transform.up), out RaycastHit hit, float.PositiveInfinity, m_trackLayer);
        {
            //From this we can adjust the position to be above the track and set the cars up vector
            Vector3 desiredU = hit.normal;

            m_avgFwdBuffer.PushValue(i_desiredF);
            m_avgUpBuffer.PushValue(desiredU);
            i_desiredPos += m_avgUpBuffer.GetAverage() * m_floatDistanceMult;

            Quaternion rot = Quaternion.LookRotation(m_avgFwdBuffer.GetAverage(), m_avgUpBuffer.GetAverage()); 
            transform.rotation = rot;
            transform.position = i_desiredPos;
        }
    }

    private void Update()
    {
        if (!m_active)
        {
            return;
        }

        //Look ahead on the spline by speed to get desired fwd, and initial desired pos
        Vector3 desiredF;
        Vector3 desiredPos;

        int splineIndPrev = m_curSplineIndex;
        m_curSpeed = Mathf.Clamp(m_curSpeed + Input.GetAxis("Vertical") * accel * Time.deltaTime, m_minSpeed, m_maxSpeed);
        m_spline.Lookahead(ref m_curSplineIndex, m_curSpeed, ref m_curSplineT, out desiredF, out desiredPos);

        if (m_curSplineIndex == 0 && splineIndPrev != 0)
        {
            ++lapCount;
            if (lapCount % 2 == 0)
            {
                OnLapComplete();
            }
        }

        UpdateTransform(desiredPos, desiredF);
    }

    private void OnLapComplete()
    {
        var playerScore = GetComponent<PlayerScore>();
        if(playerScore)
        {
            playerScore.CompleteLap();
        }
    }

    private void OnDeath(Vector3 lol)
    {
        m_active = false;
    }
}
