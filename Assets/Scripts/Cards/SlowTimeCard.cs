using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Slow Time Card")]//to be able to make the scriptableObject in the project
public class SlowTimeCard : Card
{
    [Tooltip("The duration of the slow effect")]
    [SerializeField] private float _slowDuration;
    [Tooltip("The speed to set the game to - between 0 and 3")]
    [SerializeField] private float _newGameSpeed;
    public override void Play()//plays the card
    {
        GameManager.Singleton.TriggerSlow(_newGameSpeed,_slowDuration);
        Debug.Log("Plays Slow Time card");
    }

}
