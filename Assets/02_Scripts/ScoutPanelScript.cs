using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoutPanelScript : MonoBehaviour
{
    public LibMgr libmgr;
    public ShopMgr shopmgr;

    public List<object> rareNum5 = new List<object>();
    public List<object> rareNum4 = new List<object>();
    public List<object> rareNum3 = new List<object>();
    public List<object> rareNum2 = new List<object>();
    public List<object> rareNum1 = new List<object>();

    public object UnitCode2DScout1;
    public object UnitCode2DScout2;
    public object UnitCode2DScout3;

    public GameObject howMuch1;
    public GameObject howMuch2;
    public GameObject howMuch3;

    public GameObject imge1;
    public GameObject imge2;
    public GameObject imge3;

    public GameObject character2DShop; //340size
    // Start is called before the first frame update
    void Start()
    {
        libmgr = GameObject.Find("LibMgr").GetComponent<LibMgr>();

        for (int i = 0; i < libmgr.normalUnits.Count; i++)
        {
            if (Convert.ToInt32(libmgr.normalUnits[i]["영입 희귀도"]) == 5) rareNum5.Add(libmgr.normalUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.normalUnits[i]["영입 희귀도"]) == 4) rareNum5.Add(libmgr.normalUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.normalUnits[i]["영입 희귀도"]) == 3) rareNum5.Add(libmgr.normalUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.normalUnits[i]["영입 희귀도"]) == 2) rareNum5.Add(libmgr.normalUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.normalUnits[i]["영입 희귀도"]) == 1) rareNum5.Add(libmgr.normalUnits[i]["ID"]);
            else { }
        }
        for (int i = 0; i < libmgr.otherUnits.Count; i++)
        {
            if (Convert.ToInt32(libmgr.otherUnits[i]["영입 희귀도"]) == 5) rareNum5.Add(libmgr.otherUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.otherUnits[i]["영입 희귀도"]) == 4) rareNum5.Add(libmgr.otherUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.otherUnits[i]["영입 희귀도"]) == 3) rareNum5.Add(libmgr.otherUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.otherUnits[i]["영입 희귀도"]) == 2) rareNum5.Add(libmgr.otherUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.otherUnits[i]["영입 희귀도"]) == 1) rareNum5.Add(libmgr.otherUnits[i]["ID"]);
            else { }
        }

        int rnd1 = UnityEngine.Random.Range(0, rareNum5.Count + rareNum4.Count + rareNum3.Count + rareNum2.Count + rareNum3.Count);

        if (rnd1 < rareNum5.Count) { UnitCode2DScout1 = rareNum5[rnd1]; }
        else if (rareNum5.Count <= rnd1 && rnd1 < rareNum4.Count) { UnitCode2DScout1 = rareNum4[rnd1 - rareNum5.Count]; }
        else if (rareNum4.Count <= rnd1 && rnd1 < rareNum3.Count) { UnitCode2DScout1 = rareNum3[rnd1 - rareNum4.Count - rareNum5.Count]; }
        else if (rareNum3.Count <= rnd1 && rnd1 < rareNum2.Count) { UnitCode2DScout1 = rareNum2[rnd1 - rareNum3.Count - rareNum4.Count - rareNum5.Count]; }
        else if (rareNum2.Count <= rnd1) { UnitCode2DScout1 = rareNum4[rnd1 - rareNum2.Count - rareNum3.Count - rareNum4.Count - rareNum5.Count]; }

        int rnd2 = UnityEngine.Random.Range(0, rareNum5.Count + rareNum4.Count + rareNum3.Count + rareNum2.Count + rareNum3.Count);

        if (rnd2 < rareNum5.Count) { UnitCode2DScout2 = rareNum5[rnd2]; }
        else if (rareNum5.Count <= rnd2 && rnd2 < rareNum4.Count) { UnitCode2DScout2 = rareNum4[rnd2 - rareNum5.Count]; }
        else if (rareNum4.Count <= rnd2 && rnd2 < rareNum3.Count) { UnitCode2DScout2 = rareNum3[rnd2 - rareNum4.Count - rareNum5.Count]; }
        else if (rareNum3.Count <= rnd2 && rnd2 < rareNum2.Count) { UnitCode2DScout2 = rareNum2[rnd2 - rareNum3.Count - rareNum4.Count - rareNum5.Count]; }
        else if (rareNum2.Count <= rnd2) { UnitCode2DScout2 = rareNum4[rnd2 - rareNum2.Count - rareNum3.Count - rareNum4.Count - rareNum5.Count]; }

        int rnd3 = UnityEngine.Random.Range(0, rareNum5.Count + rareNum4.Count + rareNum3.Count + rareNum2.Count + rareNum3.Count);

        if (rnd3 < rareNum5.Count) { UnitCode2DScout3 = rareNum5[rnd3]; }
        else if (rareNum5.Count <= rnd3 && rnd3 < rareNum4.Count) { UnitCode2DScout3 = rareNum4[rnd3 - rareNum5.Count]; }
        else if (rareNum4.Count <= rnd3 && rnd3 < rareNum3.Count) { UnitCode2DScout3 = rareNum3[rnd3 - rareNum4.Count - rareNum5.Count]; }
        else if (rareNum3.Count <= rnd3 && rnd3 < rareNum2.Count) { UnitCode2DScout3 = rareNum2[rnd3 - rareNum3.Count - rareNum4.Count - rareNum5.Count]; }
        else if (rareNum2.Count <= rnd3) { UnitCode2DScout3 = rareNum4[rnd3 - rareNum2.Count - rareNum3.Count - rareNum4.Count - rareNum5.Count]; }

        imge1 = Instantiate(character2DShop,gameObject.transform);
        imge1.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, 100);
        imge1.GetComponent<Units2DData>().my2DCodeNum = UnitCode2DScout1;
        imge1.GetComponent<Units2DData>().upgradeRank = UnityEngine.Random.Range(0, libmgr.stageLevelCount+1);
        Dictionary<string, object> Dictimge1 = libmgr.unitCode[imge1.GetComponent<Units2DData>().my2DCodeNum];
        imge1.GetComponent<Units2DData>().minNum = (int)Dictimge1["숫자 최소치"] + (int)(Convert.ToSingle(Dictimge1["숫자 최소치 강화"]) * imge1.GetComponent<Units2DData>().upgradeRank);
        imge1.GetComponent<Units2DData>().maxNum = (int)Dictimge1["숫자 최대치"] + (int)(Convert.ToSingle(Dictimge1["숫자최대치 강화"]) * imge1.GetComponent<Units2DData>().upgradeRank);

        imge2 = Instantiate(character2DShop, gameObject.transform);
        imge2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
        imge2.GetComponent<Units2DData>().my2DCodeNum = UnitCode2DScout2;
        imge2.GetComponent<Units2DData>().upgradeRank = UnityEngine.Random.Range(0, libmgr.stageLevelCount + 1);
        Dictionary<string, object> Dictimge2 = libmgr.unitCode[imge2.GetComponent<Units2DData>().my2DCodeNum];
        imge2.GetComponent<Units2DData>().minNum = (int)Dictimge2["숫자 최소치"] + (int)(Convert.ToSingle(Dictimge2["숫자 최소치 강화"]) * imge2.GetComponent<Units2DData>().upgradeRank);
        imge2.GetComponent<Units2DData>().maxNum = (int)Dictimge2["숫자 최대치"] + (int)(Convert.ToSingle(Dictimge2["숫자최대치 강화"]) * imge2.GetComponent<Units2DData>().upgradeRank);

        imge3 = Instantiate(character2DShop, gameObject.transform);
        imge3.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, 100);
        imge3.GetComponent<Units2DData>().my2DCodeNum = UnitCode2DScout3;
        imge3.GetComponent<Units2DData>().upgradeRank = UnityEngine.Random.Range(0, libmgr.stageLevelCount + 1);
        Dictionary<string, object> Dictimge3 = libmgr.unitCode[imge3.GetComponent<Units2DData>().my2DCodeNum];
        imge3.GetComponent<Units2DData>().minNum = (int)Dictimge3["숫자 최소치"] + (int)(Convert.ToSingle(Dictimge3["숫자 최소치 강화"]) * imge3.GetComponent<Units2DData>().upgradeRank);
        imge3.GetComponent<Units2DData>().maxNum = (int)Dictimge3["숫자 최대치"] + (int)(Convert.ToSingle(Dictimge3["숫자최대치 강화"]) * imge3.GetComponent<Units2DData>().upgradeRank);

        howMuch1.GetComponentInChildren<Text>().text = 
            (100 - Convert.ToInt32(libmgr.unitCode[imge1.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"])*10 
            + imge1.GetComponent<Units2DData>().upgradeRank*70).ToString();

        howMuch2.GetComponentInChildren<Text>().text =
            (100 - Convert.ToInt32(libmgr.unitCode[imge2.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"]) * 10
            + imge2.GetComponent<Units2DData>().upgradeRank * 70).ToString();

        howMuch3.GetComponentInChildren<Text>().text =
            (100 - Convert.ToInt32(libmgr.unitCode[imge3.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"]) * 10
            + imge3.GetComponent<Units2DData>().upgradeRank * 70).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buytheUnit(GameObject t)
    {
        int buymoney = 0;
        if (t == howMuch1)
        {
            buymoney = 100 - Convert.ToInt32(libmgr.unitCode[imge1.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"]) * 10
            + imge1.GetComponent<Units2DData>().upgradeRank * 70;
            if (libmgr.money >= buymoney)
            {
                libmgr.money -= buymoney;
                libmgr.playerUnitsData.Add(new List<object> { imge1.GetComponent<Units2DData>().my2DCodeNum, imge1.GetComponent<Units2DData>().upgradeRank });
                Destroy(t.GetComponent<Button>());
                Destroy(imge1);
            }
            else { Debug.Log("no money!"); }
        }
        else if (t == howMuch2)
        {
            buymoney = 100 - Convert.ToInt32(libmgr.unitCode[imge2.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"]) * 10
           + imge2.GetComponent<Units2DData>().upgradeRank * 70;
            if (libmgr.money >= buymoney)
            {
                libmgr.money -= buymoney;
                libmgr.playerUnitsData.Add(new List<object> { imge2.GetComponent<Units2DData>().my2DCodeNum, imge2.GetComponent<Units2DData>().upgradeRank });
                Destroy(t.GetComponent<Button>());
                Destroy(imge2);
            }
            else { Debug.Log("no money!"); }
        }
        else if (t == howMuch3)
        {
            buymoney = 100 - Convert.ToInt32(libmgr.unitCode[imge3.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"]) * 10
           + imge3.GetComponent<Units2DData>().upgradeRank * 70;
           if (libmgr.money >= buymoney)
           {
                libmgr.money -= buymoney;
                libmgr.playerUnitsData.Add(new List<object> { imge3.GetComponent<Units2DData>().my2DCodeNum, imge3.GetComponent<Units2DData>().upgradeRank });
                Destroy(t.GetComponent<Button>());
                Destroy(imge3);
            }
           else { Debug.Log("no money!"); }
        }
    }
}
