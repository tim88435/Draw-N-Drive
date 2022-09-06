using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabManager : MonoBehaviour
{
    #region Singleton
    private static PrefabManager _singleton;
    public static PrefabManager Singleton
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
    #endregion
    private void OnValidate()
    {
        Singleton = this;
    }
    public struct ObjectPrefab
    {
        public int objectID;
        public GameObject prefab;
        public ObjectPrefab(int objectID, GameObject prefab)
        {
            this.objectID = objectID;
            this.prefab = prefab;
        }
    }
    private static List<ObjectPrefab> prefabList = new List<ObjectPrefab>();
    public GameObject GetObject(int index)
    {
        return prefabList[index].prefab;
    }
    public void SaveObject(ObjectPrefab objectPrefab)
    {
        prefabList.Add(objectPrefab);
    }
#if UNITY_EDITOR
    [MenuItem("Chunks/Prefabs/Clear All Prefabs")]
    public static void ClearAllPrefabs()
    {
        prefabList.Clear();
    }
#endif
}
