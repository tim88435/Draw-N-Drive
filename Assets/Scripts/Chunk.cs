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
    [SerializeField] private List<ChunkObject>  objects = new List<ChunkObject>();
    [System.Serializable]
    public struct ChunkObject
    {
        [Tooltip("Object Identification")]
        public int objectID;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
    }
    /*
    public struct AnimatedGameObjects
    {

    }
    */
    public void SpawnObjects()
    {
        foreach (ChunkObject item in objects)
        {
            transform.position = item.position;
            transform.rotation = Quaternion.Euler(item.rotation);
            transform.localScale = item.scale;
        }
    }
    private void Start()
    {
        SpawnObjects();
    }
}
