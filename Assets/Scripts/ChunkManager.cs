using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
    public List<Chunk> savedChunks = new List<Chunk>();//list of the saved chunks to use to make the level
    #region Editor Menu Items
    [MenuItem("Chunks/Saved Chunks/Clear")]//Removes ALL saved chunks
    public static void ClearSavedChunks()
    {
        Singleton.savedChunks.Clear();
    }
    #endregion
    //2D array of the current chunks where the first list is the x column (e.g. left, road and right), and the second lists are the rows
    private List<List<Chunk>> chunks = new List<List<Chunk>>();
    [SerializeField] private int spawnDistance = 50;//Unity units of how far to spawn chunks
    [SerializeField] private SpawnPositions[] spawnPositions = new SpawnPositions[3];//currently, it is 3 chunk columns
    //struct to store the settings for the chunks
    [System.Serializable]
    public struct SpawnPositions
    {
        public int xAxis;//x-coordinate of the chunk column
        //rotation of the chunk column, so that they are always looking at the middle chunk, and the middle 'road' chunk is looking forward
        public float rotation;
        //Constructor to make it easier to set the positions
        public SpawnPositions(int _xAxis, float _rotation)
        {
            xAxis = _xAxis;
            rotation = _rotation;
        }
    }
    private void Start()
    {
        //sets left x coordinate to -10, and rotation to -90
        //sets left x coordinate to 0, and rotation to 0
        //sets left x coordinate to 10, and rotation to 90
        for (int i = 0; i < spawnPositions.Length; i++)//sets the spawnPositions length to 3
        {
            spawnPositions[i] = new SpawnPositions(10 * i - 10, 90 * i - 90);
            chunks.Add(new List<Chunk>());//add a new column of chunks for each chunk spawn position
        }
        SpawnChunks();//spawns the chunks before the player sees the scene
    }
    private void Update()
    {
        SpawnChunks();//spawns any chunks that need spawning
        DeleteChunks();//deletes any chunks that need deleting
    }
    /// <summary>
    /// Run to spawn any chunks that need to be spawned based on the set spawn distance
    /// </summary>
    private void SpawnChunks()
    {
        if (chunks[0].Count == 0)//if no chunks have been created
        {
            SpawnChunkStrip(-10);//spawn a new strip of chunks behind the player
        }
        //while the forward-most chunks arn't in front of the player enough, keep spawning new chunk strips
        while (Player.Singleton.transform.position.z + spawnDistance > chunks[0][chunks[0].Count - 1].chunkParent.position.z)
        {
            SpawnChunkStrip((int)(chunks[0][chunks[0].Count - 1].chunkParent.position.z + 10));
        }
    }
    /// <summary>
    /// Destorys any chunks that the player will no longer see
    /// </summary>
    private void DeleteChunks()
    {//while the back-most chunks are too far behind the player, keep destroying the back chunk strips
        while (Player.Singleton.transform.position.z - 15 > chunks[0][0].chunkParent.position.z)
        {
            for (int i = 0; i < 3; i++)//for each chunk in the strip
            {
                Destroy(chunks[i][0].chunkParent.gameObject);
                chunks[i].RemoveAt(0);
            }
        }
    }
    /// <summary>
    /// Spawns one chunk
    /// </summary>
    /// <param name="Coordinates">Horizontal coordinates of where to spawn the chunk</param>
    /// <param name="isRoadType">Is this a road chunk?</param>
    /// <param name="type">Type of chunk</param>
    /// <param name="rotation">Rotation of the chunk in euler angles</param>
    /// <returns></returns>
    private Chunk SpawnOneChunk(Vector2Int Coordinates, bool isRoadType, Chunk.ChunkID type, float rotation)
    {
        Chunk newChunk = Chunk.NewChunk(GetChunk(isRoadType));//instansiate a new chunk based on a saved chunk (gotten from the GetChunk generator)
        GameObject chunkParent = new GameObject();//makes a new parent for the chunk
        chunkParent.transform.position = new Vector3(Coordinates.x, 0, Coordinates.y);//moves the parent to the correct chunk location
        chunkParent.transform.Rotate(0, rotation, 0);//rotated the chunk in accordance to the spawn positions
        newChunk.chunkParent = chunkParent.transform;//sets the chunk to reference the parent as the transform to spawn children under
        newChunk.SpawnObjects();//tells the chunk to spawn the chunks' objects
        return newChunk;
    }
    /// <summary>
    /// Spawn one strip of chunks
    /// </summary>
    /// <param name="zCoordinate">Z Coordinate of the strip</param>
    private void SpawnChunkStrip(int zCoordinate)
    {
        //do this for each of the 3 columns
        for (int i = 0; i < 3; i++)
        {
            //adds the chunk to the list depending on which column it's on
            chunks[i].Add(SpawnOneChunk(new Vector2Int(spawnPositions[i].xAxis, zCoordinate), i == 1, Chunk.ChunkID.single, spawnPositions[i].rotation));
        }
    }
    /// <summary>
    /// The random chunk generator, returns a random chunk based on whether it meets the parameters
    /// </summary>
    /// <param name="isRoadType">Is the required chunk a road?</param>
    /// <returns></returns>
    private Chunk GetChunk(bool isRoadType)
    {
        List<Chunk> availableChunks = new List<Chunk>();//make a new list to use to get a random chunk
        for (int i = 0; i < savedChunks.Count; i++)//check each saved chunk
        {
            if (savedChunks[i].isRoadType == isRoadType)//if it meets parameter requirements, add it to the list
            {
                availableChunks.Add(savedChunks[i]);
            }
        }
        if (availableChunks.Count == 0)//if no chunks meet parameters
        {
            Debug.LogWarning($"No chunk available to spawn!");
            return ScriptableObject.CreateInstance<Chunk>();//give back an empty chunk, that will work but spawn no objects
        }
        return availableChunks[Random.Range(0, availableChunks.Count)];//give back a random chunk out of the possible chunks
    }
}