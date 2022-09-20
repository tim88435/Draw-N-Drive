using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Jump Card")]//to be able to make the scriptableObject in the project
public class JumpCard : Card
{
    [Tooltip("The vertical speed applied to the player's velocity")]
    [SerializeField] private float _jumpSpd;
    public override void Play()//plays the card
    {
        Vector3 vel = PlayerMovement.Singleton.Velocity;//get the player's current velocity
        vel.y = _jumpSpd;//apply the jump speed to that vector
        PlayerMovement.Singleton.Velocity = vel;//reapply the new vector back to the player
        Debug.Log("Plays Jump card");
    }
}
