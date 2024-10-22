using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMgr : MonoBehaviour
{
    public GameObject CharacterSelectPanel;
    public GameObject gameFlow;
    public LibMgr library;

    public int charNum = 100;

    public Button strBtn;
    // Start is called before the first frame update
    void Start()
    {
        gameFlow = GameObject.Find("GameFlow");
        library = GameObject.Find("LibMgr").GetComponent<LibMgr>();
        charNum = 100;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void characterSelectBtn()
    {
        CharacterSelectPanel.SetActive(true);
        strBtn.enabled = false;
    }

    public void charSelectNum(int a)
    {
        if (a != charNum)
        {
            charNum = a;
            strBtn.enabled = true;
        }
        else if (a == charNum)
        {
            charNum = 100;
            strBtn.enabled = false;
        }
    }
    public void charcterSelectToStart()
    {
        library.loadPlayerableDate(charNum);
    }
    public void ContinueBtn()
    {
        SceneManager.LoadScene(2);
    }
}
