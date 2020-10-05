using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class DestructionFX : MonoBehaviour
{
    [SerializeField]
    private bool _destroyObjectOnDestruction = false;
    [SerializeField]
    private bool _destructOnCollision = false;
   
    [SerializeField]
    public bool _constructed = true;
    [SerializeField]
    private float _lowEndTime = 1.0f;
    [SerializeField]
    private float _highEndTime = 3.0f;
    [SerializeField]
    private float _shrinkDelay = 0.5f;

    [SerializeField]
    private float _burstSizeMax = 2.0f;
    [SerializeField]
    private float _burstSizeMin = 1.0f;
    [SerializeField]
    private float _burstTime = 1.0f;

    [SerializeField]
    private bool _useShader = false;

    private float _destructTime = 0.0f;

    private bool _trackKillerObject = false;
    private GameObject _killerObject = null;

    private Material _material;
    private Mesh _mesh;

    private Vector3 _collisionPosition = new Vector3();
    private Vector3 _deathPosition = new Vector3();
    private Vector3 _deathScale = new Vector3();

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
        if(!life)
        {
            life = GetComponentInParent<PlayerLife>();
        }

        if(life)
        {
            life.OnDeath.AddListener(Destruct);
        }
    }

    // Update is called once per frame
    void Update()
    {

        float maxTime = (_highEndTime + _shrinkDelay);
        if (_constructed)
        {
            if(_destructTime > 0.0f)
            {
                _destructTime -= Time.deltaTime;
                _destructTime = Mathf.Max(_destructTime, 0.0f);
            }
        }
        else
        {
            
            if (_destructTime < maxTime)
            {
                _destructTime += Time.deltaTime;
                _destructTime = Mathf.Min(_destructTime, maxTime);
            }
            else if(_destroyObjectOnDestruction)
            {
                Destroy(gameObject);
            }
        }

        if (_trackKillerObject && _killerObject)
        {
            _collisionPosition = _killerObject.transform.position;
        }

        if (_useShader)
        {
            _material.SetInt("_vertexCount", _mesh.vertexCount);
            _material.SetFloat("_destructTime", _destructTime);
            _material.SetFloat("_lowEndTime", _lowEndTime);
            _material.SetFloat("_highEndTime", _highEndTime);

            _material.SetVector("_collisionPosition", _collisionPosition);

            _material.SetFloat("_burstSizeMax", _burstSizeMax);
            _material.SetFloat("_burstSizeMin", _burstSizeMin);
            _material.SetFloat("_burstTime", _burstTime);

            _material.SetFloat("_shrinkDelay", _shrinkDelay);
        }
        else if(!_constructed)
        {
            float positionProp = 1.0f;
            if(_lowEndTime > 0.0f)
            {
                positionProp = _destructTime / _lowEndTime;
                positionProp = Mathf.Min(positionProp, 1.0f);
            }

            float prop = _destructTime / maxTime;
            transform.position = Vector3.Lerp(_deathPosition, _collisionPosition, positionProp);
            transform.localScale = Vector3.Lerp(_deathScale, new Vector3(), prop);
        }
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

            _deathPosition = transform.position;
            _deathScale = transform.localScale;
        }
    }

    public void Destruct(GameObject destroyer)
    {
        if (_constructed)
        {
            _constructed = false;
            _collisionPosition = destroyer.transform.position;
            _killerObject = destroyer;
            _trackKillerObject = true;

            _deathPosition = transform.position;
            _deathScale = transform.localScale;
        }
    }
}
