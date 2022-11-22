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
    }
    private void OnValidate()
    {
        Singleton = this;
    }
    private void OnEnable()
    {
        Singleton = this;
    }

    #endregion
    [SerializeField] private List<GameObject> prefabList = new List<GameObject>();//list of saved object prefabs
    /// <summary>
    /// Gets the GameObject prefab in the prefab list based on the position in the Prefab List
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject GetObject(int index)
    {
        return prefabList[index];
    }
#if UNITY_EDITOR
    /// <summary>
    /// Returns if the gameobject is a prefab, and the prefab ID in the saved prefab list
    /// </summary>
    /// <param name="gameObject">Object to check if it is a prefab</param>
    /// <param name="ID">Object ID in the prefab list</param>
    /// <returns>Is the gameobject a prefab?</returns>
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
#endif
    /*
#if UNITY_EDITOR
    [MenuItem("Chunks/Prefabs/Clear All Prefabs")]
    public static void ClearAllPrefabsAndChunks()//removes ALL the saved prefabs
    {
        Singleton.prefabList.Clear();
        //Removes LL the saved chunks to make sure that there are no null references when making chunks
        ChunkManager.ClearSavedChunks();
    }
#endif
    */
}
