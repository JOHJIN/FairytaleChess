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

    public bool nextMoveFalse = false;

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
                if(upgradeCode != null)
                {
                    if (this.transform.position.z == gmr.mapMaxZ && gmr.playerUnits.Any(unit => unit == this.gameObject))
                    {
                        if (moving)
                        {
                            moveAble = false;
                            gmrUi.moveCountTxtChange(gmr.turnMaxMove - gmr.turnMove);
                            moving = false;
                        }
                        upgrading();
                    }
                    else if (this.transform.position.z == 1 && gmr.enermyUnits.Any(unit => unit == this.gameObject))
                    {
                        if (moving)
                        {
                            moveAble = false;
                            gmrUi.moveCountTxtChange(gmr.turnMaxMove - gmr.turnMove);
                            moving = false;
                        }
                        upgrading();
                    }
                }
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

        if (gmr.selectUnit == this.gameObject)
        {
            selectEffect.SetActive(true);
        }
        else
        {
            selectEffect.SetActive(false);
        }

        //ÀÌµ¿È½¼ö ³²¾ÒÀ» ¶§ ´Ù¸¥ À¯´Ö ¼±ÅÃ½Ã ÀÌµ¿ È½¼ö 0
        if (moving)
        {
            if (this.gameObject != null)
            {
                if (!gmr.selectUnit == this.gameObject)
                {
                    if (gmr.selectUnit.tag == "Friendly" || gmr.selectUnit.tag == "Player" || !gmr.playerTurn || gmr.selectUnit == null)
                    {
                        moveAble = false;
                        gmr.turnMove++;
                        gmrUi.moveCountTxtChange(gmr.turnMaxMove - gmr.turnMove);
                        moving = false;
                    }
                }
            }
            else if (this.gameObject == null)
            {
                moveAble = false;
                gmr.turnMove++;
                gmrUi.moveCountTxtChange(gmr.turnMaxMove - gmr.turnMove);
                moving = false;
                Destroy(this);
            }
        }
    }

    //ÀÏ¹Ý ÀÌµ¿
    public void Move(Vector3 Destination)
    {
        if (cantMove)
            return;
        if (pawn && Destination.z == -1 && gmr.playerUnits.Any(unit => unit.gameObject == this.gameObject))
            return;
        else if (pawn && Destination.z == 1 && gmr.enermyUnits.Any(unit => unit.gameObject == this.gameObject))
            return;
        if (onlyMyPlace && Destination.z == 1 && gmr.playerUnits.Any(unit => unit.gameObject == this.gameObject) && transform.position.z >= gmr.friendlyZone)
            return;
        else if (onlyMyPlace && Destination.z == -1 && gmr.enermyUnits.Any(unit => unit.gameObject == this.gameObject) && transform.position.z >= gmr.friendlyZone)
            return;

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

    //ÁÂ¿ì ¿öÇÁ
    public void WarpMove(Vector3 Dest)
    {
        if (cantMove) return;
        if (moveAble && !moveSmooth)
        {
            RaycastHit hit;
            if (Physics.Raycast(Dest + new Vector3(0, 2, 0), Vector3.down, out hit, 4f))
            {
                if (hit.collider.tag == "Player" || hit.collider.tag == "Enermy"
                    || hit.collider.tag == "Boss" || hit.collider.tag == "Friendly")
                {
                    Debug.Log("Something");
                    return;
                }
                else if (hit.collider.tag == "Tile")
                {
                    transform.position = new Vector3(Dest.x, transform.position.y, Dest.z);
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

    //À§Ä¡ ±³Ã¼
    public void changingPosition(GameObject main, GameObject target)
    {
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
                if (!fightEnermyMem[i].GetComponent<Units>().cristalBody)
                {
                    fightEnermyMem[i].GetComponent<Units>().maxNum -= attackMinusPow;
                    fightEnermyMem[i].GetComponent<Units>().minNum -= attackMinusPow;
                }
                if (fightEnermyMem[i].GetComponent<Units>().maxNum <= 0) fightEnermyMem[i].GetComponent<Units>().maxNum = 0;
                if (fightEnermyMem[i].GetComponent<Units>().minNum <= 0) fightEnermyMem[i].GetComponent<Units>().minNum = 0;
                StartCoroutine(gmr.fighting(this.gameObject, fightEnermyMem[i]));
            }
            else
            {
                if (Mathf.Abs(fightEnermyMem[i].transform.position.x - transform.position.x) + Mathf.Abs(fightEnermyMem[i].transform.position.z - transform.position.z) <= 1.2)
                {
                    if (!fightEnermyMem[i].GetComponent<Units>().cristalBody)
                    {
                        fightEnermyMem[i].GetComponent<Units>().maxNum -= attackMinusPow;
                        fightEnermyMem[i].GetComponent<Units>().minNum -= attackMinusPow;
                    }
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
                if (!fightEnermyMem[i].GetComponent<Units>().cristalBody)
                {
                    fightEnermyMem[i].GetComponent<Units>().maxNum -= attackMinusPow;
                    fightEnermyMem[i].GetComponent<Units>().minNum -= attackMinusPow;
                }
                if (fightEnermyMem[i].GetComponent<Units>().maxNum <= 0) fightEnermyMem[i].GetComponent<Units>().maxNum = 0;
                if (fightEnermyMem[i].GetComponent<Units>().minNum <= 0) fightEnermyMem[i].GetComponent<Units>().minNum = 0;
                StartCoroutine(gmr.fighting(fightEnermyMem[i], this.gameObject));
            }
            else
            {
                if (Mathf.Abs(fightEnermyMem[i].transform.position.x - transform.position.x) + Mathf.Abs(fightEnermyMem[i].transform.position.z - transform.position.z) <= 1.2)
                {
                    if (!fightEnermyMem[i].GetComponent<Units>().cristalBody)
                    {
                        fightEnermyMem[i].GetComponent<Units>().maxNum -= attackMinusPow;
                        fightEnermyMem[i].GetComponent<Units>().minNum -= attackMinusPow;
                    }
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

    public void upgrading()
    {
        if (upgradeCode != null)
        {
            gmr.UpgradeMapUnit(this.gameObject);
        }
    }
}
    
    //È¦Â¦ ±¸Çö ¾ÈµÊ, ·£´ý ÀÌµ¿ ±¸Çö ¾ÈµÊ
    //È¦Â¦ È¦ false || ÅÏ Â¦
