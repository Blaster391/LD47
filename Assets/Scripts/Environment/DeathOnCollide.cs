using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnCollide : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("GET DEDED");
        if(collision.rigidbody)
        {
            var player = collision.rigidbody.GetComponent<PlayerLife>();
            if (player)
            {
                Vector3 collisionPoint = transform.position;
                if (collision.contacts.Length > 0)
                {
                    collisionPoint = new Vector3();
                    foreach (var contact in collision.contacts)
                    {
                        collisionPoint += contact.point;
                    }
                    collisionPoint /= collision.contacts.Length;
                }

                player.Kill(collisionPoint);
            }
        }
    }

}
