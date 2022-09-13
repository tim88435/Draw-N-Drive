using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject
{
    public Texture cardTexture;
    public abstract void Play();
}
