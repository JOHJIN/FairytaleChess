using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public GameObject CharacterSelectPanel;

    public LibMgr libmgr;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartBtn()
    {
        SceneManager.LoadScene(1);
    }
}
