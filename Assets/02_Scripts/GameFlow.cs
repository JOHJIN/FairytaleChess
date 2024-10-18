using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlow : MonoBehaviour
{
    public GameObject CharacterSelectPanel;

    public GameObject libmgr;
    void Start()
    {
        libmgr = GameObject.Find("LibMgr");
        libmgr.GetComponent<LibMgr>().gameflow = GetComponent<GameFlow>();

        Debug.Log(Convert.ToInt32(libmgr.GetComponent<LibMgr>().normalUnits[1]["���� ��͵�"]));
        if (Convert.ToInt32(libmgr.GetComponent<LibMgr>().normalUnits[1]["���� ��͵�"]) == 5) Debug.Log("dddddd");
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

    public void GoMainBtn()
    {
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    public void LoseGoMain()
    {
        Destroy(libmgr);
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }
    public void GoShopBtn()
    {
        SceneManager.LoadScene(2);
    }
}
