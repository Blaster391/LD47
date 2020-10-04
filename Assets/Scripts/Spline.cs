using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System.Linq;

public class Spline : MonoBehaviour
{
    //public float testVal = 0.5f;

    const float c_resolutionDebug = 0.2f;
    public float m_segmentLengthDelta = 0.01f; //MUST BE SAME AS MIN SPEED
    public float m_rayOffset = 5.0f;
    public LayerMask m_trackLayer;

    public float Length { get { return m_segments.Sum(segment => segment.m_length); } }
    public Vector3 Start { get { Vector3 startPos, fwd; GetSplineTransform(0, 0, out fwd, out startPos); return startPos; } }

    //Each segment of the spline can be defined by the four spline paramaters.
    private struct Segment
    {
        public Segment(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            m_a = a;
            m_b = b;
            m_c = c;
            m_d = d;
            m_length = 0;
            m_startUp = Vector3.zero;
            m_endUp = Vector3.zero;
        }

        public Vector3 m_a;
        public Vector3 m_b;
        public Vector3 m_c;
        public Vector3 m_d;
        public float m_length;

        public Vector3 m_startUp;
        public Vector3 m_endUp;
    }

    public Transform[] m_splinePoints;
    private Segment[] m_segments;

    private void Awake()
    {
        CalculateSplineSegemnts();
    }

    public SplineTransformData CalculateAproxSplineTransformData(float i_splineVal)
    {
        SplineTransformData data = new SplineTransformData();

        //Convert sline val to worldspace distance 
        float distTravelled = i_splineVal * Length;

        //Walk the spline this distance
        int lastIndex = 0;
        float distTotal = 0;
        float distSeg = 0;
        Vector3 prevPos = m_splinePoints[0].position;
        Vector3 fwd = Vector3.forward;

        bool walkDone = false;
        for (int j = 0; j < m_splinePoints.Length && !walkDone; ++j)
        {
            distSeg = 0;
            Vector3 p0 = m_splinePoints[ClampIndex(j - 1)].position;
            Vector3 p1 = m_splinePoints[j].position;
            Vector3 p2 = m_splinePoints[ClampIndex(j + 1)].position;
            Vector3 p3 = m_splinePoints[ClampIndex(j + 2)].position;

            prevPos = p1;
            //Walk along the spline from p1 to p2, incrementing the time value
            for (int i = 0; i < (int)(1f / m_segmentLengthDelta) && !walkDone; ++i)
            {
                float t = (i + 1) * m_segmentLengthDelta;

                Vector3 curPos = GetIntraSegmentPos(m_segments[j], t);
                float dist = Vector3.Magnitude(curPos - prevPos);
                fwd = Vector3.Normalize(curPos - prevPos);
                distTotal += dist;
                distSeg += dist;
                if (distTotal >= distTravelled)
                {
                    walkDone = true;
                }
                prevPos = curPos;
            }

            if (!walkDone)
            {
                lastIndex = j;
            }
        }


        //We know the last index, and how far ahead we are to the next, interpolate that shid baby
        float segVal = Mathf.Min(distSeg / m_segments[lastIndex].m_length, 1);
        data.m_worldPos = prevPos;
        data.m_worldFwd = fwd;
        data.m_worldUp = Vector3.Lerp(m_segments[lastIndex].m_startUp, m_segments[lastIndex].m_endUp, segVal);

        return data;
    }


    //Precalculate the segments of the spline
    void CalculateSplineSegemnts()
    {
        m_segments = new Segment[m_splinePoints.Length];

        Vector3 rayPositon = m_splinePoints[0].position;
        Vector3 rayDirection = Vector3.down;
        for (int i = 0; i < m_splinePoints.Length; ++i)
        {
            Vector3 p0 = m_splinePoints[ClampIndex(i - 1)].position;
            Vector3 p1 = m_splinePoints[i].position;
            Vector3 p2 = m_splinePoints[ClampIndex(i + 1)].position;
            Vector3 p3 = m_splinePoints[ClampIndex(i + 2)].position;

            if (Application.isPlaying)
            {
                CalculateSegmentCoefficients(p0, p1, p2, p3, out m_segments[i], ref rayPositon, ref rayDirection);
            }
            else
            {
                CalculateSegmentCoefficients(p0, p1, p2, p3, out m_segments[i]);
            }
        }
    }

    Vector3 GetTrackUp(Vector3 i_rayOrigin, Vector3 i_rayDir)
    {
        if(Physics.Raycast(new Ray(i_rayOrigin - i_rayDir * m_rayOffset, i_rayDir), out RaycastHit hit, float.PositiveInfinity, m_trackLayer))
        {
            return hit.normal;
        }
        else
        {
            return i_rayDir;
        }

    }

