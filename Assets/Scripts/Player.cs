using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region Singleton
    private Player _singleton;
    public Player Singleton
    {
        get { return _singleton; }
        set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else
            {
                Debug.LogWarning($"{nameof(value)} already exists in the current scene. Deleting clone");
                Destroy(this.gameObject);
            }
        }
    }
    #endregion
    #region Properties
    private int _health;
    public int Health
    {
        get { return _health; }
        set
        {
            _health = value;
        }
    }
    #endregion
    private void OnValidate()
    {
        Singleton = this;
    }
}
