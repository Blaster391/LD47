﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField]
    private AudioSource _engineSource;
    [SerializeField]
    private AudioSource _deathSource;
    [SerializeField]
    private AudioSource _tireScreechSource;
    [SerializeField]
    private AudioSource _shootSource;

    [SerializeField]
    private AudioSource _musicSource1;

    [SerializeField]
    private AudioClip _engineClip;
    [SerializeField]
    private AudioClip _deathClip;

    [SerializeField]
    private float _lowSpeedPitch = 1.0f;
    [SerializeField]
    private float _highSpeedPitch = 2.0f;

    private PlayerLife _life = null;
    private SurfaceClampedCarController _controller = null;

    // Start is called before the first frame update
    void Start()
    {
        _life = GetComponent<PlayerLife>();
        _controller = GetComponent<SurfaceClampedCarController>();
        _life.OnDeath.AddListener(OnDeath);
    }

    // Update is called once per frame
    void Update()
    {
        if(_life.IsAlive)
        {
            if(!_engineSource.isPlaying)
            {
                _engineSource.clip = _engineClip;
                _engineSource.Play();
            }

            float speedProp = _controller.m_curSpeed / _controller.m_maxSpeed;
            float pitch = Mathf.Lerp(_lowSpeedPitch, _highSpeedPitch, speedProp);
            _engineSource.pitch = pitch;
        }



    }

    void OnDeath(Vector3 pos)
    {
        _engineSource.Stop();
        _tireScreechSource.Stop();
        _shootSource.Stop();

        _deathSource.clip = _deathClip;
        _deathSource.Play();
    }
}