    //Determine per segment coefficients
    void CalculateSegmentCoefficients(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, out Segment o_segment, ref Vector3 i_rayStart, ref Vector3 i_rayDir)
    {
        Vector3 a = 2.0f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = (2.0f * p0) - (5.0f * p1) + (4.0f * p2) - p3;
        Vector3 d = -p0 + (3.0f * p1) - (3.0f * p2) + p3;
        o_segment = new Segment(a, b, c, d);

        i_rayDir = -GetTrackUp(i_rayStart, i_rayDir);
        o_segment.m_startUp = -i_rayDir;
        //Walk the segment to determine its length and  up vectors
        Vector3 prevPos = p1;
        for (int i = 0; i < (int)(1f / m_segmentLengthDelta); ++i)
        {
            float t = (i + 1) * m_segmentLengthDelta;

            Vector3 curPos = GetIntraSegmentPos(o_segment, t);
            o_segment.m_length += Vector3.Magnitude(curPos - prevPos);
            prevPos = curPos;

            i_rayStart = curPos;
            i_rayDir = -GetTrackUp(i_rayStart, i_rayDir);
        }

        o_segment.m_endUp = -i_rayDir;
    }


    //debug version with no up calculating
    void CalculateSegmentCoefficients(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, out Segment o_segment)
    {
        Vector3 a = 2.0f * p1;
        Vector3 b = p2 - p0;
        Vector3 c = (2.0f * p0) - (5.0f * p1) + (4.0f * p2) - p3;
        Vector3 d = -p0 + (3.0f * p1) - (3.0f * p2) + p3;
        o_segment = new Segment(a, b, c, d);


        //Walk the segment to determine its length and  up vectors
        Vector3 prevPos = p1;
        for (int i = 0; i < (int)(1f / m_segmentLengthDelta); ++i)
        {
            float t = (i + 1) * m_segmentLengthDelta;

            Vector3 curPos = GetIntraSegmentPos(o_segment, t);
            prevPos = curPos;
        }

    }

    //Get position some arbitrary distance along a spline segment
    Vector3 GetIntraSegmentPos(Segment i_segment, float t)
    {
       
        return 0.5f * (i_segment.m_a + (i_segment.m_b * t) + (i_segment.m_c * Mathf.Pow(t, 2)) + (i_segment.m_d * Mathf.Pow(t, 3)));
    }


    //Update the index and curT, and return a new position and forward vec
    public void Lookahead(ref int io_index, float i_speed, ref float io_curT, out Vector3 o_fwd, out Vector3 o_pos)
    {
        //Normalise the speed
        float speedNorm = i_speed / m_segments[io_index].m_length;


        //Get our current spline pos and our next spline pos.
        Vector3 curPos = GetIntraSegmentPos(io_index, io_curT);
        io_curT = Mathf.Min(io_curT + i_speed, 1.0f);
        o_pos = GetIntraSegmentPos(io_index, io_curT);
        o_fwd = Vector3.Normalize(o_pos - curPos);

        //Have we reached end of cur segment?
        if(Mathf.Approximately(io_curT, 1.0f))
        {
            io_curT = 0;
            io_index = ClampIndex(io_index + 1);
        }
    }

    public void GetSplineTransform(int io_index, float i_t, out Vector3 o_fwd, out Vector3 o_pos)
    {
        if(i_t == 1)
        {
            Debug.LogError("do not");
        }
        o_pos = GetIntraSegmentPos(io_index, i_t);
        float peekT = Mathf.Min(i_t + m_segmentLengthDelta, 1);
        Vector3 o_peekPos = GetIntraSegmentPos(io_index, peekT);

        o_fwd = Vector3.Normalize(o_peekPos - o_pos);
    }

    public Vector3 GetSplineForwardVec(int i_segmentInd, Vector3 i_curPos, Vector3 i_predictedPosition, ref bool o_incSplineInd)
    {
        //Calculated current spline pos and predicted to determine the forward an agent following the spline should have at its current pos
        Segment segment = m_segments[i_segmentInd];

        float curT = GetT(i_segmentInd, i_curPos);
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

    //Vector3 spherePos;
    //private void Update()
    //{
    //    for (int i = 0; i < m_segments.Length; ++i)
    //    {
    //        Debug.DrawLine(m_splinePoints[i].position, m_splinePoints[i].position + m_segments[i].m_startUp, Color.cyan);
    //    }

    //    SplineTransformData data = CalculateAproxSplineTransformData(testVal);
    //    spherePos = data.m_worldPos;
    //    Debug.DrawLine(spherePos, spherePos + data.m_worldUp * 5, Color.green);
    //}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        CalculateSplineSegemnts();

        Gizmos.color = Color.green;
        //Gizmos.DrawSphere(spherePos, 2);

        Gizmos.color = Color.white;
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
