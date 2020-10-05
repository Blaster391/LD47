using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject m_bulletPrefab;
    public KeyCode m_fire;
    public KeyCode m_rotateKey;
    public Transform m_spawnTrans;
    public float m_fireRate = 0.5f;
    public AudioClip m_shootSound;


    public float m_maxRotate = 15.0f;
    public float m_rotDelt = 1.0f;
    public float m_upAngleRef = 30;

    private AudioSource m_audioSource;
    private float m_tSinceFire = 0;
    private Vector3 startRot;

    private float m_rotCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        startRot = transform.rotation.eulerAngles;

    }

    void UpdateGunRotation()
    {
        
        //How hilly is the road, lerp our rotation up or down accordingly (hill, rot in minus, drop, rot plus)
        float cosUp = Vector3.Dot(Vector3.up, PlayerLife.Instance.transform.up);

        Vector3 upToUse = cosUp > 0 ? Vector3.up : Vector3.down; 
        float cosFwd = Vector3.Dot(upToUse, PlayerLife.Instance.transform.forward);
        //Going up a hill
        if(cosFwd > 0.03)
        {
            transform.localRotation = Quaternion.Euler(-m_maxRotate + 90, 0, 0);

        }
        //Going down
        else if(cosFwd < -0.03)
        {
            transform.localRotation = Quaternion.Euler(m_maxRotate + 90, 0, 0);

        }
        else
            transform.localRotation = Quaternion.Euler(90, 0, 0);



    }


    // Update is called once per frame
    void Update()
    {
     
        UpdateGunRotation();

        if (!PlayerLife.Instance || !PlayerLife.Instance.IsAlive)
        {
            return;
        }


        m_tSinceFire += Time.deltaTime;
        if(Input.GetKey(m_fire) && m_tSinceFire >= m_fireRate)
        {
            GameObject bullet = Instantiate(m_bulletPrefab, m_spawnTrans.position, transform.rotation);
            m_tSinceFire = 0;

            m_audioSource.PlayOneShot(m_shootSound);

        }
    }
}
