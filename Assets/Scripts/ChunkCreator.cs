using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Security.Cryptography;

public class ChunkCreator : EditorWindow
{
    [MenuItem("Chunks/Prefabs/Open Chunk Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ChunkCreator)).autoRepaintOnSceneChange = true;
    }
    GameObject chunkObject;
    Chunk chunk;
    private int chunkID;
    List<Chunk.ChunkObject> chunkObjects = new List<Chunk.ChunkObject>();
    
    void OnGUI()
    {
        GUILayout.Label("Chunk", EditorStyles.boldLabel);
        chunkObject = EditorGUILayout.ObjectField(chunkObject, typeof(GameObject), true) as GameObject;
        if (chunkObject == null)
        {
            return;
        }
        if (chunk == null)
        {
            if (GUILayout.Button("New Chunk"))
            {
                chunk = new Chunk();
            }
            chunkID = int.MaxValue;
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
                if (ChunkManager.Singleton.savedChunks.Count <= chunkID)
                {
                    ChunkManager.Singleton.savedChunks.Add(chunk);
                }
                else
                {
                    ChunkManager.Singleton.savedChunks[chunkID] = chunk;
                }
                chunk = new Chunk();
                RefreshObjects();
            }
            EditorGUI.BeginChangeCheck();
            chunkID = EditorGUILayout.IntField("Chunk ID", chunkID);
            if (EditorGUI.EndChangeCheck())
            {
                if (ChunkManager.Singleton.savedChunks.Count > chunkID)
                {
                    ClearObjects();
                    chunk = ChunkManager.Singleton.savedChunks[chunkID];
                    chunk.SpawnObjects();
                }
            }
        }
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
