using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerLife : MonoBehaviour
{
    public UnityEvent<Vector3> OnDeath;

    public bool IsAlive { get; private set; } = true;

    public void Kill(Vector3 _deathPosition)
    {
        IsAlive = false;
        OnDeath.Invoke(_deathPosition);
    }

}
