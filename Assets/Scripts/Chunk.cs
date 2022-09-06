using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public enum ChunkID
    {
        single = 0,
        railroad,
        walmart,
        park,
    }
    public Transform chunkParent;
    public ChunkID chunkID = 0;
    public bool isRoadType = false;
    [SerializeField] public List<ChunkObject>  objects = new List<ChunkObject>();
    [System.Serializable]
    public struct ChunkObject
    {
        [Tooltip("Object Identification")]
        public int objectID;
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public ChunkObject(int ID, Transform objectTransform)
        {
            this.objectID = ID;
            this.position = objectTransform.localPosition;
            this.rotation = objectTransform.localEulerAngles;
            this.scale = objectTransform.localScale;
        }
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
            Transform newGameObjectTransform = GameObject.Instantiate(PrefabManager.Singleton.GetObject(item.objectID), chunkParent).transform;
            newGameObjectTransform.localPosition = item.position;
            newGameObjectTransform.localRotation = Quaternion.Euler(item.rotation);
            newGameObjectTransform.localScale = item.scale;
        }
    }
}
