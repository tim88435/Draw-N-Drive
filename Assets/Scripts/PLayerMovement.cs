using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Singleton
    private PlayerMovement _singleton;
    public PlayerMovement Singleton
    {
        get { return _singleton; }
        set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else
            {
                Debug.LogWarning($"{nameof(value)} already exists in the current scene. Deleting clone");
                Destroy(this.gameObject);
            }
        }
    }
    private void OnValidate()
    {
        Singleton = this;
    }
    #endregion
    private float _speed;
    public float Speed
    {
        get { return _speed; }
        set
        {
            _speed = value;
        }
    }
    private int _health;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
        }
    }

}
