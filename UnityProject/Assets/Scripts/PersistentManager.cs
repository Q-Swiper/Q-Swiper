using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentManager : MonoBehaviour
{
    public static PersistentManager Instance { get; private set; }

    public int levelToPlay = 1;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        Time.timeScale = 1;
    }
}
