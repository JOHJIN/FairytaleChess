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
            fUFun.moveCountUpgrade = Convert.ToSingle(Dict["�̵�Ƚ�� ��ȭ"]);
            fUFun.minNumUpgrade = Convert.ToSingle(Dict["���� �ּ�ġ ��ȭ"]);
            fUFun.maxNumUpgrade = Convert.ToSingle(Dict["�����ִ�ġ ��ȭ"]);
            fUFun.moveMaxCount = (int)Dict["�̵� Ƚ��"] + (int)fUFun.moveCountUpgrade * fUFun.upgradeRank;
            fUFun.minNum = (int)Dict["���� �ּ�ġ"] + (int)fUFun.minNumUpgrade * fUFun.upgradeRank;
            fUFun.maxNum = (int)Dict["���� �ִ�ġ"] + (int)fUFun.maxNumUpgrade * fUFun.upgradeRank;

            if (Convert.ToString(DictEffect["�¿� ����"]) == "TRUE") fUFun.warp = true;
            if (Convert.ToString(DictEffect["������ ����"]) == "TRUE") fUFun.attackEvery = true;
            if (Convert.ToString(DictEffect["��ġ ��ü"]) == "TRUE") fUFun.changePos = true;
            if (Convert.ToString(DictEffect["���ݽ� ��� ���� ����"]) != "")
                fUFun.attackMinusPow = Convert.ToInt32(DictEffect["���ݽ� ��� ���� ����"]);
            if (Convert.ToString(DictEffect["�ڽ� ���� ����"]) != "")
                fUFun.morePower = Convert.ToInt32(DictEffect["�ڽ� ���� ����"]);
            if (Convert.ToString(DictEffect["������ �й�"]) == "TRUE") fUFun.playerHeart = true;
            if (Convert.ToString(DictEffect["Ȧ�� ���ʸ� ȿ��"]) == "TRUE") fUFun.oddTurnEffect = true;
            if (Convert.ToString(DictEffect["¦�� ���ʸ� ȿ��"]) == "TRUE") fUFun.evenTurnEffect = true;
            if (Convert.ToString(DictEffect["��ġ ����"]) == "TRUE") fUFun.cantMove = true;
            if (Convert.ToString(DictEffect["��� ���� ���� �����ϸ� ��ȭ(������)"]) != "")
                fUFun.upgradeCode = DictEffect["��� ���� ���� �����ϸ� ��ȭ(������)"];
            if (Convert.ToString(DictEffect["�밢���� �̵�"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = false; }
            if (Convert.ToString(DictEffect["������ �̵�"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = true; }
            if (Convert.ToString(DictEffect["�ڽ� ���������� �̵� ����"]) == "TRUE") fUFun.onlyMyPlace = true;
            if (Convert.ToString(DictEffect["�߸������� ��ġ"]) == "TRUE") fUFun.placeAnywhere = true;
            if (Convert.ToString(DictEffect["�� ������ �����"]) == "TRUE") fUFun.poision = true;
            if (Convert.ToString(DictEffect["���� ���� ������ ����"]) == "TRUE") fUFun.cristalBody = true;
            if (Convert.ToString(DictEffect["�ڷ� �̵� �Ұ�"]) == "TRUE") fUFun.pawn = true;
            if (Convert.ToString(DictEffect["���� ����� ���� �̵�"]) == "TRUE") fUFun.randomMove = true;
            if (Convert.ToString(DictEffect["�ٸ� �Ʊ� ���� ����"]) != "")
                fUFun.powerUpTotem = Convert.ToInt32(DictEffect["�ٸ� �Ʊ� ���� ����"]);
            if (Convert.ToString(DictEffect["�� ������ �̵� ���"]) == "TRUE") fUFun.frozen = true;

            shopmgr.shopPlayerUnits.Add(My2Dunit);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
