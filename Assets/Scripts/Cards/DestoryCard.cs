using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Destory Card")]//to be able to make the scriptableObject in the project
public class DestoryCard : Card
{
    [SerializeField] private GameObject _projectilePrefab;
    public override void Play()//plays the card
    {
        Instantiate(_projectilePrefab, PlayerMovement.Singleton.BulletSpawn.position, Quaternion.identity, null);
        Debug.Log("Plays Destroy card");
    }
}
