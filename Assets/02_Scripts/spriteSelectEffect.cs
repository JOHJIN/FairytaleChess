using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteSelectEffect : MonoBehaviour
{
    public MenuMgr mmgr;
    public Material selectMaterial;
    public int myNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mmgr.charNum == myNum)
        {
            gameObject.GetComponent<Image>().material = selectMaterial;
        }
        else { gameObject.GetComponent<Image>().material = null; }
    }
}
