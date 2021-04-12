using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1)]
public class GameManager : MonoBehaviour
{
    public int Score;
    
    

    #region Singleton

    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
                Debug.LogError("Game Manager is NULL");
            return _instance;
        }
    }

    #endregion
    
   

    private void Awake()
    { 
        if(_instance != null)
            Destroy(gameObject);
        else
        {
            _instance = this;
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void DebugButton()
    {
        PointGrid.Instance.CheckAndDestroyIfHasMatch();
        PointGrid.Instance.HasMatch = false;
    }
    
}
