using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChunkCreator : EditorWindow
{
    [MenuItem("Chunks/Open Chunk Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ChunkCreator)).autoRepaintOnSceneChange = true;
    }
    GameObject chunkObject;
    Chunk chunk;
    [Range (0f, 1f)] private int chunkID;
    List<Chunk.ChunkObject> chunkObjects = new List<Chunk.ChunkObject>();
    
    void OnGUI()
    {
        GUILayout.Label("Chunk", EditorStyles.boldLabel);
        chunkObject = EditorGUILayout.ObjectField("Chunk Parent" ,chunkObject, typeof(GameObject), true) as GameObject;
        if (chunkObject == null)
        {
            return;
        }
        chunkID = EditorGUILayout.IntField("Chunk ID", chunkID);
        if (ChunkManager.Singleton.savedChunks.Count > chunkID)
        {
            if (GUILayout.Button("Load Chunk"))
            {
                ClearObjects();
                chunk = new Chunk(ChunkManager.Singleton.savedChunks[chunkID]);
                chunk.chunkParent = chunkObject.transform;
                chunk.SpawnObjects();
            }
        }
        if (chunk == null)
        {
            if (GUILayout.Button("New Chunk"))
            {
                chunk = new Chunk();
            }
            chunkID = ChunkManager.Singleton.savedChunks.Count;
        }
        else
        {
            if (GUILayout.Button("Clear Chunk"))
            {
                ClearObjects();
                chunk = new Chunk();
            }
            GUILayout.Label("Chunk Information");
            chunk.chunkID = (Chunk.ChunkID)EditorGUILayout.EnumPopup("Chunk Type", chunk.chunkID);
            chunk.isRoadType = EditorGUILayout.Toggle("Is This a Road Chunk?", chunk.isRoadType);
            if (chunkObjects.Count != chunkObject.transform.childCount)
            {
                RefreshObjects();
            }
            //Debug.Log($"chunkObjects.Count: {chunkObjects.Count}\n chunkObject.transform.childCount: {chunkObject.transform.childCount}");
            if (GUILayout.Button("Save Chunk"))
            {
                RefreshObjects();
                Chunk newChunk = new Chunk(chunk);
                if (ChunkManager.Singleton.savedChunks.Count <= chunkID)
                {
                    ChunkManager.Singleton.savedChunks.Add(newChunk);
                }
                else
                {
                    ChunkManager.Singleton.savedChunks[chunkID] = newChunk;
                }
                chunkID++;
                chunk = new Chunk();
                RefreshObjects();
                Repaint();
            }
        }
        GUILayout.Label("Current Chunks", EditorStyles.boldLabel);
        #region Show The Current Chunks List
        SerializedObject chunkManager = new SerializedObject(ChunkManager.Singleton);
        SerializedProperty chunkList = chunkManager.FindProperty("savedChunks");
        EditorGUILayout.PropertyField(chunkList, true);
        chunkManager.ApplyModifiedProperties();
        #endregion
        /*
        GUILayout.Label("Current Prefabs", EditorStyles.boldLabel);
        #region Show The Current Prefabs List
        SerializedObject prefabManager = new SerializedObject(PrefabManager.Singleton);
        SerializedProperty prefabList = prefabManager.FindProperty("prefabList");
        EditorGUILayout.PropertyField(prefabList, true);
        prefabManager.ApplyModifiedProperties();
        #endregion*/
    }
    private void ClearObjects()
    {
        while (chunkObject.transform.childCount != 0)
        {
            DestroyImmediate(chunkObject.transform.GetChild(0).gameObject);
        }
    }
    private void RefreshObjects()
    {
        chunkObjects.Clear();
        for (int i = 0; i < chunkObject.transform.childCount; i++)
        {
            GameObject gameObject = chunkObject.transform.GetChild(i).gameObject;
            if (PrefabManager.Singleton.CheckIfObject(gameObject, out int ID))
            {
                chunkObjects.Add(new Chunk.ChunkObject(ID, chunkObject.transform.GetChild(i)));
            }
            else
            {
                Debug.LogWarning($"Chunk Object {gameObject.name} is not a setup prefab\n Unparenting object...");
                gameObject.transform.parent = null;
            }
        }
        chunk.objects = chunkObjects;
    }
}
