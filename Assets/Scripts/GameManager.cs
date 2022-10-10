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
    [Tooltip("Current multiplier of game speed")]
    [Range(0, 3)]
    [SerializeField] private float _gameSpeed = 1;//the current game speed
    private float _targetGameSpeed;//the game speed to move towards
    private float _slowDuration;//the duration to slow the game for

    public float GameSpeed
    {
        get { return _gameSpeed; }
        set { _gameSpeed = value; }
    }

    public void TriggerSlow(float degree, float duration)
    {
        _targetGameSpeed = degree;
        _slowDuration = duration;
        StopCoroutine("SlowGameSpeed");
        StartCoroutine("SlowGameSpeed");
    }

    IEnumerator SlowGameSpeed()
    {
        while (_gameSpeed != _targetGameSpeed)//while the game has not reached the target speed
        {
            _gameSpeed = Mathf.MoveTowards(_gameSpeed, _targetGameSpeed, Time.deltaTime);//move towards the target speed
            yield return null;//repeat each frame
        }

        yield return new WaitForSeconds(_slowDuration);//wait the duration of the effect

        while (_gameSpeed != 1)//while game speed is not back to normal
        {
            _gameSpeed = Mathf.MoveTowards(_gameSpeed, 1, 0.5f * Time.deltaTime);//move towards normal speed
            yield return null;//repeat each frame
        }
        _targetGameSpeed = 1; //set the target speed back to default
    }
}
