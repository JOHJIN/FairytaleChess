using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameMgr : MonoBehaviour
{
    public LibMgr libmgr;

    public int turnMove = 0;
    public int turnMaxMove = 2;

    public GameObject selectUnit;
    public bool selectOn = false;

    public int mapMaxX = 7;
    public int mapMaxZ = 7;
    public int friendlyZone = 2;
    public GameObject normalTiles;
    public GameObject friendlyTiles;
    public GameObject enermyTiles;
    public GameObject tileInMap;

    public bool playerTurn = true;
    public float turnTime = 0;

    public bool placementTime = true;

    public GameObject playerBass;
    public GameObject friendlyBass;
    public GameObject bossBass;
    public GameObject enermyBass;
    public List<GameObject> playerUnits;
    public List<GameObject> enermyUnits;
    public GmrUI gmrUi;
    public GameFlow gmFlow;

    public int enermyPlaceCount = 0;
    public int luckCount = 0;

    public Camera mainCam;

    int rndbossplace = 0;
    void Start()
    {
        StartCoroutine(gmrStartCorou());
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 15f))
            {
                if (hit.collider.tag == "Friendly" || hit.collider.tag == "Player")
                {
                    selectUnit = hit.collider.gameObject;
                    selectOn = true;
                }
                else if (hit.collider.tag == "Enermy" || hit.collider.tag == "Boss")
                {
                    selectUnit = hit.collider.gameObject;
                    selectOn = false;
                }
                else if (selectOn && hit.collider.tag == "Tile")
                {
                    if (hit.collider)
                    {
                        //��ġŸ��
                        if (placementTime)
                        {
                            if (selectUnit.GetComponent<Units>().placeAnywhere 
                                && hit.collider.transform.position.z <= mapMaxZ - friendlyZone)
                            {
                                selectUnit.transform.position =
                                new Vector3(hit.collider.transform.position.x,
                                0, hit.collider.transform.position.z);
                                selectUnit = null;
                                selectOn = false;
                            }
                            else if (!selectUnit.GetComponent<Units>().placeAnywhere
                                && hit.collider.transform.position.z <= friendlyZone)
                            {
                                selectUnit.transform.position =
                                new Vector3(hit.collider.transform.position.x,
                                0, hit.collider.transform.position.z);
                                selectUnit = null;
                                selectOn = false;
                            }

                        }
                        //���� �� Ŭ������ ���� �̵� ���
                        else
                        {
                            float minusX = hit.collider.transform.position.x -
                                    selectUnit.transform.position.x;
                            float minusZ = hit.collider.transform.position.z -
                                selectUnit.transform.position.z;
                            if (Mathf.Abs(minusX) <= 1.2 && Mathf.Abs(minusZ) <= 1.2)
                            {
                                //���
                                if (Mathf.Abs(minusX) + Mathf.Abs(minusZ) >= 2)
                                {
                                    if (minusX > 0.4 && minusZ > 0.4)
                                    {
                                        unitMove(1, 1);
                                    }
                                    if (minusX < -0.4 && minusZ > 0.4)
                                    {
                                        unitMove(-1, 1);
                                    }
                                    if (minusX > 0.4 && minusZ < -0.4)
                                    {
                                        unitMove(1, -1);
                                    }
                                    if (minusX < -0.4 && minusZ < -0.4)
                                    {
                                        unitMove(-1, -1);
                                    }
                                    else 
                                    {
                                        selectUnit = null;
                                        selectOn = false;
                                    }
                                }
                                //��
                                else
                                {
                                    if (minusX > 0.4 && Mathf.Abs(minusZ) < 0.4)
                                    {
                                        unitMove(1, 0);
                                    }
                                    if (minusX < -0.4 && Mathf.Abs(minusZ) < 0.4)
                                    {
                                        unitMove(-1, 0);
                                    }
                                    if (Mathf.Abs(minusX) < 0.4 && minusZ > 0.4)
                                    {
                                        unitMove(0, 1);
                                    }
                                    if (Mathf.Abs(minusX) < 0.4 && minusZ < -0.4)
                                    {
                                        unitMove(0, -1);
                                    }
                                    else
                                    {
                                        selectUnit = null;
                                        selectOn = false;
                                    }
                                }
                            }
                            else
                            {
                                selectUnit = null;
                                selectOn = false;
                            }
                        }
                    }                  
                    else
                    {
                        selectUnit = null;
                        selectOn = false; 
                    }
                }
                else
                {
                    selectUnit = null;
                    selectOn = false;
                }
            }
        }

        //�̵�Ű, Ű�е�� ���� �̵�
        if (selectOn && !placementTime)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                unitMove(-1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                unitMove(1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                unitMove(0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                unitMove(0, -1);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                unitMove(-1, -1);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                unitMove(1, -1);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                unitMove(-1, 1);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                unitMove(1, 1);
            }

            //if (placementTime && Input.GetMouseButton(0))
            //{ 
            //}
        }

        //�� �ð� ����
        if (playerTurn && !placementTime)
        {
            turnTime += Time.deltaTime;
            if (turnTime >= 120f)
            {
                gmrUi.whosTurnTxtChange();
                playerTurn = false;
                turnMove = 0;
                selectOn = false;
                turnTime = 0;
                gmrUi.moveCountTxtChange(turnMaxMove - turnMove);
            }
        }
    }

    public void unitMove(int unitMoveX, int unitMoveZ)
    {
        Vector3 go = new Vector3(unitMoveX, 0, unitMoveZ);
        selectUnit.GetComponent<Units>().Move(go);
    }

    public void PlayerTurnOverBtn()
    {
        //��ġ ������ �� ��ư Ŭ��
        if (placementTime)
        {
            StartCoroutine(placementOverCorou());
            StartCoroutine(startAndDestroy());
        }

        // �� ����� ��ư Ŭ��
        if(playerTurn && !placementTime)
        {
            gmrUi.whosTurnTxtChange();
            turnTime = 0;
            playerTurn = false;
            turnMove = 0;
            selectOn = false;
            gmrUi.moveCountTxtChange(turnMaxMove - turnMove);
        }
    }
    
    IEnumerator gmrStartCorou() // ���� �ڷ�ƾ / ĳ���� ��ȯ
    {
        libmgr = GameObject.Find("LibMgr").GetComponent<LibMgr>();

        GameObject p1 = Instantiate(playerBass);
        Dictionary<string, object> playerDict = libmgr.unitCode[libmgr.playerUnitsData[0][0]];
        p1.name = playerDict["Name"].ToString();
        p1.transform.position = new Vector3(0, 3, 2);
        p1.GetComponent<Units>().wakeUpUnit();
        p1.GetComponent<Units>().upgradeRank = (int)libmgr.playerUnitsData[0][1];
        playerUnits.Add(p1);

        for (int i = 1; i < libmgr.playerUnitsData.Count; i++)
        {
            GameObject fU = Instantiate(friendlyBass);
            Dictionary<string, object> Dict = libmgr.unitCode[libmgr.playerUnitsData[i][0]];
            fU.name = Dict["Name"].ToString();
            fU.transform.position = new Vector3(i, 3, 2);

            Units fUFun = fU.GetComponent<Units>();
            fUFun.upgradeRank = (int)libmgr.playerUnitsData[i][1];
            fUFun.moveCountUpgrade = Convert.ToSingle(Dict["�̵�Ƚ�� ��ȭ"]);
            fUFun.minNumUpgrade = Convert.ToSingle(Dict["���� �ּ�ġ ��ȭ"]);
            fUFun.maxNumUpgrade = Convert.ToSingle(Dict["�����ִ�ġ ��ȭ"]);
            fUFun.moveMaxCount = (int)Dict["�̵� Ƚ��"] + (int)fUFun.moveCountUpgrade *fUFun.upgradeRank;
            fUFun.minNum = (int)Dict["���� �ּ�ġ"] + (int)fUFun.minNumUpgrade *fUFun.upgradeRank;
            fUFun.maxNum = (int)Dict["���� �ִ�ġ"] + (int)fUFun.maxNumUpgrade *fUFun.upgradeRank;

            fUFun.wakeUpUnit();
            playerUnits.Add(fU);
        }
        placementTime = true;
        yield return StartCoroutine(stageLoad(libmgr.stageLevelCount));
        yield return StartCoroutine(makeEnermyUnits());
        yield return StartCoroutine(unitplacementCoro());
    }
    
    //��ġ ������ �÷����� ���� �ƴ� ���ʹ� ������ �Ѿ�� ���� �ذ� ���ؼ� �ڷ�ƾ���� ����
    IEnumerator placementOverCorou()
    {
        gmrUi.whosTurnTxtChange();
        yield return null;
        placementTime = false;
        turnMove = 0;
        turnTime = 0;
        selectOn = false;
        playerTurn = true;
        yield return new WaitForSecondsRealtime(0.5f);
    }

    //��ġ ������ ��ġ ���� ���� �ı�
    IEnumerator startAndDestroy()
    {
        for (int i = 0; i < playerUnits.Count; i++)
        {
            if (playerUnits[i].transform.position.y > 2.5f)
            {
                Destroy(playerUnits[i]);
                playerUnits.RemoveAt(i);
                i--;
            }
        }
        yield return null;
    }

    //�� ���� 
    public IEnumerator stageLoad(int sLevel)
    {
        Dictionary<string, object> bossMapDict = libmgr.bossCodeStage[libmgr.bossToMeet[sLevel][0]];
        mapMaxX = (int)bossMapDict["MapMaxX"];
        mapMaxZ = (int)bossMapDict["MapMaxZ"];
        friendlyZone = (int)bossMapDict["FriendlyZone"];
        yield return null;
        //�� �� ����
        GameObject tileMap = Instantiate(tileInMap);
        tileMap.transform.position = new Vector3(3, -0.5f, 3);
        for (int i = 1; i <= mapMaxX; i++)
        {
            for (int j = 1; j <= mapMaxZ; j++)
            {
                if (j <= friendlyZone)
                {
                    GameObject zTile = Instantiate(friendlyTiles, tileMap.transform);
                    zTile.transform.position = new Vector3(i, -0.5f, j);
                }
                else if (j > mapMaxZ - friendlyZone)
                {
                    GameObject zTile = Instantiate(enermyTiles, tileMap.transform);
                    zTile.transform.position = new Vector3(i, -0.5f, j);
                }
                else
                {
                    GameObject zTile = Instantiate(normalTiles, tileMap.transform);
                    zTile.transform.position = new Vector3(i, -0.5f, j);
                }
            }
        }
    }
    //�� ���� ����
    //���� ����
    public IEnumerator makeEnermyUnits()
    {
        Debug.Log(0);

        GameObject BossfU = Instantiate(bossBass);
        Dictionary<string, object> DictBoss = libmgr.unitCode[libmgr.bossToMeet[libmgr.stageLevelCount][0]];
        BossfU.name = DictBoss["Name"].ToString();
        BossfU.GetComponent<Units>().wakeUpUnit();
        BossfU.transform.position = new Vector3(mapMaxX+3, 3, mapMaxZ + 1);
        enermyUnits.Add(BossfU);

        Debug.Log(1);

        yield return null;

        //�� �⺻ ����
        for (int i = 1; i < libmgr.stageLevelCount + 2; i++)
        {
            GameObject fU = Instantiate(enermyBass);
            Dictionary<string, object> DictFirst = libmgr.unitCode[libmgr.bossToMeet[libmgr.stageLevelCount][0]];
            Dictionary<string, object> Dict = libmgr.unitCode[DictFirst["�⺻"]];
            fU.name = Dict["Name"].ToString();
            fU.transform.position = new Vector3(mapMaxX-i+3, 3, mapMaxZ + 1);

            Units fUFun = fU.GetComponent<Units>();
            fUFun.upgradeRank = libmgr.stageLevelCount;
            fUFun.moveCountUpgrade = Convert.ToSingle(Dict["�̵�Ƚ�� ��ȭ"]);
            fUFun.minNumUpgrade = Convert.ToSingle(Dict["���� �ּ�ġ ��ȭ"]);
            fUFun.maxNumUpgrade = Convert.ToSingle(Dict["�����ִ�ġ ��ȭ"]);
            fUFun.moveMaxCount = (int)Dict["�̵� Ƚ��"] + (int)fUFun.moveCountUpgrade * fUFun.upgradeRank;
            fUFun.minNum = (int)Dict["���� �ּ�ġ"] + (int)fUFun.minNumUpgrade * fUFun.upgradeRank;
            fUFun.maxNum = (int)Dict["���� �ִ�ġ"] + (int)fUFun.maxNumUpgrade * fUFun.upgradeRank;

            Debug.Log(2);

            fUFun.wakeUpUnit();
            enermyUnits.Add(fU);
        }
        // �� ���� ���� 3��
        if (libmgr.stageLevelCount >= 3)
        {
            GameObject fU = Instantiate(enermyBass);
            Dictionary<string, object> UniqueDictFirst = libmgr.unitCode[libmgr.bossToMeet[libmgr.stageLevelCount][0]];
            Dictionary<string, object> UniqueDict = libmgr.unitCode[UniqueDictFirst["����"]];
            fU.name = UniqueDict["Name"].ToString();
            fU.transform.position = new Vector3(mapMaxX - enermyUnits.Count+3, 3, mapMaxZ + 1);

            Units fUFun = fU.GetComponent<Units>();
            fUFun.upgradeRank = libmgr.stageLevelCount;
            fUFun.moveCountUpgrade = Convert.ToSingle(UniqueDict["�̵�Ƚ�� ��ȭ"]);
            fUFun.minNumUpgrade = Convert.ToSingle(UniqueDict["���� �ּ�ġ ��ȭ"]);
            fUFun.maxNumUpgrade = Convert.ToSingle(UniqueDict["�����ִ�ġ ��ȭ"]);
            fUFun.moveMaxCount = (int)UniqueDict["�̵� Ƚ��"] + (int)fUFun.moveCountUpgrade * fUFun.upgradeRank;
            fUFun.minNum = (int)UniqueDict["���� �ּ�ġ"] + (int)fUFun.minNumUpgrade * fUFun.upgradeRank;
            fUFun.maxNum = (int)UniqueDict["���� �ִ�ġ"] + (int)fUFun.maxNumUpgrade * fUFun.upgradeRank;

            fUFun.wakeUpUnit();
            enermyUnits.Add(fU);
        }
        //�� ���� ���� 6��
        if (libmgr.stageLevelCount >= 6)
        {
            GameObject fU = Instantiate(enermyBass);
            Dictionary<string, object> UniqueDictFirst = libmgr.unitCode[libmgr.bossToMeet[libmgr.stageLevelCount][0]];
            Dictionary<string, object> UniqueDict = libmgr.unitCode[UniqueDictFirst["����"]];
            fU.name = UniqueDict["Name"].ToString();
            fU.transform.position = new Vector3(mapMaxX - enermyUnits.Count + 3, 3, mapMaxZ + 1);

            Units fUFun = fU.GetComponent<Units>();
            fUFun.upgradeRank = libmgr.stageLevelCount;
            fUFun.moveCountUpgrade = Convert.ToSingle(UniqueDict["�̵�Ƚ�� ��ȭ"]);
            fUFun.minNumUpgrade = Convert.ToSingle(UniqueDict["���� �ּ�ġ ��ȭ"]);
            fUFun.maxNumUpgrade = Convert.ToSingle(UniqueDict["�����ִ�ġ ��ȭ"]);
            fUFun.moveMaxCount = (int)UniqueDict["�̵� Ƚ��"] + (int)fUFun.moveCountUpgrade * fUFun.upgradeRank;
            fUFun.minNum = (int)UniqueDict["���� �ּ�ġ"] + (int)fUFun.minNumUpgrade * fUFun.upgradeRank;
            fUFun.maxNum = (int)UniqueDict["���� �ִ�ġ"] + (int)fUFun.maxNumUpgrade * fUFun.upgradeRank;

            fUFun.wakeUpUnit();
            enermyUnits.Add(fU);
        }
        mainCam.transform.position = new Vector3(mapMaxX / 2 + 0.5f, mapMaxZ + 2, mapMaxZ / 2);
    }

    //���� ��ġ
    public IEnumerator unitplacementCoro()
    {
        yield return new WaitForSeconds(0.05f);
        rndbossplace = UnityEngine.Random.Range(1, mapMaxX + 1);
        enermyUnits[0].GetComponent<Units>().placeMove(new Vector3(rndbossplace, enermyUnits[0].transform.position.y, mapMaxZ));
    }
    public void unitplacementMore()
    {
        for (int k = 0; k < 1; k++)
        {
            enermyPlaceCount++;
            if (enermyPlaceCount >= enermyUnits.Count)
                break;
            if (enermyUnits[enermyPlaceCount].GetComponent<Units>().placeAnywhere)
            {
                int rnd = UnityEngine.Random.Range(1, mapMaxX + 1);
                int rnd2 = UnityEngine.Random.Range(friendlyZone + 1, mapMaxZ + 1);

                Vector3 upUnitPlace = new Vector3(rnd, 3, rnd2);
                if (Physics.Raycast(upUnitPlace, Vector3.down, out RaycastHit hit, 15f))
                {
                    if (hit.collider.tag == "Tile")
                    {
                        enermyUnits[enermyPlaceCount].GetComponent<Units>().placeMove(new Vector3(rnd, enermyUnits[enermyPlaceCount].transform.position.y, rnd2));
                        luckCount = 0;
                    }
                    else
                    {
                        k--;
                        enermyPlaceCount--;
                        luckCount++;
                        if (luckCount >= 30)
                        {
                            Destroy(enermyUnits[enermyPlaceCount]);
                            enermyUnits.RemoveAt(enermyPlaceCount);
                            luckCount = 0;
                        }
                    }
                }
            }
            else
            {
                Debug.Log(4);

                int rnd = UnityEngine.Random.Range(1, mapMaxX + 1);
                int rnd2 = UnityEngine.Random.Range(0, friendlyZone);
                Vector3 upUnitPlace = new Vector3(rnd, 10, mapMaxZ - rnd2);
                if (Physics.Raycast(upUnitPlace, Vector3.down, out RaycastHit hit, 15f))
                {
                    Debug.DrawRay(upUnitPlace, Vector3.down * 15, Color.red, 10f);
                    if (hit.collider.tag == "Tile")
                    {
                        Debug.Log(enermyUnits[0].transform.position);
                        Debug.Log(hit.collider);
                        Debug.Log(hit.collider.transform.position);
                        enermyUnits[enermyPlaceCount].GetComponent<Units>().placeMove(new Vector3(rnd, enermyUnits[enermyPlaceCount].transform.position.y, mapMaxZ - rnd2));
                        luckCount = 0;
                    }
                    else
                    {
                        Debug.Log("p2");
                        Debug.Log(hit.collider);
                        k--;
                        enermyPlaceCount--;
                        luckCount++;
                        if (luckCount >= 30)
                        {
                            Destroy(enermyUnits[enermyPlaceCount]);
                            enermyUnits.RemoveAt(enermyPlaceCount);
                            luckCount = 0;
                        }
                    }
                }
            }
        }
    }

    //    Debug.Log(3);
    //    int luckCount = 0;
    //    //�⺻ ���� ��ġ
    //    for (int k = 1; k < enermyUnits.Count; k++)
    //    {
    //        if (enermyUnits[k].GetComponent<Units>().placeAnywhere)
    //        {
    //            int rnd = UnityEngine.Random.Range(1, mapMaxX + 1);
    //            int rnd2 = UnityEngine.Random.Range(friendlyZone + 1, mapMaxZ + 1);

    //            Vector3 upUnitPlace = new Vector3(rnd, 3, rnd2);
    //            if (Physics.Raycast(upUnitPlace, Vector3.down, out RaycastHit hit, 15f))
    //            {
    //                if (hit.collider.tag == "Tile")
    //                {
    //                    enermyUnits[k].transform.position = new Vector3(rnd, 0, rnd2);
    //                    luckCount = 0;
    //                    yield return new WaitForSeconds(0.05f);
    //                }
    //                else
    //                {
    //                    k--;
    //                    luckCount++;
    //                    if (luckCount >= 30)
    //                    {
    //                        Destroy(enermyUnits[k]);
    //                        enermyUnits.RemoveAt(k);
    //                        luckCount = 0;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            Debug.Log(4);

    //            //int layerMask = (-1) - (1 << LayerMask.NameToLayer("PlayerUnits"));
    //            int rnd = UnityEngine.Random.Range(1, mapMaxX + 1);
    //            int rnd2 = UnityEngine.Random.Range(0, friendlyZone);
    //            Vector3 upUnitPlace = new Vector3(rnd, 10, mapMaxZ - rnd2);
    //            if (Physics.Raycast(upUnitPlace, Vector3.down, out RaycastHit hit, 15f))
    //            {
    //                Debug.DrawRay(upUnitPlace, Vector3.down * 15, Color.red, 10f);
    //                if (hit.collider.tag == "Tile")
    //                {
    //                    Debug.Log(enermyUnits[0].transform.position);
    //                    Debug.Log(hit.collider);
    //                    Debug.Log(hit.collider.transform.position);
    //                    enermyUnits[k].transform.position = new Vector3(rnd, 0, mapMaxZ - rnd2);
    //                    luckCount = 0;
    //                    yield return new WaitForSeconds(0.05f);
    //                }
    //                else
    //                {
    //                    Debug.Log("p2");
    //                    Debug.Log(hit.collider);
    //                    k--;
    //                    luckCount++;
    //                    if (luckCount >= 30)
    //                    {
    //                        Destroy(enermyUnits[k]);
    //                        enermyUnits.RemoveAt(k);
    //                        luckCount = 0;
    //                    }
    //                }
    //            }
    //        }

    //    }
    //    Debug.Log(5);
    //}
}
