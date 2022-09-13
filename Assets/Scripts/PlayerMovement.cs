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
    [SerializeField] private float _speed;//current speed of the player
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
    private CharacterController player;//character controller reference
    [Tooltip("Maximum speed the player can travel at in unity units per second")]
    [SerializeField] private float maxSpeed;
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
    private float timeSinceLastHit;//time since the player has last hit an object and lost health
    private float lastFixedUpdatePosition = 0;//horizontal position that the player was at the last fixed update
    #endregion
    private void Start()
    {
        player = GetComponent<CharacterController>();//gets the character controller on the player game object
    }
    private void FixedUpdate()
    {
        if (lastFixedUpdatePosition + Speed * forgivenessRatio > transform.position.z)//if the player has hit something
        {
            if (timeSinceLastHit + invinsibilitySeconds < Time.time)//if the player does not have invincibility frames
            {
                Speed = 0;//set the player's speed to 0
                Player.Singleton.Health--;//reduce the player's health
                //sets the time since the player has last lit an object to give the player invinsibility
                timeSinceLastHit = Time.time;
            }
        }
        lastFixedUpdatePosition = transform.position.z;//updates the position to use when checking decelerattion
        Speed += acceleration * Time.fixedDeltaTime;//increases the speed based on the set acceleration
        //makes sure the player isn't moving back, and isn't going faster than they should be
        Speed = Mathf.Clamp(Speed, 0, maxSpeed);
        //moves the player forward, horizontally based on input and down based on gravity
        player.Move(new Vector3(Input.GetAxis("Horizontal") * sensitivity, -gravity * Time.fixedDeltaTime, Speed));
    }
    private void OnValidate()
    {
        Singleton = this;//sets this to be the only instance
    }
}
