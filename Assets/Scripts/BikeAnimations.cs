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
    [Tooltip("The angle that the bike will turn in the direction that it is driving")]
    [SerializeField] private AnimationCurve turnAngle;
    [Tooltip("The angle that the bike will 'angle' itself to make turning easier")]
    [SerializeField] private AnimationCurve bikeAngle;
    [Tooltip("The angle that the front wheel and the handles will turn with the player's input")]
    [SerializeField] private AnimationCurve handlesAngle;
    #endregion
    private void FixedUpdate()
    {
        //y is rotating bike in the direction of movement
        //z is rotating the bike to be on an angle          Angle \/                                                                          ratio depending on current speed \/                      Angle \/                                                                          ratio depending on current speed \/
        transform.localRotation = Quaternion.Euler(0, 180 + ReverseAngle(Quaternion.Euler(PlayerMovement.Singleton.Velocity).eulerAngles.x) * turnAngle.Evaluate(PlayerMovement.Singleton.Velocity.z), ReverseAngle(Quaternion.Euler(PlayerMovement.Singleton.Velocity).eulerAngles.x) * bikeAngle.Evaluate(PlayerMovement.Singleton.Velocity.z));
        //front wheel and handle rotation               Angle \/                                                                          ratio depending on current speed \/
        bikeHandles.localRotation = Quaternion.Euler(0, ReverseAngle(Quaternion.Euler(PlayerMovement.Singleton.Velocity).eulerAngles.x) * handlesAngle.Evaluate(PlayerMovement.Singleton.Velocity.z), 0);
        //wheel spin
        bikeFrontWheel.Rotate(Vector3.right, -PlayerMovement.Singleton.Velocity.z * Time.deltaTime * 90 * GameManager.Singleton.GameSpeed, Space.Self);
        bikeBackWheel.Rotate(Vector3.right, -PlayerMovement.Singleton.Velocity.z * Time.deltaTime * 90 * GameManager.Singleton.GameSpeed, Space.Self);
        //Mathf.Pow(50 - speedRatio, (float)System.Math.E) * 0.001f + 15;
    }
    /// <summary>
    /// Changes the angle to be from a 360 degree angle to a 180 to -180 angle
    /// </summary>
    /// <param name="original"></param>
    /// <returns>180 to -180 angle</returns>
    private float ReverseAngle(float original)
    {
        original = original % 360;//makes sure that it's between 360 and -360
        if (original >= 180)//check for over 180
        {
            original -= 360;
        }
        else if (original <= -180)//check for under -180
        {
            original += 360;
        }
        return original;
    }
    private void OnValidate()
    {
        Singleton = this;
    }
    private void OnEnable()
    {
        Singleton = this;
    }
}
