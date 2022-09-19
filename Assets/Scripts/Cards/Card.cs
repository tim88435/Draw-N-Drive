using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : ScriptableObject
{
    public Texture cardTexture;//children's textures
    public abstract void Play();//refernece to child's methods to be able to play any card
}
