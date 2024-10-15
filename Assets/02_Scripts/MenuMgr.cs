using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuMgr : MonoBehaviour
{
    public GameObject CharacterSelectPanel;
    public LibMgr library;

    public int charNum = 100;

    public Button strBtn;
    // Start is called before the first frame update
    void Start()
    {
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
}
