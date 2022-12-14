using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Singleton
    private static Player _singleton;
    public static Player Singleton
    {
        get { return _singleton; }
        set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)//making sure there is only one instance of this class
            {
                Debug.LogWarning($"{nameof(value)} already exists in the current scene. Deleting clone");
                Destroy(value.gameObject);
            }
        }
    }
    #endregion
    #region Properties
    [Tooltip("Current Health of the player")]
    [Range(0, 10)]
    [SerializeField] private int _health;
    [SerializeField] ProgressBar _progressBar;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            //update the smoke
            switch (_health)
            {
                case 3:
                    _particleSystem.Stop();
                    break;
                case 2:
                    _particleSystem.Play();
                    settings.startColor = Color.white;
                    break;
                case 1:
                    _particleSystem.Play();
                    settings.startColor = Color.black;
                    break;
                default:
                    if (_health <= 0)
                    {
                        MenuManager.ReturnToMainMenu();
                    }
                    break;
            }
        }
    }
    #endregion
    private ParticleSystem.MainModule settings;//reference to the particle system's main variables
    private ParticleSystem _particleSystem;//reference to the partilce system 
    private void OnValidate()
    {
        Singleton = this;//setting this as the only instance
    }
    private void OnEnable()
    {
        Singleton = this;
    }
    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();//sets reference fot the partilce system
        settings = _particleSystem.main;//sets reference fot the partilce system's main variables
        //Health = 3;//sets the main to 3
        _particleSystem.Stop();
        _progressBar.MaxProgress = 1000;
        _progressBar.CurrentProgress = 0;
    }
    private void Update()
    {
        _progressBar.CurrentProgress = transform.position.z;
    }
}
