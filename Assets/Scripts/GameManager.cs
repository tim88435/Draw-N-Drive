using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton

    private static GameManager _singleton;
    public static GameManager Singleton
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
    private void OnValidate()
    {
        Singleton = this;
    }
    #endregion
    [Range(0, 3)]
    [SerializeField] private float _gameSpeed = 1;
    public float GameSpeed
    {
        get { return _gameSpeed; }
        set { _gameSpeed = value; }
    }
}
