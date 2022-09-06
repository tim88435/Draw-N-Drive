using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabManager: MonoBehaviour
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
    }    private void OnValidate()
    {
        Singleton = this;
    }
    
    #endregion
    /*
    public struct ObjectPrefab
    {
        public int objectID;
        public GameObject prefab;
        public ObjectPrefab(int objectID, GameObject prefab)
        {
            this.objectID = objectID;
            this.prefab = prefab;
        }
    }*/
    //public static List<ObjectPrefab> prefabList = new List<ObjectPrefab>();
    public List<GameObject> prefabList = new List<GameObject>();
    public GameObject GetObject(int index)
    {
        //return prefabList[index].prefab;
        return prefabList[index];
    }
    public bool CheckIfObject(GameObject gameObject, out int ID)
    {
        GameObject prefab;
        if (prefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject))
        {
            if (prefabList.Contains(prefab))
            {
                ID = prefabList.IndexOf(prefab);
                return true;
            }
        }
        ID = 0;
        return false;
    }
    //public static void SaveObject(ObjectPrefab objectPrefab)
    public void SaveObject(GameObject objectPrefab)
    {
        prefabList.Add(objectPrefab);
    }
#if UNITY_EDITOR
    [MenuItem("Chunks/Prefabs/Clear All Prefabs")]
    public static void ClearAllPrefabs()
    {
        Singleton.prefabList.Clear();
    }
#endif
}
