using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Units : MonoBehaviour
{
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

    public bool placeAnywhere = false;

    public bool moveSmooth = false;
    public bool placementMove = false;

    Vector3 UnitVec;
    Vector3 zeroS;

    
    void Start()
    {
        //gmr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        //halfNum = (maxNum + minNum)/ 2;
        //UnitVec = transform.position;
        //zeroS = Vector3.zero;
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
    }

    public void Move(Vector3 Destination)
    {
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
                    }
                    else if (moveCount < moveMaxCount)
                    { } // 이 유닛을 움직이지 않으면 moveAble = false로
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
                    { }// 이 유닛을 움직이지 않으면 moveAble = false로
                }
            }

        }
        else if (!moveAble)
            Debug.Log("!moveAble");
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
    // 바닥 타일 상관 없이 이동하는 문제

}
