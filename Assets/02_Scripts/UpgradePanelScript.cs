using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        GameObject p1 = Instantiate(myUnit2DBass, scrollviewContent.transform);
        Dictionary<string, object> playerDict = libmgr.unitCode[libmgr.playerUnitsData[0][0]];
        p1.name = playerDict["Name"].ToString();
        p1.GetComponent<Units2DData>().upgradeRank = (int)libmgr.playerUnitsData[0][1];
        shopmgr.shopPlayerUnits.Add(p1);

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

            if (Convert.ToString(DictEffect["좌우 워프"]) == "TRUE") fUFun.warp = true;
            if (Convert.ToString(DictEffect["전방향 전투"]) == "TRUE") fUFun.attackEvery = true;
            if (Convert.ToString(DictEffect["위치 교체"]) == "TRUE") fUFun.changePos = true;
            if (Convert.ToString(DictEffect["공격시 상대 숫자 변동"]) != "")
                fUFun.attackMinusPow = Convert.ToInt32(DictEffect["공격시 상대 숫자 변동"]);
            if (Convert.ToString(DictEffect["자신 숫자 변동"]) != "")
                fUFun.morePower = Convert.ToInt32(DictEffect["자신 숫자 변동"]);
            if (Convert.ToString(DictEffect["죽으면 패배"]) == "TRUE") fUFun.playerHeart = true;
            if (Convert.ToString(DictEffect["홀수 차례만 효과"]) == "TRUE") fUFun.oddTurnEffect = true;
            if (Convert.ToString(DictEffect["짝수 차례만 효과"]) == "TRUE") fUFun.evenTurnEffect = true;
            if (Convert.ToString(DictEffect["위치 고정"]) == "TRUE") fUFun.cantMove = true;
            if (Convert.ToString(DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"]) != "")
                fUFun.upgradeCode = DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"];
            if (Convert.ToString(DictEffect["대각선만 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = false; }
            if (Convert.ToString(DictEffect["전방위 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = true; }
            if (Convert.ToString(DictEffect["자신 진영에서만 이동 가능"]) == "TRUE") fUFun.onlyMyPlace = true;
            if (Convert.ToString(DictEffect["중립진영에 배치"]) == "TRUE") fUFun.placeAnywhere = true;
            if (Convert.ToString(DictEffect["못 움직임 사망시"]) == "TRUE") fUFun.poision = true;
            if (Convert.ToString(DictEffect["숫자 범위 변하지 않음"]) == "TRUE") fUFun.cristalBody = true;
            if (Convert.ToString(DictEffect["뒤로 이동 불가"]) == "TRUE") fUFun.pawn = true;
            if (Convert.ToString(DictEffect["차례 종료시 랜덤 이동"]) == "TRUE") fUFun.randomMove = true;
            if (Convert.ToString(DictEffect["다른 아군 숫자 변동"]) != "")
                fUFun.powerUpTotem = Convert.ToInt32(DictEffect["다른 아군 숫자 변동"]);
            if (Convert.ToString(DictEffect["못 움직임 이동 대신"]) == "TRUE") fUFun.frozen = true;

            shopmgr.shopPlayerUnits.Add(My2Dunit);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
