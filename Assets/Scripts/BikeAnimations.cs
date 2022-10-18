using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeAnimations : MonoBehaviour
{
    #region Singleton
    private static BikeAnimations _singleton;
    public static BikeAnimations Singleton
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
    #region Variables
    [SerializeField] private Transform bikeHandles, bikeFrontWheel, bikeBackWheel;
    [Range(0, 90)] [SerializeField] private float bikeEulerAngleVertical;
    [Range(0, 90)] [SerializeField] private float bikeEulerAngleHorizontal;
    [Range(0, 90)] [SerializeField] private float handleEulerAngle;
    private float speedRatio
    {
        get
        {
            return PlayerMovement.Singleton.Velocity.z * 0.02f;
        }
    }
    #endregion
    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(0, 180 + PlayerMovement.Singleton.Velocity.normalized.x * bikeEulerAngleHorizontal, Input.GetAxis("Horizontal") * bikeEulerAngleVertical * speedRatio * speedRatio);
        bikeHandles.localRotation = Quaternion.Euler(0, PlayerMovement.Singleton.Velocity.normalized.x * handleEulerAngle, 0);
        bikeFrontWheel.Rotate(Vector3.right, -PlayerMovement.Singleton.Velocity.z * Time.deltaTime * 90 * GameManager.Singleton.GameSpeed, Space.Self);
        bikeBackWheel.Rotate(Vector3.right, -PlayerMovement.Singleton.Velocity.z * Time.deltaTime * 90 * GameManager.Singleton.GameSpeed, Space.Self);
        //Mathf.Pow(50 - speedRatio, (float)System.Math.E) * 0.001f + 15;
    }
}
