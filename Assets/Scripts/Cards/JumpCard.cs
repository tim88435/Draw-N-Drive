using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Cards/Jump Card")]
public class JumpCard : Card
{
    public override void Play()
    {
        Player.Singleton.transform.Translate(0, 10, 0);
    }
}
