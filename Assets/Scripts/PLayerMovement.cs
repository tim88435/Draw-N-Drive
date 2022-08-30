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
    #endregion
    #region Properties
    private float _speed;
    public float Speed
    {
        get { return _speed; }
        set
        {
            _speed = value;
        }
    }
    #endregion
    #region Variables
    private CharacterController player;
    private float maxSpeed;
    private float MaxAcceleration;
    private Vector3 lastFixedUpdatePosition;
    #endregion
    private void Start()
    {
        
    }
    private void OnValidate()
    {
        Singleton = this;
    }
}
