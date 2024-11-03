using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteSelectShop : MonoBehaviour
{
    public ShopMgr smgr;
    public Material selectMaterial;
    public Image myImg;
    // Start is called before the first frame update
    void Start()
    {
        smgr = GameObject.Find("ShopMgr").GetComponent<ShopMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        if (smgr.selectMyUnit2D == this.gameObject)
        {
           myImg.material = selectMaterial;
        }
        else { myImg.material = null; }
    }
}
