using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreOnTrigger : MonoBehaviour
{
    [SerializeField]
    private float _scoreToAdd = 10.0f;

    private bool _scoreGiven = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if(_scoreGiven)
        {
            return;
        }

        Debug.Log("GET SCORED");

        if(collider.GetComponent<Bullet>() != null)
        {
            _scoreGiven = true;
            var dc = GetComponent<DestructionFX>();
            if (dc)
            {
                dc.Destruct(collider.gameObject);
            }
            return;
        }

        var player = collider.GetComponent<PlayerScore>();
        if (player)
        {
            player.AddScore(_scoreToAdd);
            _scoreGiven = true;
        }
        else
        {
            return;
        }

        var destructionComponent = GetComponent<DestructionFX>();
        if(destructionComponent)
        {
            destructionComponent.Destruct(collider.gameObject);
        }

        var audioSource = GetComponent<AudioSource>();
        if(audioSource)
        {
            audioSource.Play();
        }
    }
}
