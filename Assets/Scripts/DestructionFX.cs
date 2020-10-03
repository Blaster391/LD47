using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DestructionFX : MonoBehaviour
{
    [SerializeField]
    private bool _destructOnCollision = false;
    [SerializeField]
    private bool _constructed = true;
    [SerializeField]
    private float _lowEndTime = 1.0f;
    [SerializeField]
    private float _highEndTime = 3.0f;

    private float _destructTime = 0.0f;

    private Material _material;
    private Mesh _mesh;

    private Vector3 _collisionPosition = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        
#if UNITY_EDITOR
        _mesh = GetComponent<MeshFilter>().sharedMesh;
        _material = GetComponent<Renderer>().sharedMaterial;
#else
        _mesh = GetComponent<MeshFilter>().mesh;
        _material = GetComponent<Renderer>().material;
#endif
        if (!_constructed)
        {
            _destructTime = _highEndTime;
        }

        var life = GetComponent<PlayerLife>();
        if(life)
        {
            life.OnDeath.AddListener(Destruct);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_constructed)
        {
            if(_destructTime > 0.0f)
            {
                _destructTime -= Time.deltaTime;
                _destructTime = Mathf.Max(_destructTime, 0.0f);
            }
        }
        else
        {
            if (_destructTime < _highEndTime)
            {
                _destructTime += Time.deltaTime;
                _destructTime = Mathf.Min(_destructTime, _highEndTime);
            }
        }

        _material.SetInt("_vertexCount", _mesh.vertexCount);
        _material.SetFloat("_destructTime", _destructTime);
        _material.SetFloat("_lowEndTime", _lowEndTime);
        _material.SetFloat("_highEndTime", _highEndTime);
        _material.SetVector("_collisionPosition", _collisionPosition);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_destructOnCollision)
        {
            Vector3 collisionPoint = transform.position;
            if(collision.contacts.Length > 0)
            {
                collisionPoint = new Vector3();
                foreach (var contact in collision.contacts)
                {
                    collisionPoint += contact.point;
                }
                collisionPoint /= collision.contacts.Length;
            }

            Destruct(collisionPoint);
        }
    }

    public void Construct(Vector3 collisionPosition)
    {
        if(!_constructed)
        {
            _constructed = true;
        }

    }

    public void Destruct(Vector3 collisionPosition)
    {
        if(_constructed)
        {
            _constructed = false;
            _collisionPosition = collisionPosition;
        }
    }
}
