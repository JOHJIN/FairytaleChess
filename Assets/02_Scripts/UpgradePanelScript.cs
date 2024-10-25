using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelScript : MonoBehaviour
{
    public LibMgr libmgr;
    public GameObject myUnit2DBass;
    public GameObject scrollviewContent;
    public ShopMgr shopmgr;

    // Start is called before the first frame update
    void Start()
    {
        libmgr = GameObject.Find("LibMgr").GetComponent<LibMgr>();
        settingMyUnits();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void imTarget(GameObject target)
    {
        shopmgr.selectMyUnit2D = target;
    }

    public void settingMyUnits()
    {
        for (int i = 1; i < libmgr.playerUnitsData.Count; i++)
        {
            GameObject My2Dunit = Instantiate(myUnit2DBass, scrollviewContent.transform);

            Dictionary<string, object> Dict = libmgr.unitCode[libmgr.playerUnitsData[i][0]];
            Dictionary<string, object> DictEffect = libmgr.unitCodeEffects[libmgr.playerUnitsData[i][0]];
            My2Dunit.name = Dict["Name"].ToString();

            Units2DData fUFun = My2Dunit.GetComponent<Units2DData>();
            fUFun.my2DCodeNum = libmgr.playerUnitsData[i][0];
            fUFun.upgradeRank = (int)libmgr.playerUnitsData[i][1];
            fUFun.moveCountUpgrade = Convert.ToSingle(Dict["이동횟수 강화"]);
            fUFun.minNumUpgrade = Convert.ToSingle(Dict["숫자 최소치 강화"]);
            fUFun.maxNumUpgrade = Convert.ToSingle(Dict["숫자최대치 강화"]);
            fUFun.moveMaxCount = (int)Dict["이동 횟수"] + (int)fUFun.moveCountUpgrade * fUFun.upgradeRank;
            fUFun.minNum = (int)Dict["숫자 최소치"] + (int)fUFun.minNumUpgrade * fUFun.upgradeRank;
            fUFun.maxNum = (int)Dict["숫자 최대치"] + (int)fUFun.maxNumUpgrade * fUFun.upgradeRank;
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
                fUFun.itsEffect += ", 저주";
                fUFun.attackMinusPow = Convert.ToInt32(DictEffect["공격시 상대 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["자신 숫자 변동"]) != "")
            {
                fUFun.itsEffect += ", 축복";
                fUFun.morePower = Convert.ToInt32(DictEffect["자신 숫자 변동"]);
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
                fUFun.itsEffect += ", 아군 축복";
                fUFun.powerUpTotem = Convert.ToInt32(DictEffect["다른 아군 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["못 움직임 이동 대신"]) == "TRUE")
            {
                fUFun.itsEffect += ", 빙결";
                fUFun.frozen = true;
            }
            My2Dunit.GetComponent<Units2DData>().unit2DImage.sprite = Resources.Load<Sprite>("Image2D/" + My2Dunit.GetComponent<Units2DData>().my2DCodeNum.ToString());
            fUFun.unitEffectTxt.GetComponent<Text>().text = fUFun.itsEffect;
            fUFun.unitMoveTxt.GetComponent<Text>().text = "Move : " + fUFun.moveMaxCount.ToString();
            fUFun.unitNameTxt.GetComponent<Text>().text = Dict["Name"].ToString() + " + " + fUFun.upgradeRank;
            fUFun.unitNumTxt.GetComponent<Text>().text = fUFun.minNum + " ~ " + fUFun.maxNum;

            My2Dunit.GetComponent<Button>().onClick.AddListener(() => imTarget(My2Dunit));
            shopmgr.shopPlayerUnits.Add(My2Dunit);
        }
        GameObject p1 = Instantiate(myUnit2DBass, scrollviewContent.transform);
        Dictionary<string, object> playerDict = libmgr.unitCode[libmgr.playerUnitsData[0][0]];
        p1.name = playerDict["Name"].ToString();
        p1.GetComponent<Units2DData>().my2DCodeNum = libmgr.playerUnitsData[0][0];
        p1.GetComponent<Units2DData>().upgradeRank = (int)libmgr.playerUnitsData[0][1];
        p1.GetComponent<Units2DData>().unit2DImage.sprite = Resources.Load<Sprite>("Image2D/" + p1.GetComponent<Units2DData>().my2DCodeNum.ToString());
        p1.GetComponent<Units2DData>().unitEffectTxt.GetComponent<Text>().text = p1.GetComponent<Units2DData>().itsEffect;
        p1.GetComponent<Units2DData>().unitMoveTxt.GetComponent<Text>().text = "Move : " + p1.GetComponent<Units2DData>().moveMaxCount.ToString();
        p1.GetComponent<Units2DData>().unitNameTxt.GetComponent<Text>().text = playerDict["Name"].ToString() + " + " + p1.GetComponent<Units2DData>().upgradeRank;
        p1.GetComponent<Units2DData>().unitNumTxt.GetComponent<Text>().text = p1.GetComponent<Units2DData>().minNum + " ~ " + p1.GetComponent<Units2DData>().maxNum;
        p1.GetComponent<Button>().onClick.AddListener(() => imTarget(p1));
        shopmgr.shopPlayerUnits.Add(p1);
    }
}
