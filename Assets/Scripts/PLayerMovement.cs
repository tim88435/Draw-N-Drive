using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Singleton
    private static PlayerMovement _singleton;
    public static PlayerMovement Singleton
    {
        get { return _singleton; }
        set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.LogWarning($"{nameof(value)} already exists in the current scene. Deleting clone");
                Destroy(value.gameObject);
            }
        }
    }
    #endregion
    #region Properties
    [SerializeField] private float _speed;
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
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float gravity = 10f;
    [Range(0.95f,1.05f)]
    [SerializeField] float forgivenessRatio = 1;
    [SerializeField] float sensitivity;
    public float invinsibilitySeconds;
    public float timeSinceLastHit;
    [SerializeField] private float lastFixedUpdatePosition = 0;
    #endregion
    private void Start()
    {
        player = GetComponent<CharacterController>();
        
        //player.slopeLimit = 0;
#if UNITY_EDITOR

#endif
    }
    private void FixedUpdate()
    {
        if (lastFixedUpdatePosition + Speed * forgivenessRatio > transform.position.z)
        {
            if (timeSinceLastHit + invinsibilitySeconds < Time.time)
            {
                Speed = 0;
                Player.Singleton.Health--;
                timeSinceLastHit = Time.time;
            }
        }
        lastFixedUpdatePosition = transform.position.z;
        Speed += acceleration * Time.fixedDeltaTime;
        Speed = Mathf.Clamp(Speed, 0, maxSpeed);
        player.Move(new Vector3(Input.GetAxis("Horizontal") * sensitivity, -gravity * Time.fixedDeltaTime, Speed));
    }
    private void OnValidate()
    {
        Singleton = this;
    }
}
