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
            fUFun.moveCountUpgrade = Convert.ToSingle(Dict["�̵�Ƚ�� ��ȭ"]);
            fUFun.minNumUpgrade = Convert.ToSingle(Dict["���� �ּ�ġ ��ȭ"]);
            fUFun.maxNumUpgrade = Convert.ToSingle(Dict["�����ִ�ġ ��ȭ"]);
            fUFun.moveMaxCount = (int)Dict["�̵� Ƚ��"] + (int)fUFun.moveCountUpgrade * fUFun.upgradeRank;
            fUFun.minNum = (int)Dict["���� �ּ�ġ"] + (int)fUFun.minNumUpgrade * fUFun.upgradeRank;
            fUFun.maxNum = (int)Dict["���� �ִ�ġ"] + (int)fUFun.maxNumUpgrade * fUFun.upgradeRank;
            if (Convert.ToString(DictEffect["�밢���� �̵�"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = false; }
            if (Convert.ToString(DictEffect["������ �̵�"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = true; }

            if (fUFun.rook && !fUFun.bishop)
                fUFun.itsEffect = "��";
            else if (!fUFun.rook && fUFun.bishop)
                fUFun.itsEffect = "���";
            else if (fUFun.rook && fUFun.bishop)
                fUFun.itsEffect = "��";

            if (Convert.ToString(DictEffect["�ڷ� �̵� �Ұ�"]) == "TRUE")
            {
                fUFun.itsEffect += ", ��";
                fUFun.pawn = true;
            }
            if (Convert.ToString(DictEffect["Ȧ�� ���ʸ� ȿ��"]) == "TRUE")
            {
                fUFun.itsEffect += ", Ȧ�� ����";
                fUFun.oddTurnEffect = true;
            }
            if (Convert.ToString(DictEffect["¦�� ���ʸ� ȿ��"]) == "TRUE")
            {
                fUFun.itsEffect += ", ¦�� ����";
                fUFun.evenTurnEffect = true;
            }
            if (Convert.ToString(DictEffect["�¿� ����"]) == "TRUE")
            {
                fUFun.itsEffect += ", ����";
                fUFun.warp = true;
            }
            if (Convert.ToString(DictEffect["������ ����"]) == "TRUE")
            {
                fUFun.itsEffect += ", ������ ����";
                fUFun.attackEvery = true;
            }
            if (Convert.ToString(DictEffect["��ġ ��ü"]) == "TRUE")
            {
                fUFun.itsEffect += ", ����";
                fUFun.changePos = true;
            }
            if (Convert.ToString(DictEffect["���ݽ� ��� ���� ����"]) != "")
            {
                fUFun.itsEffect += ", ����";
                fUFun.attackMinusPow = Convert.ToInt32(DictEffect["���ݽ� ��� ���� ����"]);
            }
            if (Convert.ToString(DictEffect["�ڽ� ���� ����"]) != "")
            {
                fUFun.itsEffect += ", �ູ";
                fUFun.morePower = Convert.ToInt32(DictEffect["�ڽ� ���� ����"]);
            }
            if (Convert.ToString(DictEffect["������ �й�"]) == "TRUE")
            {
                fUFun.itsEffect += ", ����";
                fUFun.playerHeart = true;
            }
            if (Convert.ToString(DictEffect["��ġ ����"]) == "TRUE")
            {
                fUFun.itsEffect += ", �̵� �Ұ�";
                fUFun.cantMove = true;
            }
            if (Convert.ToString(DictEffect["��� ���� ���� �����ϸ� ��ȭ(������)"]) != "")
            {
                fUFun.itsEffect += ", ���׷��̵�";
                fUFun.upgradeCode = DictEffect["��� ���� ���� �����ϸ� ��ȭ(������)"];
            }
            if (Convert.ToString(DictEffect["�ڽ� ���������� �̵� ����"]) == "TRUE")
            {
                fUFun.itsEffect += ", ��";
                fUFun.onlyMyPlace = true;
            }
            if (Convert.ToString(DictEffect["�߸������� ��ġ"]) == "TRUE")
            {
                fUFun.itsEffect += ", ������";
                fUFun.placeAnywhere = true;
            }
            if (Convert.ToString(DictEffect["�� ������ �����"]) == "TRUE")
            {
                fUFun.itsEffect += ", ��";
                fUFun.poision = true;
            }
            if (Convert.ToString(DictEffect["���� ���� ������ ����"]) == "TRUE")
            {
                fUFun.itsEffect += ", ���� ���� ����";
                fUFun.cristalBody = true;
            }
            if (Convert.ToString(DictEffect["���� ����� ���� �̵�"]) == "TRUE")
            {
                fUFun.itsEffect += ", ���� �̵�";
                fUFun.randomMove = true;
            }
            if (Convert.ToString(DictEffect["�ٸ� �Ʊ� ���� ����"]) != "")
            {
                fUFun.itsEffect += ", �Ʊ� �ູ";
                fUFun.powerUpTotem = Convert.ToInt32(DictEffect["�ٸ� �Ʊ� ���� ����"]);
            }
            if (Convert.ToString(DictEffect["�� ������ �̵� ���"]) == "TRUE")
            {
                fUFun.itsEffect += ", ����";
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
