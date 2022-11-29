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
    [SerializeField] private Vector3 _velocity;//current velosity of the player
    public Vector3 Velocity
    {
        get { return _velocity; }
        set
        {
            _velocity = value;
        }
    }
    [Tooltip("Maximum speed the player can travel at in unity units per second")]
    [SerializeField] private float _maxSpeed;
    public float MaxSpeed
    {
        get { return _maxSpeed; }
        set { _maxSpeed = value; }
    }
    [Tooltip("The position to spawn a projectile from")]
    [SerializeField] private Transform _bulletSpawn;
    public Transform BulletSpawn { get => _bulletSpawn; }
    #endregion
    #region Variables
    private CharacterController player;//character controller reference
    [Tooltip("Acceleration that the player will speed up by in unity units per second squared")]
    [SerializeField] private float acceleration;
    [Tooltip("Gravity to apply to the player in unity units per second squared")]
    [SerializeField] private float gravity = 10f;
    [Tooltip("Ratio by how much the player would be forgiven when the player has decelerated (e.g. bumped into an object)")]
    [Range(0f,1f)]
    [SerializeField] private float forgivenessRatio = 1;
    [Tooltip("Speed by which the player can move left or right")]
    [SerializeField] private float sensitivity;
    [Tooltip("Seconds of invincibility the player is given when they hit an object")]
    [SerializeField] private float invinsibilitySeconds;
    [Tooltip("Maximum upward velocity during which the player is safe from collision")]
    [SerializeField] private float jumpSafety = 0.8f;
    
    private float timeSinceLastHit;//time since the player has last hit an object and lost health
    private float lastFixedUpdatePosition = 0;//horizontal position that the player was at the last fixed update
    #endregion
    private void Start()
    {
        player = GetComponent<CharacterController>();//gets the character controller on the player game object
    }
    private void FixedUpdate()
    {
        //increases the speed based on the set acceleration and adds gravity
        Velocity += new Vector3(0, -gravity * GameManager.Singleton.GameSpeed, acceleration);
        //makes sure the player isn't moving back, and isn't going faster than they should be,
        //and resets the vertical velosity if the playeris on the ground
        Velocity = new Vector3 (Input.GetAxis("Horizontal") * sensitivity, Velocity.y <= 0 && player.isGrounded ? 0 : Velocity.y, Mathf.Clamp(Velocity.z, 0, MaxSpeed));
        //moves the player forward, horizontally based on input
        player.Move(Velocity * GameManager.Singleton.GameSpeed * Time.fixedDeltaTime);
        timeSinceLastHit += Time.fixedDeltaTime * GameManager.Singleton.GameSpeed;
        if (Velocity.y <= jumpSafety && lastFixedUpdatePosition + Velocity.z * forgivenessRatio * GameManager.Singleton.GameSpeed * Time.fixedDeltaTime > transform.position.z && GameManager.Singleton.GameSpeed > 0)//if the player has hit something
        {
            if (timeSinceLastHit > invinsibilitySeconds)//if the player does not have invincibility frames
            {
                Velocity = Vector3.zero;//set the player's speed to 0
                Player.Singleton.Health--;//reduce the player's health
                //sets the time since the player has last lit an object to give the player invinsibility
                timeSinceLastHit = 0;
            }
        }
        lastFixedUpdatePosition = transform.position.z;//updates the position to use when checking deceleration
    }
    private void OnValidate()
    {
        Singleton = this;//sets this to be the only instance
    }
    private void OnEnable()
    {
        Singleton = this;
    }
}
