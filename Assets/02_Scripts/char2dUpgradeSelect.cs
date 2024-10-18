using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class char2dUpgradeSelect : MonoBehaviour
{
    public ShopMgr shopmgr;
    // Start is called before the first frame update
    void Start()
    {
        shopmgr = GameObject.Find("ShopMgr").GetComponent<ShopMgr>();
        gameObject.GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnButtonClick()
    {
        shopmgr.selectingUnit(this.gameObject);
    }
}
