using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Units : MonoBehaviour
{
    public object myCodeNum;
    public object upgradeCode;

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
    void Start()
    {
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
                    if (pawn && Destination.z == -1)
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
                    if (pawn && Destination.z == -1)
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
            fightEnermyMem[i].GetComponent<Units>().maxNum -= attackMinusPow;
            fightEnermyMem[i].GetComponent<Units>().minNum -= attackMinusPow;
            if (fightEnermyMem[i].GetComponent<Units>().maxNum <= 0) fightEnermyMem[i].GetComponent<Units>().maxNum = 0;
            if (fightEnermyMem[i].GetComponent<Units>().minNum <= 0) fightEnermyMem[i].GetComponent<Units>().minNum = 0;
            StartCoroutine(gmr.fighting(this.gameObject, fightEnermyMem[i]));
        }
    }
    public void enermyFight()
    {
        List<GameObject> gmLi = gmr.playerUnits.FindAll(unit => Mathf.Abs(unit.transform.position.x - transform.position.x) <= 1);
        List<GameObject> fightEnermyMem = gmLi.FindAll(unit => Mathf.Abs(unit.transform.position.z - transform.position.z) <= 1);
        fightEnermyMem.ForEach(gg => StartCoroutine(gmr.fighting(gg, this.gameObject)));
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
    
    //홀짝 구현 안됨, 그로 인해 자신 숫자 변동도 구현 안됨 + 죽으면 게임 패배 구현 안됨 + 뒤집기 부터 시작
}
