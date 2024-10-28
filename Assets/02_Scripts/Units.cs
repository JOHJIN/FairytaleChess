using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Units : MonoBehaviour
{
    public object myCodeNum;
    public object upgradeCode = null;

    public String unitEffectTxt;
    public Sprite unit2DImage;

    public int moveCount = 0;
    public int moveMaxCount = 1;
    public int maxNum = 0;
    public int minNum = 0;
    public float maxNumUpgrade = 0;
    public float minNumUpgrade = 0;
    public int upgradeRank = 0;
    public float moveCountUpgrade = 0;
    public float halfNum = 0;
    public GameMgr gmr;
    public GmrUI gmrUi;
    public LibMgr libmgr;

    public bool moveAble = false;

    public bool rook = true;
    public bool bishop = false;
    public bool pawn = false;
    public bool cantMove = false;

    public bool warp = false;
    public bool attackEvery = false;
    public bool changePos = false;
    public bool playerHeart = false;
    public bool oddTurnEffect = false;
    public bool evenTurnEffect = false;
    public bool onlyMyPlace = false;
    public bool placeAnywhere = false;
    public bool poision = false;
    public bool cristalBody = false;
    public bool randomMove = false;
    public bool frozen = false;

    public int attackMinusPow = 0;
    public int morePower = 0;
    public int powerUpTotem = 0;

    public bool moveSmooth = false;
    public bool placementMove = false;

    Vector3 UnitVec;
    Vector3 zeroS;

    public bool moving = false;

    GameObject selectEffect;
    void Start()
    {
        libmgr = gmr.libmgr;
        selectEffect = transform.GetChild(1).gameObject;
    }


    void Update()
    {
        if (moveSmooth)
        {
            transform.position = Vector3.SmoothDamp(transform.position, UnitVec, ref zeroS, 0.25f);
            if (Mathf.Abs(transform.position.x - UnitVec.x) +
                Mathf.Abs(transform.position.z - UnitVec.z) < 0.01f)
            {
                transform.position = UnitVec;
                moveAfterFight();
                moveSmooth = false;
            }
        }
        if (placementMove)
        {
            transform.position = Vector3.SmoothDamp(transform.position, UnitVec, ref zeroS, 0.25f);
            if (Mathf.Abs(transform.position.x - UnitVec.x) +
               Mathf.Abs(transform.position.z - UnitVec.z) < 0.01f)
            {  
                transform.position = UnitVec;
                UnitVec.y = 0;
                transform.position = Vector3.SmoothDamp(transform.position, UnitVec, ref zeroS, 0.1f);
                if (Mathf.Abs(transform.position.y - UnitVec.y) < 0.005f)
                {
                    transform.position = UnitVec;
                    placementMove = false;
                    gmr.unitplacementMore();
                }
            }
        }
        //이동횟수 남았을 때 다른 유닛 선택시 이동 횟수 0
        if (moving)
        {
            if (!gmr.selectUnit == gameObject)
            {
                if (gmr.selectUnit.tag == "Friendly" || gmr.selectUnit.tag == "Player")
                {
                    moveAble = false;
                    gmrUi.moveCountTxtChange(gmr.turnMaxMove - gmr.turnMove);
                    moving = false;
                }
            }
        }

        if (upgradeCode != null && transform.position.z >= gmr.mapMaxZ)
        {
            UpgradeThisUnit();
        }

        if (gmr.selectUnit == this.gameObject)
        {
            selectEffect.SetActive(true);
        }
        else 
        {
            selectEffect.SetActive(false);
        }
    }

    //일반 이동
    public void Move(Vector3 Destination)
    {
        if(cantMove) return;
        if (moveAble && !moveSmooth)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Destination, out hit, 1f))
            {
                if (hit.collider.tag == "Player" || hit.collider.tag == "Enermy"
                    || hit.collider.tag == "Boss" || hit.collider.tag == "Friendly")
                {
                    Debug.Log("Something");
                    return;
                }
            }

            if (Mathf.Abs(Destination.x + Destination.z) == 1)
            {
                if (rook)
                {
                    if (pawn && Destination.z == -1 && gmr.playerUnits.Any(unit => unit.gameObject == this.gameObject))
                        return;
                    else if (pawn && Destination.z == 1 && gmr.enermyUnits.Any(unit => unit.gameObject == this.gameObject))
                        return;
                    moveSmooth = true;
                    UnitVec = transform.position + Destination;
                    moveCount++;
                    if (moveCount >= moveMaxCount)
                    {
                        moveAble = false;
                        gmr.selectOn = false;
                        gmr.turnMove++;
                        if (gmr.turnMaxMove <= gmr.turnMove)
                        { gmr.canMove = false; }
                        gmrUi.moveCountTxtChange(gmr.turnMaxMove - gmr.turnMove);
                        moving = false;
                    }
                    else if (moveCount < moveMaxCount)
                    {
                        moving = true;
                    }
                }
            }
            else if (Mathf.Abs(Destination.x + Destination.z) == 0
                || Mathf.Abs(Destination.x + Destination.z) == 2)
            {
                if (bishop)
                {
                    if (pawn && Destination.z == -1 && gmr.playerUnits.Any(unit => unit.gameObject == this.gameObject))
                        return;
                    else if (pawn && Destination.z == 1 && gmr.enermyUnits.Any(unit => unit.gameObject == this.gameObject))
                        return;

                    UnitVec = transform.position + Destination;
                    moveCount++;
                    moveSmooth = true;
                    if (moveCount >= moveMaxCount)
                    {
                        moveAble = false;
                        gmr.selectOn = false;
                        gmr.turnMove++;
                        gmrUi.moveCountTxtChange(gmr.turnMaxMove - gmr.turnMove);
                    }
                    else if (moveCount < moveMaxCount)
                    {
                        moving = true;
                    }
                }
            }

        }
        else if (!moveAble)
            Debug.Log("!moveAble");
    }

    //좌우 워프
    public void WarpMove(Vector3 Dest)
    {
        if(cantMove) return;
        if (moveAble && !moveSmooth)
        {
            RaycastHit hit;
            if (Physics.Raycast(Dest+ new Vector3(0,2,0), Vector3.down, out hit, 4f))
            {
                if (hit.collider.tag == "Player" || hit.collider.tag == "Enermy"
                    || hit.collider.tag == "Boss" || hit.collider.tag == "Friendly")
                {
                    Debug.Log("Something");
                    return;
                }
                else if (hit.collider.tag == "Tile")
                {
                    transform.position = new Vector3(Dest.x,transform.position.y,Dest.z);
                    moveAfterFight();
                    warp = false;
                    moveCount++;
                    if (moveCount >= moveMaxCount)
                    {
                        moveAble = false;
                        gmr.selectOn = false;
                        gmr.turnMove++;
                        gmrUi.moveCountTxtChange(gmr.turnMaxMove - gmr.turnMove);
                    }
                    else if (moveCount < moveMaxCount)
                    {
                        moving = true;
                    }
                }
            }
        }
    }

    //위치 교체
    public void changingPosition(GameObject main, GameObject target)
    {
        if (cantMove) return;
        Vector3 dd = main.transform.position;
        transform.position = target.transform.position;
        target.transform.position = dd;
        moveCount++;
        if (moveCount >= moveMaxCount)
        {
            moveAble = false;
            gmr.selectOn = false;
            gmr.turnMove++;
            gmrUi.moveCountTxtChange(gmr.turnMaxMove - gmr.turnMove);
        }
        else if (moveCount < moveMaxCount)
        {
            moving = true;
        }
    }

    public void placeMove(Vector3 placeVec)
    {
        UnitVec = placeVec;
        placementMove = true;
    }

    public void wakeUpUnit()
    {
        gmr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        gmrUi = GameObject.Find("GameMgr").GetComponent<GmrUI>();
        halfNum = (maxNum + minNum) / 2;
        UnitVec = transform.position;
        zeroS = Vector3.zero;
    }
    public void playerFight()
    {
        List<GameObject> gmLi = gmr.enermyUnits.FindAll(unit => Mathf.Abs(unit.transform.position.x - transform.position.x) <= 1);
        List<GameObject> fightEnermyMem = gmLi.FindAll(unit => Mathf.Abs(unit.transform.position.z - transform.position.z) <= 1);
        for (int i = 0; i < fightEnermyMem.Count; i++)
        {
            if (attackEvery)
            {
                fightEnermyMem[i].GetComponent<Units>().maxNum -= attackMinusPow;
                fightEnermyMem[i].GetComponent<Units>().minNum -= attackMinusPow;
                if (fightEnermyMem[i].GetComponent<Units>().maxNum <= 0) fightEnermyMem[i].GetComponent<Units>().maxNum = 0;
                if (fightEnermyMem[i].GetComponent<Units>().minNum <= 0) fightEnermyMem[i].GetComponent<Units>().minNum = 0;
                StartCoroutine(gmr.fighting(this.gameObject, fightEnermyMem[i]));
            }
            else
            {
                if (Mathf.Abs(fightEnermyMem[i].transform.position.x - transform.position.x) + Mathf.Abs(fightEnermyMem[i].transform.position.z - transform.position.z) <= 1.2)
                {
                    fightEnermyMem[i].GetComponent<Units>().maxNum -= attackMinusPow;
                    fightEnermyMem[i].GetComponent<Units>().minNum -= attackMinusPow;
                    if (fightEnermyMem[i].GetComponent<Units>().maxNum <= 0) fightEnermyMem[i].GetComponent<Units>().maxNum = 0;
                    if (fightEnermyMem[i].GetComponent<Units>().minNum <= 0) fightEnermyMem[i].GetComponent<Units>().minNum = 0;
                    StartCoroutine(gmr.fighting(this.gameObject, fightEnermyMem[i]));
                }
            }
        }
    }
    public void enermyFight()
    {
        List<GameObject> gmLi = gmr.playerUnits.FindAll(unit => Mathf.Abs(unit.transform.position.x - transform.position.x) <= 1);
        List<GameObject> fightEnermyMem = gmLi.FindAll(unit => Mathf.Abs(unit.transform.position.z - transform.position.z) <= 1); 
        for (int i = 0; i < fightEnermyMem.Count; i++)
        {
            if (attackEvery)
            {
                fightEnermyMem[i].GetComponent<Units>().maxNum -= attackMinusPow;
                fightEnermyMem[i].GetComponent<Units>().minNum -= attackMinusPow;
                if (fightEnermyMem[i].GetComponent<Units>().maxNum <= 0) fightEnermyMem[i].GetComponent<Units>().maxNum = 0;
                if (fightEnermyMem[i].GetComponent<Units>().minNum <= 0) fightEnermyMem[i].GetComponent<Units>().minNum = 0;
                StartCoroutine(gmr.fighting(fightEnermyMem[i], this.gameObject));
            }
            else
            {
                if (Mathf.Abs(fightEnermyMem[i].transform.position.x - transform.position.x) + Mathf.Abs(fightEnermyMem[i].transform.position.z - transform.position.z) <= 1.2)
                {
                    fightEnermyMem[i].GetComponent<Units>().maxNum -= attackMinusPow;
                    fightEnermyMem[i].GetComponent<Units>().minNum -= attackMinusPow;
                    if (fightEnermyMem[i].GetComponent<Units>().maxNum <= 0) fightEnermyMem[i].GetComponent<Units>().maxNum = 0;
                    if (fightEnermyMem[i].GetComponent<Units>().minNum <= 0) fightEnermyMem[i].GetComponent<Units>().minNum = 0;
                    StartCoroutine(gmr.fighting(fightEnermyMem[i], this.gameObject));
                }
            }
        }
    }

    public void moveAfterFight()
    {
        if (gmr.playerUnits.Any(unit => unit == this.gameObject))
        {
            if (gmr.enermyUnits.Any(unit => Mathf.Abs(unit.transform.position.x - transform.position.x) <= 1 && Mathf.Abs(unit.transform.position.z - transform.position.z) <= 1))
            {
                if (attackEvery)
                {
                    playerFight();
                    moveSmooth = false;
                }
                else
                {
                    if (gmr.enermyUnits.Any(unit => Mathf.Abs(unit.transform.position.x - transform.position.x) + Mathf.Abs(unit.transform.position.z - transform.position.z) <= 1.2))
                    {
                        playerFight();
                        moveSmooth = false;
                    }
                }
            }
        }
        else if (gmr.enermyUnits.Any(unit => unit == this.gameObject))
        {
            if (gmr.playerUnits.Any(unit => Mathf.Abs(unit.transform.position.x - transform.position.x) <= 1 && Mathf.Abs(unit.transform.position.z - transform.position.z) <= 1))
            {
                if (attackEvery)
                {
                    enermyFight();
                    moveSmooth = false;
                }
                else
                {
                    if (gmr.playerUnits.Any(unit => Mathf.Abs(unit.transform.position.x - transform.position.x) + Mathf.Abs(unit.transform.position.z - transform.position.z) <= 1.2))
                    {
                        enermyFight();
                        moveSmooth = false;
                    }
                }
            }
        }
    }

    public void UpgradeThisUnit()
    {
        //Dictionary<string, object> DictFirst = libmgr.unitCode[libmgr.bossToMeet[libmgr.stageLevelCount][0]];
        //Dictionary<string, object> Dict = libmgr.unitCode[DictFirst["기본"]];
        //fU.name = Dict["Name"].ToString();
        //fU.transform.position = new Vector3(mapMaxX - i + 3, 3, mapMaxZ + 1);

        //Units fUFun = fU.GetComponent<Units>();
        //fUFun.myCodeNum = DictFirst["기본"];
        //fUFun.upgradeRank = libmgr.stageLevelCount;
        //fUFun.moveCountUpgrade = Convert.ToSingle(Dict["이동횟수 강화"]);
        //fUFun.minNumUpgrade = Convert.ToSingle(Dict["숫자 최소치 강화"]);
        //fUFun.maxNumUpgrade = Convert.ToSingle(Dict["숫자최대치 강화"]);
        //fUFun.moveMaxCount = (int)Dict["이동 횟수"] + (int)fUFun.moveCountUpgrade * fUFun.upgradeRank;
        //fUFun.minNum = (int)Dict["숫자 최소치"] + (int)fUFun.minNumUpgrade * fUFun.upgradeRank;
        //fUFun.maxNum = (int)Dict["숫자 최대치"] + (int)fUFun.maxNumUpgrade * fUFun.upgradeRank;
        //fUFun.unit2DImage = Resources.Load<Sprite>("Image2D/" + fUFun.myCodeNum);

        //Dictionary<string, object> DictEffect = libmgr.unitCodeEffects[DictFirst["기본"]];
        //if (Convert.ToString(DictEffect["대각선만 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = false; }
        //if (Convert.ToString(DictEffect["전방위 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = true; }

        //if (fUFun.rook && !fUFun.bishop)
        //    fUFun.unitEffectTxt = "룩";
        //else if (!fUFun.rook && fUFun.bishop)
        //    fUFun.unitEffectTxt = "비숍";
        //else if (fUFun.rook && fUFun.bishop)
        //    fUFun.unitEffectTxt = "퀸";

        //if (Convert.ToString(DictEffect["뒤로 이동 불가"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 폰";
        //    fUFun.pawn = true;
        //}
        //if (Convert.ToString(DictEffect["홀수 차례만 효과"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 홀수 차례";
        //    fUFun.oddTurnEffect = true;
        //}
        //if (Convert.ToString(DictEffect["짝수 차례만 효과"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 짝수 차례";
        //    fUFun.evenTurnEffect = true;
        //}
        //if (Convert.ToString(DictEffect["좌우 워프"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 워프";
        //    fUFun.warp = true;
        //}
        //if (Convert.ToString(DictEffect["전방향 전투"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 전방향 전투";
        //    fUFun.attackEvery = true;
        //}
        //if (Convert.ToString(DictEffect["위치 교체"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 교대";
        //    fUFun.changePos = true;
        //}
        //if (Convert.ToString(DictEffect["공격시 상대 숫자 변동"]) != "")
        //{
        //    fUFun.unitEffectTxt += ", 저주";
        //    fUFun.attackMinusPow = Convert.ToInt32(DictEffect["공격시 상대 숫자 변동"]);
        //}
        //if (Convert.ToString(DictEffect["자신 숫자 변동"]) != "")
        //{
        //    fUFun.unitEffectTxt += ", 축복";
        //    fUFun.morePower = Convert.ToInt32(DictEffect["자신 숫자 변동"]);
        //}
        //if (Convert.ToString(DictEffect["죽으면 패배"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 심장";
        //    fUFun.playerHeart = true;
        //}
        //if (Convert.ToString(DictEffect["위치 고정"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 이동 불가";
        //    fUFun.cantMove = true;
        //}
        //if (Convert.ToString(DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"]) != "")
        //{
        //    fUFun.unitEffectTxt += ", 업그레이드";
        //    fUFun.upgradeCode = DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"];
        //}
        //if (Convert.ToString(DictEffect["자신 진영에서만 이동 가능"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 士";
        //    fUFun.onlyMyPlace = true;
        //}
        //if (Convert.ToString(DictEffect["중립진영에 배치"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 정찰병";
        //    fUFun.placeAnywhere = true;
        //}
        //if (Convert.ToString(DictEffect["못 움직임 사망시"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 독";
        //    fUFun.poision = true;
        //}
        //if (Convert.ToString(DictEffect["숫자 범위 변하지 않음"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 숫자 변동 없음";
        //    fUFun.cristalBody = true;
        //}
        //if (Convert.ToString(DictEffect["차례 종료시 랜덤 이동"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 랜덤 이동";
        //    fUFun.randomMove = true;
        //}
        //if (Convert.ToString(DictEffect["다른 아군 숫자 변동"]) != "")
        //{
        //    fUFun.unitEffectTxt += ", 아군 축복";
        //    fUFun.powerUpTotem = Convert.ToInt32(DictEffect["다른 아군 숫자 변동"]);
        //}
        //if (Convert.ToString(DictEffect["못 움직임 이동 대신"]) == "TRUE")
        //{
        //    fUFun.unitEffectTxt += ", 빙결";
        //    fUFun.frozen = true;
        //}

        //fUFun.wakeUpUnit();
        //enermyUnits.Add(fU);
    }
    
    //홀짝 구현 안됨, 그로 인해 자신 숫자 변동도 구현 안됨 + 뒤집기 + 자신진영에서만 이동가능, 독(사망시 상대 한턴 못 움직임), 숫자 범위 변하지 않음, 랜덤 이동, 빙결, 못움직임
}
