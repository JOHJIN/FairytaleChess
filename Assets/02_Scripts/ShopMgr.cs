using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopMgr : MonoBehaviour
{
    public LibMgr libmgr;

    public Text haveMoneyTxt;
    public GameObject selectMyUnit2D;
    public GameObject myUnit2DBass;
    public GameObject UnitupgradePanel;

    public List<GameObject> shopPlayerUnits;
    public Text buyUpgradeUnitTxt;

    public GameObject gggg;
    public GameObject gggg2;
    // Start is called before the first frame update
    void Start()
    {
        libmgr = GameObject.Find("LibMgr").GetComponent<LibMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        haveMoneyTxt.text = libmgr.money + " G";
    }

    public void NextStageBtn()
    {
        libmgr.enermyLoadAndStage();
    }

    public void selectingUnit(GameObject a)
    {
        selectMyUnit2D = a;
    }

    public void IfSelectWellStart()
    {
        gggg = Instantiate(myUnit2DBass, UnitupgradePanel.transform);

        gggg.GetComponent<RectTransform>().anchoredPosition = new Vector3(-300, 120, 0);
        gggg.GetComponent<Units2DData>().minNum = selectMyUnit2D.GetComponent<Units2DData>().minNum;
        gggg.GetComponent<Units2DData>().maxNum = selectMyUnit2D.GetComponent<Units2DData>().maxNum;

        Dictionary<string, object> Dict = libmgr.unitCode[selectMyUnit2D.GetComponent<Units2DData>().my2DCodeNum];

        gggg2 = Instantiate(myUnit2DBass, UnitupgradePanel.transform);
        gggg2.GetComponent<RectTransform>().anchoredPosition = new Vector3(300, 120, 0);
        gggg2.GetComponent<Units2DData>().minNum = (int)Dict["숫자 최소치"] + (int)(Convert.ToSingle(Dict["숫자 최소치 강화"])*(selectMyUnit2D.GetComponent<Units2DData>().upgradeRank+1));
        gggg2.GetComponent<Units2DData>().maxNum = (int)Dict["숫자 최대치"] + (int)(Convert.ToSingle(Dict["숫자최대치 강화"]) * (selectMyUnit2D.GetComponent<Units2DData>().upgradeRank + 1));

        buyUpgradeUnitTxt.text = (125 - Convert.ToInt32(libmgr.unitCode[selectMyUnit2D.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"]) * 15
            + selectMyUnit2D.GetComponent<Units2DData>().upgradeRank * 60).ToString();
    }

    public void buyUnitUpgrade()
    {
        if (libmgr.money >= 125 - Convert.ToInt32(libmgr.unitCode[selectMyUnit2D.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"]) * 15
            + selectMyUnit2D.GetComponent<Units2DData>().upgradeRank * 60)
        {
            libmgr.money -= 125 - Convert.ToInt32(libmgr.unitCode[selectMyUnit2D.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"]) * 15
            + selectMyUnit2D.GetComponent<Units2DData>().upgradeRank * 60;

            var targetUnitData = libmgr.playerUnitsData.Find(unitData => unitData[0] == selectMyUnit2D.GetComponent<Units2DData>().my2DCodeNum &&
            Convert.ToInt32(unitData[1]) == selectMyUnit2D.GetComponent<Units2DData>().upgradeRank);
            if (targetUnitData != null)
            {
                libmgr.playerUnitsData.Remove(targetUnitData);
            }
            libmgr.playerUnitsData.Add(new List<object> { selectMyUnit2D.GetComponent<Units2DData>().my2DCodeNum, selectMyUnit2D.GetComponent<Units2DData>().upgradeRank + 1});
            selectMyUnit2D = null;
            Destroy(gggg);
            Destroy(gggg2);
            buyUpgradeUnitTxt.text = "? G";
        }
    }
}
