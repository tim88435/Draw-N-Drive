using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private List<ChunkObjects>  obj = new List<ChunkObjects>();
    [System.Serializable]
    public struct ChunkObjects
    {
        [SerializeField] int objectIdentification;
        [SerializeField] GameObject gameObject;
        [SerializeField] Transform transform;
    }
    /*
    public struct AnimatedGameObjects
    {

    }
    */
}
