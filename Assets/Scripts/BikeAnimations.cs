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
        transform.localRotation = Quaternion.Euler(0, 180 + PlayerMovement.Singleton.Velocity.x * bikeEulerAngleHorizontal * Time.fixedDeltaTime * SpeedRatio(), 0);
    }
    private float SpeedRatio()
    {
        float ratio = 1/lowSpeedTurnRatio - PlayerMovement.Singleton.Velocity.z / (PlayerMovement.Singleton.MaxSpeed * lowSpeedTurnRatio * 1.25f);
        return ratio;
    }
}
