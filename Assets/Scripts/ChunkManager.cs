using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Chunk;

public class ChunkManager : MonoBehaviour
{
    #region Singleton

    private static ChunkManager _singleton;
    public static ChunkManager Singleton
    {
        get { return _singleton; }
        set
        {
            if (_singleton == null)
            {
                _singleton = value;
            }
            else if (_singleton != value)
            {
                Debug.LogWarning($"{nameof(value)} already exists in the current scene. Deleting clone");
                Destroy(value.gameObject);
            }
        }
    }
    private void OnValidate()
    {
        Singleton = this;
    }

    #endregion
    public List<Chunk> savedChunks = new List<Chunk>();
    #region Editor Menu Items
    [MenuItem("Chunks/Saved Chunks/Clear")]
    public static void ClearSavedChunks()
    {
        Singleton.savedChunks.Clear();
    }
    [MenuItem("Chunks/Saved Chunks/Debug List")]
    public static void DebugSavedChunks()
    {
        Debug.Log($"Saved Chunks Amount: {Singleton.savedChunks.Count}");
        foreach (Chunk item in Singleton.savedChunks)
        {
            Debug.Log($"Objects in chunk: {item.objects.Count}");
        }
    }
    #endregion
    private List<List<Chunk>> chunks = new List<List<Chunk>>();
    [SerializeField] private int spawnDistance = 50;
    private void Start()
    {
        chunks.Add(new List<Chunk>());
        chunks.Add(new List<Chunk>());
        chunks.Add(new List<Chunk>());
        SpawnChunks();
    }
    private void Update()
    {
        SpawnChunks();
        DeleteChunks();
    }
    private void SpawnChunks()
    {
        if (chunks[0].Count == 0)
        {
            List<Chunk> newChunks = SpawnChunkStrip(-10);
            chunks[0].Add(newChunks[0]);
            chunks[1].Add(newChunks[1]);
            chunks[2].Add(newChunks[2]);
        }
        while (Player.Singleton.transform.position.z + spawnDistance > chunks[0][chunks[0].Count - 1].chunkParent.position.z)
        {
            List<Chunk> newChunks = SpawnChunkStrip((int)(chunks[0][chunks[0].Count - 1].chunkParent.position.z + 10));
            chunks[0].Add(newChunks[0]);
            chunks[1].Add(newChunks[1]);
            chunks[2].Add(newChunks[2]);
        }
    }
    private void DeleteChunks()
    {
        while (Player.Singleton.transform.position.z - 15 > chunks[0][0].chunkParent.position.z)
        {
            for (int i = 0; i < 3; i++)
            {
                Destroy(chunks[i][0].chunkParent.gameObject);
                chunks[i].RemoveAt(0);
            }
        }
    }
    private Chunk SpawnOneChunk(Vector2Int Coordinates, bool isRoadType, ChunkID type, float rotation)
    {
        Chunk newChunk = SetChunk(GetChunk(isRoadType));
        GameObject chunkParent = new GameObject();
        chunkParent.transform.position = new Vector3(Coordinates.x, 0, Coordinates.y);
        chunkParent.transform.Rotate(0, rotation, 0);
        newChunk.chunkParent = chunkParent.transform;
        newChunk.SpawnObjects();
        return newChunk;
    }
    private List<Chunk> SpawnChunkStrip(int zCoordinate)
    {
        List<Chunk> chunkStrip = new List<Chunk>();
        chunkStrip.Add(SpawnOneChunk(new Vector2Int(-10, zCoordinate), false, ChunkID.single, 90));
        chunkStrip.Add(SpawnOneChunk(new Vector2Int(0, zCoordinate), true, ChunkID.single, 0));
        chunkStrip.Add(SpawnOneChunk(new Vector2Int(10, zCoordinate), false, ChunkID.single, -90));
        return chunkStrip;
    }
    private Chunk GetChunk(bool isRoadType)
    {
        List<Chunk> availableChunks = new List<Chunk>();
        for (int i = 0; i < savedChunks.Count; i++)
        {
            if (savedChunks[i].isRoadType == isRoadType)
            {
                availableChunks.Add(savedChunks[i]);
            }
        }
        if (availableChunks.Count == 0)
        {
            Debug.LogWarning($"No chunk available to spawn!");
            return ScriptableObject.CreateInstance<Chunk>();
        }
        return availableChunks[Random.Range(0, availableChunks.Count)];
    }
}