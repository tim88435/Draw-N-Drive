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
            else if (_singleton != value)
            {
                Debug.LogWarning($"{nameof(value)} already exists in the current scene. Deleting clone");
                Destroy(value.gameObject);
            }
        }
    }
    #endregion
    #region Properties
    [SerializeField] private int _health;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
            switch (_health)
            {
                case 3:
                    particleSystem.Stop();
                    break;
                case 2:
                    particleSystem.Play();
                    settings.startColor = Color.white;
                    break;
                case 1:
                    particleSystem.Play();
                    settings.startColor = Color.black;
                    break;
                default:
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.ExitPlaymode();
#endif
                    break;
            }
        }
    }
    #endregion
    private ParticleSystem.MainModule settings;
    private ParticleSystem particleSystem;
    private void OnValidate()
    {
        Singleton = this;
    }
    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        settings = particleSystem.main;
        //settings.startColor = Color.black;
        Health = 3;
    }
    private void Update()
    {
        //Health = _health;
    }
}
