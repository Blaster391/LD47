using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject m_bulletPrefab;
    public KeyCode m_fire;
    public Transform m_spawnTrans;
    public float m_fireRate = 0.5f;
    public AudioClip m_shootSound;


    private AudioSource m_audioSource;
    private float m_tSinceFire = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

   
    // Update is called once per frame
    void Update()
    {
        if(!PlayerLife.Instance || !PlayerLife.Instance.IsAlive)
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
