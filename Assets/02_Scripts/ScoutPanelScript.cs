using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoutPanelScript : MonoBehaviour
{
    public LibMgr libmgr;
    public ShopMgr shopmgr;
    public UpgradePanelScript upgradescript;

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

    public GameObject imges;

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

        StartCoroutine(coroutineTurn());
    }

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
                upgradescript.settingMyUnits();
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
                upgradescript.settingMyUnits();
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
                upgradescript.settingMyUnits();
           }
           else { Debug.Log("no money!"); }
        }
    }

    public IEnumerator unitRandomScout(int a)
    {
        int rnd1 = UnityEngine.Random.Range(0, rareNum5.Count + rareNum4.Count + rareNum3.Count + rareNum2.Count + rareNum3.Count);

        if (rnd1 < rareNum5.Count) { UnitCode2DScout1 = rareNum5[rnd1]; }
        else if (rareNum5.Count <= rnd1 && rnd1 < rareNum4.Count) { UnitCode2DScout1 = rareNum4[rnd1 - rareNum5.Count]; }
        else if (rareNum4.Count <= rnd1 && rnd1 < rareNum3.Count) { UnitCode2DScout1 = rareNum3[rnd1 - rareNum4.Count - rareNum5.Count]; }
        else if (rareNum3.Count <= rnd1 && rnd1 < rareNum2.Count) { UnitCode2DScout1 = rareNum2[rnd1 - rareNum3.Count - rareNum4.Count - rareNum5.Count]; }
        else if (rareNum2.Count <= rnd1) { UnitCode2DScout1 = rareNum4[rnd1 - rareNum2.Count - rareNum3.Count - rareNum4.Count - rareNum5.Count]; }

        imges = Instantiate(character2DShop, gameObject.transform);
        imges.GetComponent<Units2DData>().my2DCodeNum = UnitCode2DScout1;
        imges.GetComponent<Units2DData>().upgradeRank = UnityEngine.Random.Range(0, libmgr.stageLevelCount + 1);
        Dictionary<string, object> Dictimge1 = libmgr.unitCode[imges.GetComponent<Units2DData>().my2DCodeNum];
        imges.GetComponent<Units2DData>().minNum = (int)Dictimge1["숫자 최소치"] + (int)(Convert.ToSingle(Dictimge1["숫자 최소치 강화"]) * imges.GetComponent<Units2DData>().upgradeRank);
        imges.GetComponent<Units2DData>().maxNum = (int)Dictimge1["숫자 최대치"] + (int)(Convert.ToSingle(Dictimge1["숫자최대치 강화"]) * imges.GetComponent<Units2DData>().upgradeRank);
        imges.name = Dictimge1["Name"].ToString();

        Units2DData fUFun = imges.GetComponent<Units2DData>();
        fUFun.my2DCodeNum = UnitCode2DScout1;
        fUFun.upgradeRank = UnityEngine.Random.Range(0, libmgr.stageLevelCount + 1);
        fUFun.moveCountUpgrade = Convert.ToSingle(Dictimge1["이동횟수 강화"]);
        fUFun.minNumUpgrade = Convert.ToSingle(Dictimge1["숫자 최소치 강화"]);
        fUFun.maxNumUpgrade = Convert.ToSingle(Dictimge1["숫자최대치 강화"]);
        fUFun.moveMaxCount = (int)Dictimge1["이동 횟수"] + (int)fUFun.moveCountUpgrade * fUFun.upgradeRank;
        fUFun.minNum = (int)Dictimge1["숫자 최소치"] + (int)fUFun.minNumUpgrade * fUFun.upgradeRank;
        fUFun.maxNum = (int)Dictimge1["숫자 최대치"] + (int)fUFun.maxNumUpgrade * fUFun.upgradeRank;

        Dictionary<string, object> DictEffect = libmgr.unitCodeEffects[UnitCode2DScout1];

        if (Convert.ToString(DictEffect["대각선만 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = false; }
        if (Convert.ToString(DictEffect["전방위 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = true; }

        if (fUFun.rook && !fUFun.bishop)
            fUFun.itsEffect = "룩";
        else if (!fUFun.rook && fUFun.bishop)
            fUFun.itsEffect = "비숍";
        else if (fUFun.rook && fUFun.bishop)
            fUFun.itsEffect = "퀸";

        if (Convert.ToString(DictEffect["뒤로 이동 불가"]) == "TRUE")
        {
            fUFun.itsEffect += ", 폰";
            fUFun.pawn = true;
        }
        if (Convert.ToString(DictEffect["홀수 차례만 효과"]) == "TRUE")
        {
            fUFun.itsEffect += ", 홀수 차례";
            fUFun.oddTurnEffect = true;
        }
        if (Convert.ToString(DictEffect["짝수 차례만 효과"]) == "TRUE")
        {
            fUFun.itsEffect += ", 짝수 차례";
            fUFun.evenTurnEffect = true;
        }
        if (Convert.ToString(DictEffect["좌우 워프"]) == "TRUE")
        {
            fUFun.itsEffect += ", 워프";
            fUFun.warp = true;
        }
        if (Convert.ToString(DictEffect["전방향 전투"]) == "TRUE")
        {
            fUFun.itsEffect += ", 전방향 전투";
            fUFun.attackEvery = true;
        }
        if (Convert.ToString(DictEffect["위치 교체"]) == "TRUE")
        {
            fUFun.itsEffect += ", 교대";
            fUFun.changePos = true;
        }
        if (Convert.ToString(DictEffect["공격시 상대 숫자 변동"]) != "")
        {
            fUFun.attackMinusPow = Convert.ToInt32(DictEffect["공격시 상대 숫자 변동"]);
            fUFun.itsEffect += ", 저주" + " - " + fUFun.attackMinusPow;
        }
        if (Convert.ToString(DictEffect["자신 숫자 변동"]) != "")
        {
            fUFun.morePower = Convert.ToInt32(DictEffect["자신 숫자 변동"]);
            fUFun.itsEffect += ", 축복" + " + " + fUFun.morePower;
        }
        if (Convert.ToString(DictEffect["죽으면 패배"]) == "TRUE")
        {
            fUFun.itsEffect += ", 심장";
            fUFun.playerHeart = true;
        }
        if (Convert.ToString(DictEffect["위치 고정"]) == "TRUE")
        {
            fUFun.itsEffect += ", 이동 불가";
            fUFun.cantMove = true;
        }
        if (Convert.ToString(DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"]) != "")
        {
            fUFun.itsEffect += ", 업그레이드";
            fUFun.upgradeCode = DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"];
        }
        if (Convert.ToString(DictEffect["자신 진영에서만 이동 가능"]) == "TRUE")
        {
            fUFun.itsEffect += ", 士";
            fUFun.onlyMyPlace = true;
        }
        if (Convert.ToString(DictEffect["중립진영에 배치"]) == "TRUE")
        {
            fUFun.itsEffect += ", 정찰병";
            fUFun.placeAnywhere = true;
        }
        if (Convert.ToString(DictEffect["못 움직임 사망시"]) == "TRUE")
        {
            fUFun.itsEffect += ", 독";
            fUFun.poision = true;
        }
        if (Convert.ToString(DictEffect["숫자 범위 변하지 않음"]) == "TRUE")
        {
            fUFun.itsEffect += ", 숫자 변동 없음";
            fUFun.cristalBody = true;
        }
        if (Convert.ToString(DictEffect["차례 종료시 랜덤 이동"]) == "TRUE")
        {
            fUFun.itsEffect += ", 랜덤 이동";
            fUFun.randomMove = true;
        }
        if (Convert.ToString(DictEffect["다른 아군 숫자 변동"]) != "")
        {
            fUFun.powerUpTotem = Convert.ToInt32(DictEffect["다른 아군 숫자 변동"]);
            fUFun.itsEffect += ", 아군 축복" + " + " + fUFun.powerUpTotem;
        }
        if (Convert.ToString(DictEffect["못 움직임 이동 대신"]) == "TRUE")
        {
            fUFun.itsEffect += ", 빙결";
            fUFun.frozen = true;
        }

        imges.GetComponent<Units2DData>().unit2DImage.sprite = Resources.Load<Sprite>("Image2D/" + imges.GetComponent<Units2DData>().my2DCodeNum.ToString());
        fUFun.unitEffectTxt.GetComponent<Text>().text = fUFun.itsEffect;
        fUFun.unitMoveTxt.GetComponent<Text>().text = "Move : " + fUFun.moveMaxCount.ToString();
        fUFun.unitNameTxt.GetComponent<Text>().text = Dictimge1["Name"].ToString() + " + " + fUFun.upgradeRank;
        fUFun.unitNumTxt.GetComponent<Text>().text =fUFun.minNum + " ~ " + fUFun.maxNum;

        if (a == 1)
        { 
            imge1 = imges; 
            imge1.GetComponent<RectTransform>().anchoredPosition = new Vector2(-500, 100);
            howMuch1.GetComponentInChildren<Text>().text =
                (100 - Convert.ToInt32(libmgr.unitCode[imge1.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"]) * 10
                + imge1.GetComponent<Units2DData>().upgradeRank * 70).ToString();
        }
        else if (a == 2) 
        { 
            imge2 = imges; 
            imge2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
            howMuch2.GetComponentInChildren<Text>().text =
                (100 - Convert.ToInt32(libmgr.unitCode[imge2.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"]) * 10
                + imge2.GetComponent<Units2DData>().upgradeRank * 70).ToString();
        }
        else if (a == 3) 
        { 
            imge3 = imges; 
            imge3.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, 100);
            howMuch3.GetComponentInChildren<Text>().text =
                (100 - Convert.ToInt32(libmgr.unitCode[imge3.GetComponent<Units2DData>().my2DCodeNum]["영입 희귀도"]) * 10
                + imge3.GetComponent<Units2DData>().upgradeRank * 70).ToString();
        }

        yield return new WaitForSecondsRealtime(0.3f);
    }

    public IEnumerator coroutineTurn()
    {
        yield return StartCoroutine(unitRandomScout(1));
        yield return StartCoroutine(unitRandomScout(2));
        yield return StartCoroutine(unitRandomScout(3));
    }
}
