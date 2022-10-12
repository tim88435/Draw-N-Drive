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
    [Range(0,45)][SerializeField] private float bikeEulerAngleVertical, bikeEulerAngleHorizontal, frontWheelEulerAngle, handleEylerAngle;
    [SerializeField] private float lowSpeedTurnRatio;
    #endregion
    private void FixedUpdate()
    {
        transform.localRotation = Quaternion.Euler(0, 180 + PlayerMovement.Singleton.Velocity.x * bikeEulerAngleHorizontal * Time.fixedDeltaTime * SpeedRatio(lowSpeedTurnRatio * lowSpeedTurnRatio), 0);
        bikeHandles.localRotation = Quaternion.Euler(0, PlayerMovement.Singleton.Velocity.x * bikeEulerAngleHorizontal * Time.fixedDeltaTime * SpeedRatio(lowSpeedTurnRatio * lowSpeedTurnRatio * 0.5f), 0);
        bikeFrontWheel.Rotate(Vector3.right, -PlayerMovement.Singleton.Velocity.z * Time.deltaTime * 90 * GameManager.Singleton.GameSpeed, Space.Self);
        bikeBackWheel.Rotate(Vector3.right, -PlayerMovement.Singleton.Velocity.z * Time.deltaTime * 90 * GameManager.Singleton.GameSpeed, Space.Self);
    }
    private float SpeedRatio(float amount)
    {
        float ratio = lowSpeedTurnRatio - PlayerMovement.Singleton.Velocity.z / (PlayerMovement.Singleton.MaxSpeed * (1/lowSpeedTurnRatio) * 1.2f);
        ratio = (PlayerMovement.Singleton.Velocity.z - PlayerMovement.Singleton.MaxSpeed) * (PlayerMovement.Singleton.Velocity.z - PlayerMovement.Singleton.MaxSpeed) * amount + 1;
        return ratio;
    }
}
