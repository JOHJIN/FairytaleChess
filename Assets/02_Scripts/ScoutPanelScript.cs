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
            if (Convert.ToInt32(libmgr.normalUnits[i]["���� ��͵�"]) == 5) rareNum5.Add(libmgr.normalUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.normalUnits[i]["���� ��͵�"]) == 4) rareNum5.Add(libmgr.normalUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.normalUnits[i]["���� ��͵�"]) == 3) rareNum5.Add(libmgr.normalUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.normalUnits[i]["���� ��͵�"]) == 2) rareNum5.Add(libmgr.normalUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.normalUnits[i]["���� ��͵�"]) == 1) rareNum5.Add(libmgr.normalUnits[i]["ID"]);
            else { }
        }
        for (int i = 0; i < libmgr.otherUnits.Count; i++)
        {
            if (Convert.ToInt32(libmgr.otherUnits[i]["���� ��͵�"]) == 5) rareNum5.Add(libmgr.otherUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.otherUnits[i]["���� ��͵�"]) == 4) rareNum5.Add(libmgr.otherUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.otherUnits[i]["���� ��͵�"]) == 3) rareNum5.Add(libmgr.otherUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.otherUnits[i]["���� ��͵�"]) == 2) rareNum5.Add(libmgr.otherUnits[i]["ID"]);
            else if (Convert.ToInt32(libmgr.otherUnits[i]["���� ��͵�"]) == 1) rareNum5.Add(libmgr.otherUnits[i]["ID"]);
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
            buymoney = 100 - Convert.ToInt32(libmgr.unitCode[imge1.GetComponent<Units2DData>().my2DCodeNum]["���� ��͵�"]) * 10
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
            buymoney = 100 - Convert.ToInt32(libmgr.unitCode[imge2.GetComponent<Units2DData>().my2DCodeNum]["���� ��͵�"]) * 10
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
            buymoney = 100 - Convert.ToInt32(libmgr.unitCode[imge3.GetComponent<Units2DData>().my2DCodeNum]["���� ��͵�"]) * 10
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
        imges.GetComponent<Units2DData>().minNum = (int)Dictimge1["���� �ּ�ġ"] + (int)(Convert.ToSingle(Dictimge1["���� �ּ�ġ ��ȭ"]) * imges.GetComponent<Units2DData>().upgradeRank);
        imges.GetComponent<Units2DData>().maxNum = (int)Dictimge1["���� �ִ�ġ"] + (int)(Convert.ToSingle(Dictimge1["�����ִ�ġ ��ȭ"]) * imges.GetComponent<Units2DData>().upgradeRank);
        imges.name = Dictimge1["Name"].ToString();

        Units2DData fUFun = imges.GetComponent<Units2DData>();
        fUFun.my2DCodeNum = UnitCode2DScout1;
        fUFun.upgradeRank = UnityEngine.Random.Range(0, libmgr.stageLevelCount + 1);
        fUFun.moveCountUpgrade = Convert.ToSingle(Dictimge1["�̵�Ƚ�� ��ȭ"]);
        fUFun.minNumUpgrade = Convert.ToSingle(Dictimge1["���� �ּ�ġ ��ȭ"]);
        fUFun.maxNumUpgrade = Convert.ToSingle(Dictimge1["�����ִ�ġ ��ȭ"]);
        fUFun.moveMaxCount = (int)Dictimge1["�̵� Ƚ��"] + (int)fUFun.moveCountUpgrade * fUFun.upgradeRank;
        fUFun.minNum = (int)Dictimge1["���� �ּ�ġ"] + (int)fUFun.minNumUpgrade * fUFun.upgradeRank;
        fUFun.maxNum = (int)Dictimge1["���� �ִ�ġ"] + (int)fUFun.maxNumUpgrade * fUFun.upgradeRank;

        Dictionary<string, object> DictEffect = libmgr.unitCodeEffects[UnitCode2DScout1];

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
            fUFun.attackMinusPow = Convert.ToInt32(DictEffect["���ݽ� ��� ���� ����"]);
            fUFun.itsEffect += ", ����" + " - " + fUFun.attackMinusPow;
        }
        if (Convert.ToString(DictEffect["�ڽ� ���� ����"]) != "")
        {
            fUFun.morePower = Convert.ToInt32(DictEffect["�ڽ� ���� ����"]);
            fUFun.itsEffect += ", �ູ" + " + " + fUFun.morePower;
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
            fUFun.powerUpTotem = Convert.ToInt32(DictEffect["�ٸ� �Ʊ� ���� ����"]);
            fUFun.itsEffect += ", �Ʊ� �ູ" + " + " + fUFun.powerUpTotem;
        }
        if (Convert.ToString(DictEffect["�� ������ �̵� ���"]) == "TRUE")
        {
            fUFun.itsEffect += ", ����";
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
                (100 - Convert.ToInt32(libmgr.unitCode[imge1.GetComponent<Units2DData>().my2DCodeNum]["���� ��͵�"]) * 10
                + imge1.GetComponent<Units2DData>().upgradeRank * 70).ToString();
        }
        else if (a == 2) 
        { 
            imge2 = imges; 
            imge2.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
            howMuch2.GetComponentInChildren<Text>().text =
                (100 - Convert.ToInt32(libmgr.unitCode[imge2.GetComponent<Units2DData>().my2DCodeNum]["���� ��͵�"]) * 10
                + imge2.GetComponent<Units2DData>().upgradeRank * 70).ToString();
        }
        else if (a == 3) 
        { 
            imge3 = imges; 
            imge3.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, 100);
            howMuch3.GetComponentInChildren<Text>().text =
                (100 - Convert.ToInt32(libmgr.unitCode[imge3.GetComponent<Units2DData>().my2DCodeNum]["���� ��͵�"]) * 10
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
