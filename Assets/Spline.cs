using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Spline : MonoBehaviour
{
    const float c_resolutionDebug = 0.2f;

    //Each segment of the spline can be defined by the four spline paramaters.
    private struct Segment
    {
        public Segment(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            m_a = a;
            m_b = b;
            m_c = c;
            m_d = d;
        }

        public Vector3 m_a;
        public Vector3 m_b;
        public Vector3 m_c;
        public Vector3 m_d;
    }

    public Transform[] m_splinePoints;
    private Segment[] m_segments;

    private void Start()
    {
        CalculateSplineSegemnts();
    }

    //Precalculate the segments of the spline
    void CalculateSplineSegemnts()
    {
        m_segments = new Segment[m_splinePoints.Length];

        for (int i = 0; i < m_splinePoints.Length; ++i)
        {
            Vector3 p0 = m_splinePoints[ClampIndex(i - 1)].position;
            Vector3 p1 = m_splinePoints[i].position;
            Vector3 p2 = m_splinePoints[ClampIndex(i + 1)].position;
            Vector3 p3 = m_splinePoints[ClampIndex(i + 2)].position;

            CalculateSegmentCoefficients(p0, p1, p2, p3, out m_segments[i]);
        }
    }

    //Determine per segment coefficients
    void CalculateSegmentCoefficients(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, out Segment o_segment)
    {
        Vector3 a = 2.0f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = (2.0f * p0) - (5.0f * p1) + (4.0f * p2) - p3;
        Vector3 d = -p0 + (3.0f * p1) - (3.0f * p2) + p3;
        o_segment = new Segment(a, b, c, d);
    }

    //Get position some arbitrary distance along a spline segment
    Vector3 GetIntraSegmentPos(Segment i_segment, float t)
    {
       
        return 0.5f * (i_segment.m_a + (i_segment.m_b * t) + (i_segment.m_c * Mathf.Pow(t, 2)) + (i_segment.m_d * Mathf.Pow(t, 3)));
    }


    public Vector3 GetSplineForwardVec(int i_segmentInd, Vector3 i_curPos, Vector3 i_predictedPosition, ref bool o_incSplineInd)
    {
        //Calculated current spline pos and predicted to determine the forward an agent following the spline should have at its current pos
        Segment segment = m_segments[i_segmentInd];

        float curT = GetT(i_segmentInd, i_curPos);
        Debug.Log(curT);
        float threshold = 0.01f;
        if(curT + threshold >= 1)
        {
            o_incSplineInd = true;
            curT = 1;
        }

        Vector3 curSplinePos = 0.5f * (segment.m_a + (segment.m_b * curT) + (segment.m_c * Mathf.Pow(curT, 2)) + (segment.m_d * Mathf.Pow(curT, 3)));

        float predT = GetT(i_segmentInd, i_predictedPosition);
        Vector3 predSplinePos = 0.5f * (segment.m_a + (segment.m_b * predT) + (segment.m_c * Mathf.Pow(predT, 2)) + (segment.m_d * Mathf.Pow(predT, 3)));

        return Vector3.Normalize(predSplinePos - curSplinePos);

    }

    public Vector3 GetIntraSegmentPos(int i_segmentInd, float t)
    {
        Segment segment = m_segments[i_segmentInd];
        return 0.5f * (segment.m_a + (segment.m_b * t) + (segment.m_c * Mathf.Pow(t, 2)) + (segment.m_d * Mathf.Pow(t, 3)));
    }

    public float GetT(int i_splineIndex, Vector3 i_curPos)
    {
        //Get distance vec from pos i and i+1
        Vector3 segVec = (m_splinePoints[ClampIndex(i_splineIndex + 1)].position - m_splinePoints[i_splineIndex].position);

        //Get distance vec from agent to start spline point of its current segment projected in dir of segment
        float agentDist = Vector3.Dot(i_curPos - m_splinePoints[i_splineIndex].position, segVec.normalized);

        //Get projection of agentVec along segVec to see how close we are to end of this segment
        return agentDist / segVec.magnitude;

    }



    //Display without having to press play
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        CalculateSplineSegemnts();
        
        for (int i = 0; i < m_splinePoints.Length; i++)
        {
            DrawSplineSegment(i);
        }
    }

    public void DrawSplineSegment(int index)
   {
        //Sample 4 points around index to get spline between it and the next
        Vector3 p0 = m_splinePoints[ClampIndex(index - 1)].position;
        Vector3 p1 = m_splinePoints[index].position;
        Vector3 p2 = m_splinePoints[ClampIndex(index + 1)].position;
        Vector3 p3 = m_splinePoints[ClampIndex(index + 2)].position;

        Vector3 prevPos = p1;
        //Walk along the spline from p1 to p2, incrementing the time value
        for (int i = 0; i < (int)(1f/ c_resolutionDebug); ++i)
        {
            float t = (i + 1) * c_resolutionDebug;

            Vector3 curPos = GetIntraSegmentPos(m_segments[index], t); 
            Gizmos.DrawLine(prevPos, curPos);
            prevPos = curPos;
        }
    }


    
    //Clamp index such that the spline will form a complete loop
    public int ClampIndex(int index)
    {
        if(index < 0)
        {
            return m_splinePoints.Length - 1;
        }

        if(index > m_splinePoints.Length)
        {
            return 1;
        }

        if(index > m_splinePoints.Length - 1)
        {
            return 0;
        }

        return index;
    }

}
