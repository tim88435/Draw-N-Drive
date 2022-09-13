using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChunkCreator : EditorWindow
{
#if UNITY_EDITOR //to make sure that the build maker doesn't throw a fit unsure that 'MenuItem' is
    
    [MenuItem("Chunks/Open Chunk Creator")]
    public static void ShowWindow()
    {
        //makes sure that the chunk objects get refreshed every time something changes in the editor
        //(rather than when you hover over the chunk creator)
        EditorWindow.GetWindow(typeof(ChunkCreator)).autoRepaintOnSceneChange = true;
    }
#endif
    GameObject chunkObject;//reference to the parent gameobject to which to spawn chunk objects in the heirachy
    Chunk chunk;//reference to current chunk that's been worked on
    private int chunkID;//chunk id that the user sets, and the position of the chunk in the chunk manager's list of saved chunks
    private int ChunkID
    {
        get { return chunkID; }
        set { chunkID = Mathf.Max(value, 0);//makes sure that the chunk ID can't go under 0
        }
    }
    List<Chunk.ChunkObject> chunkObjects = new List<Chunk.ChunkObject>();//list of chunk objects under the chunk parent in the heirachy
    
    void OnGUI()
    {
        //GUILayout.Label("Chunk", EditorStyles.boldLabel); //needless?
        chunkObject = EditorGUILayout.ObjectField("Chunk Parent" ,chunkObject, typeof(GameObject), true) as GameObject;//field for game object
        if (chunkObject == null)
        {
            return;//if the user hasn't thrown in the parent yet, stop there
        }
        ChunkID = EditorGUILayout.IntField("Chunk ID", ChunkID);//allow the player to change what chunk ID they're working on
        if (chunk != null)
        {
            chunk.name = EditorGUILayout.TextField("Chunk Name:", chunk.name);
        }
        if (ChunkManager.Singleton.savedChunks.Count > ChunkID)//if the chunk id is less or equal to the number of chunks saved
        {
            if (GUILayout.Button("Load Chunk"))//allow the user to load the saved chunk
            {
                ClearObjects();//remove the children from the parent
                chunk = Chunk.NewChunk(ChunkManager.Singleton.savedChunks[ChunkID]);//set the current chunk as an instance of the saved chunk
                chunk.chunkParent = chunkObject.transform;//updates the new chunk's parent to the current parent
                chunk.SpawnObjects();//spawn the objects in the saved chunk
            }
        }
        if (chunk == null)//if the chunk is not yet set (e.g. when the parent is first attached)
        {
            chunk = CreateInstance<Chunk>();//make a new chunk
            //if we choose to change the chunks back into Components
            /*
            if (GUILayout.Button("New Chunk"))
            {
                chunk = CreateInstance<Chunk>();
            }
            ChunkID = ChunkManager.Singleton.savedChunks.Count;
            */
        }
        else
        {
            if (GUILayout.Button("Clear Chunk"))//allow the user to clear the children from the parent in the unity toolbar
            {
                ClearObjects();
                chunk = CreateInstance<Chunk>();
            }
            GUILayout.Label("Chunk Information");//main fields for the chunk
            chunk.chunkID = (Chunk.ChunkID)EditorGUILayout.EnumPopup("Chunk Type", chunk.chunkID);
            chunk.isRoadType = EditorGUILayout.Toggle("Is This a Road Chunk?", chunk.isRoadType);
            if (chunkObjects.Count != chunkObject.transform.childCount)//if the chunk's objects' amount doesn't match the children of the parent
            {
                RefreshObjects();//update the objects for the chunk
            }
            if (GUILayout.Button("Save Chunk"))
            {
                RefreshObjects();//update the objects for the chunk to make sure no mistakes are made
                Chunk newChunk = Chunk.NewChunk(chunk);
                if (ChunkManager.Singleton.savedChunks.Count <= ChunkID)//if this is a new chunk based on the chunk ID
                {
                    ChunkManager.Singleton.savedChunks.Add(newChunk);//add it to the saved chunks
                }
                else
                {//otherwise update the saved chunk at the ID number with the current chunk
                    ChunkManager.Singleton.savedChunks[ChunkID] = newChunk;
                }
                //QOL?
                ChunkID++;//increment the chunk id to move to the next chunk
                //Unnesesary (next two lines)? \/ \/ \/
                chunk = CreateInstance<Chunk>();//make a new chunk to work with
                RefreshObjects();//update the objects for the chunk

                Repaint();//this is done to update the saved chunks list in the window
            }
        }
        GUILayout.Label("Current Chunks", EditorStyles.boldLabel);
        #region Show The Current Chunks List
        SerializedObject chunkManager = new SerializedObject(ChunkManager.Singleton);//serializes the chunk manager
        SerializedProperty chunkList = chunkManager.FindProperty("savedChunks");//gets the list of saved chunks
        EditorGUILayout.PropertyField(chunkList, true);//shows the saved chunks in the window
        chunkManager.ApplyModifiedProperties();//allows the user to delete saved chunks
        #endregion
    }
    /// <summary>
    /// Destroys all the children of the current chunk's parent
    /// </summary>
    private void ClearObjects()
    {
        while (chunkObject.transform.childCount != 0)//while there are still children
        {
            DestroyImmediate(chunkObject.transform.GetChild(0).gameObject);//destroy them
        }
    }
    /// <summary>
    /// Updates the current chunk objects with the parent's children
    /// </summary>
    private void RefreshObjects()
    {
        chunkObjects.Clear();//clears the current chunk's objects
        for (int i = 0; i < chunkObject.transform.childCount; i++)//checks each child in the parent
        {
            GameObject gameObject = chunkObject.transform.GetChild(i).gameObject;//gets the individual child
            if (PrefabManager.Singleton.CheckIfObject(gameObject, out int ID))//is this a saved prefab?
            {
                //add it to the list, according to it's transform and id in the saved prefab list
                chunkObjects.Add(new Chunk.ChunkObject(ID, chunkObject.transform.GetChild(i)));
            }
            else
            {
                //tell the user that it's not saved as a prefab in the prefab manager with its own prefab ID
                Debug.LogWarning($"Chunk Object {gameObject.name} is not a setup prefab\n Unparenting object...");
                //rather than deletes the child, it makes it an orphan to allow the user to delete it later or add it to the saved prefabs
                gameObject.transform.parent = null;
            }
        }
        chunk.objects = chunkObjects;//sets the chunks' objects as the new list
    }
}
