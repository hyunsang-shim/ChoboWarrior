using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btnQuit : MonoBehaviour
{
    public void QuitGame()
    {
        Debug.Log("Gonna Quit This Game!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }


    private void OnApplicationQuit()
    {
        Debug.Log("Saving...");
        DataManager.Instance.SavePlayerData();
        Debug.Log("Saving...Complete!!");
    }
}
