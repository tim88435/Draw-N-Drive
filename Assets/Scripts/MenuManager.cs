using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void GoToScene(int i)
    {
        if (SceneManager.GetSceneByBuildIndex(i) != null)
        SceneManager.LoadScene(i);
    }

    public void QuitApplication()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

    public static void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
