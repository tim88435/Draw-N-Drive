using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chunk : ScriptableObject
{
    public Chunk(Chunk chunk)
    {
        chunkID = chunk.chunkID;
        isRoadType = chunk.isRoadType;
        for (int i = 0; i < chunk.objects.Count; i++)
        {
            ChunkObject newObject = new ChunkObject(chunk.objects[i]);
            objects.Add(newObject);
        }
    }
    public Chunk()
    {
        chunkID = ChunkID.single;
        isRoadType = false;
        objects = new List<ChunkObject>();
    }
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
            objectID = ID;
            position = objectTransform.localPosition;
            rotation = objectTransform.localEulerAngles;
            scale = objectTransform.localScale;
        }
        public ChunkObject(int ID, Vector3 localPosition, Vector3 localEulerAngles, Vector3 localScale)
        {
            objectID = ID;
            position = localPosition;
            rotation = localEulerAngles;
            scale = localScale;
        }
        public ChunkObject(ChunkObject chunkObject)
        {
            objectID = chunkObject.objectID;
            position = new Vector3(chunkObject.position.x, chunkObject.position.y, chunkObject.position.z);
            rotation = new Vector3(chunkObject.rotation.x, chunkObject.rotation.y, chunkObject.rotation.z);
            scale = new Vector3(chunkObject.scale.x, chunkObject.scale.y, chunkObject.scale.z);
        }
    }
    public void SpawnObjects()
    {
        if (chunkParent == null)
        {
            Debug.LogWarning($"Chunk parent not attached");
            return;
        }
        foreach (ChunkObject item in objects)
        {
            Transform newGameObjectTransform = (PrefabUtility.InstantiatePrefab(PrefabManager.Singleton.GetObject(item.objectID), chunkParent) as GameObject).transform;
            newGameObjectTransform.localPosition = item.position;
            newGameObjectTransform.localRotation = Quaternion.Euler(item.rotation);
            newGameObjectTransform.localScale = item.scale;
        }
    }
}
