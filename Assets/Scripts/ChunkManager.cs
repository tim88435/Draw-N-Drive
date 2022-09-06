using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

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
}