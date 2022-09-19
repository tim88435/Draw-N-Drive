using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Destory Card")]//to be able to make the scriptableObject in the project
public class DestoryCard : Card
{
    public override void Play()//plays the card
    {
        Debug.Log("Plays Destroy card");
    }
}
