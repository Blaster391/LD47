using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceClampedCarController : MonoBehaviour
{

    public Transform m_forcePos;
    public LayerMask m_trackLayer;
    private Rigidbody m_rBody;
    public float m_speed = 3;
    public float m_minSpeed = 0.1f;
    public float m_strafe = 0.05f;
    public Spline m_spline;

    private int m_curSplineIndex = 0;
       
    private bool GetRaycastDownAtNewPosition(Vector3 movementDirection, out RaycastHit hitInfo)
    {
        Ray ray = new Ray(transform.position + movementDirection   , -transform.up);

        if (Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, m_trackLayer))
        {
            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        RaycastHit hitInfo;

        Vector3 minFwdSpeed = Mathf.Max(m_minSpeed, m_speed * Input.GetAxis("Vertical")) * transform.forward ;

        if (GetRaycastDownAtNewPosition(minFwdSpeed + transform.right * Input.GetAxis("Horizontal") * m_strafe, out hitInfo))
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
            float rotSpeed = 175;
            float movSpeed = 75;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, Time.deltaTime * rotSpeed);
            transform.position = Vector3.MoveTowards(transform.position, predPos, Time.deltaTime * movSpeed);
        }

      
    }
}
