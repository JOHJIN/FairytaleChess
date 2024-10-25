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
        Units2DData DD = gggg.GetComponent<Units2DData>();
        DD.upgradeRank = selectMyUnit2D.GetComponent<Units2DData>().upgradeRank;
        DD.my2DCodeNum = selectMyUnit2D.GetComponent<Units2DData>().my2DCodeNum;
        DD.unitNameTxt.GetComponent<Text>().text = selectMyUnit2D.GetComponent<Units2DData>().unitNameTxt.GetComponent<Text>().text;
        DD.unitMoveTxt = selectMyUnit2D.GetComponent<Units2DData>().unitMoveTxt;
        DD.unitEffectTxt.GetComponent<Text>().text = selectMyUnit2D.GetComponent<Units2DData>().unitEffectTxt.GetComponent<Text>().text;
        DD.unit2DImage.sprite = selectMyUnit2D.GetComponent<Units2DData>().unit2DImage.sprite;

        gggg.GetComponent<RectTransform>().anchoredPosition = new Vector3(-300, 120, 0);

        Dictionary<string, object> Dict = libmgr.unitCode[selectMyUnit2D.GetComponent<Units2DData>().my2DCodeNum];

        gggg2 = Instantiate(myUnit2DBass, UnitupgradePanel.transform);
        Units2DData EE = gggg2.GetComponent<Units2DData>();
        EE.minNum = (int)Dict["숫자 최소치"] + (int)(Convert.ToSingle(Dict["숫자 최소치 강화"])*(selectMyUnit2D.GetComponent<Units2DData>().upgradeRank+1));
        EE.maxNum = (int)Dict["숫자 최대치"] + (int)(Convert.ToSingle(Dict["숫자최대치 강화"]) * (selectMyUnit2D.GetComponent<Units2DData>().upgradeRank+1));
        EE.moveMaxCount = (int)Dict["이동 횟수"] + (int)(Convert.ToSingle(Dict["이동횟수 강화"]) * (selectMyUnit2D.GetComponent<Units2DData>().upgradeRank+1));

        EE.unitEffectTxt.GetComponent<Text>().text = DD.unitEffectTxt.GetComponent<Text>().text;
        EE.unit2DImage.sprite = DD.unit2DImage.sprite;
        EE.unitNameTxt.GetComponent<Text>().text = Dict["Name"] + " + " + DD.upgradeRank+1;
        EE.upgradeRank = DD.upgradeRank+1;
        EE.unitMoveTxt.GetComponent<Text>().text = "Move : " + EE.moveMaxCount.ToString();
        EE.unitNumTxt.GetComponent<Text>().text = EE.minNum + " ~ " + EE.maxNum;

        gggg2.GetComponent<RectTransform>().anchoredPosition = new Vector3(300, 120, 0);

        buyUpgradeUnitTxt.text = (125 - Convert.ToInt32(libmgr.unitCode[selectMyUnit2D.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"]) * 15
            + selectMyUnit2D.GetComponent<Units2DData>().upgradeRank * 60).ToString();
    }

    public void buyUnitUpgrade()
    {
        if (selectMyUnit2D != null)
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
                libmgr.playerUnitsData.Add(new List<object> { selectMyUnit2D.GetComponent<Units2DData>().my2DCodeNum, selectMyUnit2D.GetComponent<Units2DData>().upgradeRank + 1 });
                selectMyUnit2D = null;
                Destroy(gggg);
                Destroy(gggg2);
                buyUpgradeUnitTxt.text = "? G";
            }
        }
    }

    public void OutUpgradePanel()
    {
        selectMyUnit2D = null;
        Destroy(gggg);
        Destroy(gggg2);
    }
}
