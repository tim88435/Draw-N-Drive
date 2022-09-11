using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chunk : ScriptableObject
{
    public enum ChunkID//type of chunk in relation to its neighbors
    {
        single = 0,//if it is a single chunk that doesn't interact with its neighbors
        railroad,//if it's a chunk that's part of the railroad strip of chunks
        walmart,
        park,
    }
    public Transform chunkParent;//parent gameobject that will hold the chunk elements
    public ChunkID chunkID = 0;//type of chunk for this chunk
    public bool isRoadType = false;//if this chunk is part of the road
    [SerializeField] public List<ChunkObject>  objects = new List<ChunkObject>();//list of objects in the chunk
    /// <summary>
    /// Saved object of a chunk
    /// </summary>
    [System.Serializable]
    public struct ChunkObject//struct to hold/save an object in the chunk
    {
        [Tooltip("Object Identification")]
        public int objectID;//position of the prefab in the prefab chunk list used to spawn the object
        //transform values for the object in the scene
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        //constructors for the chunk object
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID">ID of the object in the Prefab Manager</param>
        /// <param name="objectTransform">Local transform of the object</param>
        public ChunkObject(int ID, Transform objectTransform)
        {//for use when you have the objectID and the transform where the object should be in relation to the parent
            objectID = ID;
            position = objectTransform.localPosition;
            rotation = objectTransform.localEulerAngles;
            scale = objectTransform.localScale;
        }
        /// <summary>
        /// Make a new instance of a chunk object based on a previous template
        /// </summary>
        public ChunkObject(ChunkObject chunkObject)
        {//for when you want to create a new instance of the gameobject in the chunk
            objectID = chunkObject.objectID;
            position = new Vector3(chunkObject.position.x, chunkObject.position.y, chunkObject.position.z);
            rotation = new Vector3(chunkObject.rotation.x, chunkObject.rotation.y, chunkObject.rotation.z);
            scale = new Vector3(chunkObject.scale.x, chunkObject.scale.y, chunkObject.scale.z);
        }
    }
    /// <summary>
    /// Instantiate the prefabs of the chunk under the chunk parent
    /// </summary>
    public void SpawnObjects()//spawn the objects in the scene
    {
        if (chunkParent == null)//if the parent is not yet set
        {
            Debug.LogWarning($"Chunk parent not attached");//complain to the Unity user
            return;//go no further since there is no parent to spawn the objects under
        }
        foreach (ChunkObject item in objects)//for each saved object in the saved chunk
        {
            //Instantiate the prefab object, but make sure that it is still considered a prefab
            Transform newGameObjectTransform = (PrefabUtility.InstantiatePrefab(PrefabManager.Singleton.GetObject(item.objectID), chunkParent) as GameObject).transform;
            //set the transform of the object to the saved transform
            newGameObjectTransform.localPosition = item.position;
            newGameObjectTransform.localRotation = Quaternion.Euler(item.rotation);
            newGameObjectTransform.localScale = item.scale;
        }
    }
    /// <summary>
    /// Make a new instance of a chunk
    /// </summary>
    /// <param name="template">Chunk to crate an instance for</param>
    /// <returns></returns>
    public static Chunk NewChunk(Chunk template)//pretty much a constructor for a new chunk, (based on an existing chunk)
    {
        Chunk chunk = CreateInstance<Chunk>();//CreateInstance for scriptable objects (instead of using 'new')
        chunk.chunkID = template.chunkID;//set chunkID to template's chunkID
        chunk.isRoadType = template.isRoadType;//set isRoadType to template's isRoadType
        //add the chunk objects to the new chunk
        for (int i = 0; i < template.objects.Count; i++)
        {
            ChunkObject newObject = new ChunkObject(template.objects[i]);
            chunk.objects.Add(newObject);
        }
        return chunk;
    }
}
