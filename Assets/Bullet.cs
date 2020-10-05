using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float m_lifetime = 5f;
    public float m_spawnV = 10;

    private Rigidbody m_rb;

    float m_tSinceSpawn = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody>();
        m_rb.velocity = m_spawnV * transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        m_tSinceSpawn += Time.deltaTime;

        if(m_tSinceSpawn > m_lifetime)
        {
            Destroy(gameObject);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        collision.collider.gameObject.SendMessage("OnShoot", null, SendMessageOptions.DontRequireReceiver);

        Destroy(gameObject);
    }
}
