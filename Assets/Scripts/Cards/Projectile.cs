using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _moveSpd;

    private void Start()
    {
        _moveSpd += PlayerMovement.Singleton.Velocity.z;
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.forward * _moveSpd * Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Player")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
