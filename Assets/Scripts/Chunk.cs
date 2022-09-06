using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public enum ChunkID
    {
        single = 0,
        strip,
        block,
    }
    public ChunkID chunkID = 0;
    public bool isRoadType = false;
    [SerializeField] private List<ChunkObjects>  obj = new List<ChunkObjects>();
    [System.Serializable]
    public struct ChunkObjects
    {
        [SerializeField] int objectIdentification;
        [SerializeField] public Transform transform;
    }
    /*
    public struct AnimatedGameObjects
    {

    }
    */
    private void Start()
    {
        Debug.Log(obj[0].transform.);
    }
}
