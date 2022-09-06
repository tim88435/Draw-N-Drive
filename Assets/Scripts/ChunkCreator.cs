using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ChunkCreator : EditorWindow
{
    [MenuItem("Chunks/Prefabs/Open Chunk Creator")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ChunkCreator));
    }
    GameObject chunkObject;
    Chunk chunk;
    void OnGUI()
    {
        GUILayout.Label("Chunk Creator", EditorStyles.boldLabel);
        chunkObject = EditorGUILayout.ObjectField(chunkObject, typeof(GameObject), true) as GameObject;
        if (chunkObject == null)
        {
            return;
        }
        if (chunk == null)
        {
            if (!(chunk = chunkObject.GetComponent<Chunk>()))
            {
                if (GUILayout.Button("New Chunk"))
                {
                    chunk = chunkObject.AddComponent<Chunk>();
                }
            }
        }
        else
        {
            if (GUILayout.Button("New Chunk"))
            {
                while (chunkObject.transform.childCount != 0)
                {
                    DestroyImmediate(chunkObject.transform.GetChild(0).gameObject);
                }
                chunk = chunkObject.AddComponent<Chunk>();
            }
        }
    }
}
