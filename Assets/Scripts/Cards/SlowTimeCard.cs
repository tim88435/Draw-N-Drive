using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Slow Time Card")]//to be able to make the scriptableObject in the project
public class SlowTimeCard : Card
{
    public override void Play()//plays the card
    {
        Debug.Log("Plays Slow Time card");
    }
}
