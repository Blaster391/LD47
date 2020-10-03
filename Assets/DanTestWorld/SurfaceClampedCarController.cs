using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceClampedCarController : MonoBehaviour
{

    public Transform m_forcePos;
    public float m_accelForce = 500.0f;
    public float m_brakeForce = 200.0f;
    public float m_maxVelSqr = 900.0f;
    public float m_startVelSqr = 25.0f; //Also the min 
    public float m_sideForce = 100.0f;
    public float m_sideFrictionLerp = 0.5f;

    private Rigidbody m_rBody;

    // Start is called before the first frame update
    void Start()
    {
        m_rBody = GetComponent<Rigidbody>();
        m_rBody.velocity = Mathf.Sqrt(m_startVelSqr) * transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //Calculate the forces
        float fwdInput = Input.GetAxis("Vertical");
        float sideInput = Input.GetAxis("Horizontal");

        float fwdForceMag = 0;
        float sideForceMag = 0;

       
        sideForceMag = sideInput * m_sideForce;
       

        if(fwdInput > 0)
        {
            //Dont apply fwd force if it will make car too fast
            if((Vector3.Project(m_rBody.velocity, transform.forward).sqrMagnitude) < m_maxVelSqr)
            {
                fwdForceMag = m_accelForce * fwdInput;
            }

        }
        else if(fwdInput < 0)
        {
            //Dont apply brake force if it will make car too slow
            if (Vector3.SqrMagnitude(Vector3.Project(m_rBody.velocity, transform.forward)) > m_startVelSqr)
            {
                fwdForceMag = -m_brakeForce * Mathf.Abs(fwdInput);
            }
        }

        //TODO stop it from getting too slow

        Vector3 fwdForce = transform.forward * fwdForceMag;
        Vector3 sideForce = transform.right * sideForceMag;
        //Apply the forces
        m_rBody.AddForceAtPosition(sideForce, m_forcePos.position, ForceMode.VelocityChange);
        m_rBody.AddForceAtPosition(fwdForce, m_forcePos.position, ForceMode.VelocityChange);
    }
}
