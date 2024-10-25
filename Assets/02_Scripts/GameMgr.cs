using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class GameMgr : MonoBehaviour
{
    public LibMgr libmgr;

    public int turnMove = 0;
    public int turnMaxMove = 2;
    public bool canMove = true;

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

    public Text playerdiceTxt;
    public Text enermydiceTxt;
    public GameObject FightPanel;
    public GameObject winPanel;
    public bool win = false;
    public GameObject losePanel;
    public bool lose = false;

    public GameObject move1EnermyAi;
    public GameObject targetPlayerUnit1Ai;

    public bool AiTurn = false;
    public bool aiSelect = false;
    float aiTime = 0;
    public int bosstype = 0;

    public Image playerUnitFight2D;
    public Image enermyUnitFight2D;
    public Image winplayerUnitFight2D;
    public Image winenermyUnitFight2D;
    public Image loseplayerUnitFight2D;
    public Image loseenermyUnitFight2D;
    public Text moneyTxt;
    void Start()
    {
        StartCoroutine(gmrStartCorou());
        bosstype = UnityEngine.Random.Range(-1, 2);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 15f))
            {
                if (hit.collider.tag == "Friendly" || hit.collider.tag == "Player")
                {
                    if (selectOn && selectUnit.GetComponent<Units>().changePos)
                    {
                        selectUnit.GetComponent<Units>().changingPosition(selectUnit, hit.collider.gameObject);
                    }
                    else 
                    { 
                        selectUnit = hit.collider.gameObject;
                        selectOn = true; 
                    }
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
                        //배치타임
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
                        //게임 중 클릭으로 유닛 이동 기능
                        else if (!placementTime && canMove)
                        {
                            float minusX = hit.collider.transform.position.x -
                                    selectUnit.transform.position.x;
                            float minusZ = hit.collider.transform.position.z -
                                selectUnit.transform.position.z;
                            if (Mathf.Abs(minusX) <= 1.3 && Mathf.Abs(minusZ) <= 1.3 && !playerUnits.Any(s => s.GetComponent<Units>().moveSmooth == true))
                            {
                                //비숍
                                if (Mathf.Abs(minusX) + Mathf.Abs(minusZ) >= 2)
                                {
                                    if (minusX > 0.5 && minusZ > 0.5)
                                    {
                                        unitMove(1, 1);
                                    }
                                    else if (minusX < -0.5 && minusZ > 0.5)
                                    {
                                        unitMove(-1, 1);
                                    }
                                    else if (minusX > 0.5 && minusZ < -0.5)
                                    {
                                        unitMove(1, -1);
                                    }
                                    else if (minusX < -0.5 && minusZ < -0.5)
                                    {
                                        unitMove(-1, -1);
                                    }
                                    else
                                    {
                                        selectUnit = null;
                                        selectOn = false;
                                    }
                                }
                                //룩
                                else
                                {
                                    if (minusX > 0.5 && Mathf.Abs(minusZ) < 0.5)
                                    {
                                        unitMove(1, 0);
                                    }
                                    if (minusX < -0.5 && Mathf.Abs(minusZ) < 0.5)
                                    {
                                        unitMove(-1, 0);
                                    }
                                    if (Mathf.Abs(minusX) < 0.5 && minusZ > 0.5)
                                    {
                                        unitMove(0, 1);
                                    }
                                    if (Mathf.Abs(minusX) < 0.5 && minusZ < -0.5)
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
                            //워프
                            else if (Mathf.Abs(minusX) >= mapMaxX - 1.1 && minusZ == 0 && selectUnit.GetComponent<Units>().warp && selectUnit.GetComponent<Units>().rook)
                            {
                                selectUnit.GetComponent<Units>().WarpMove(hit.collider.transform.position);
                            }
                            else if (Mathf.Abs(minusX) >= mapMaxX - 1.1 && minusZ == 1 && selectUnit.GetComponent<Units>().warp && selectUnit.GetComponent<Units>().bishop)
                            {
                                selectUnit.GetComponent<Units>().WarpMove(hit.collider.transform.position);
                            }
                            else
                            {
                                selectUnit = null;
                                selectOn = false;
                            }
                        }
                        else
                        {}
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

        //이동키, 키패드로 유닛 이동
        if (selectOn && !placementTime && !playerUnits.Any(s => s.GetComponent<Units>().moveSmooth == true) && canMove)
        {
            //레이 쏘아서 바닥 블럭 확인
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.Keypad4))
            {
                if (selectUnit.GetComponent<Units>().warp && selectUnit.transform.position.x <= 1.1)
                {
                    if (Physics.Raycast(new Vector3(mapMaxX, 1, selectUnit.transform.position.z), Vector3.down, out RaycastHit hhh, 3f))
                    {
                        if (hhh.collider.tag == "Tile") selectUnit.GetComponent<Units>().WarpMove(hhh.collider.transform.position);
                    }
                }
                else
                {
                    if (Physics.Raycast(selectUnit.transform.position + new Vector3(-1, 1, 0), Vector3.down, out RaycastHit hhh, 3f))
                    {
                        if (hhh.collider.tag == "Tile") unitMove(-1, 0);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.Keypad6))
            {
                if (selectUnit.GetComponent<Units>().warp && selectUnit.transform.position.x >= mapMaxX-0.1)
                {
                    if (Physics.Raycast(new Vector3(1, 1, selectUnit.transform.position.z), Vector3.down, out RaycastHit hhh, 3f))
                    {
                        if (hhh.collider.tag == "Tile") selectUnit.GetComponent<Units>().WarpMove(hhh.collider.transform.position);
                    }
                }
                else
                {
                    if (Physics.Raycast(selectUnit.transform.position + new Vector3(1, 1, 0), Vector3.down, out RaycastHit hhh, 3f))
                    {
                        if (hhh.collider.tag == "Tile") unitMove(1, 0);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Keypad8))
            {
                if (Physics.Raycast(selectUnit.transform.position + new Vector3(0, 1, 1), Vector3.down, out RaycastHit hhh, 3f))
                {
                    if (hhh.collider.tag == "Tile") unitMove(0, 1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                if (Physics.Raycast(selectUnit.transform.position + new Vector3(0, 1, -1), Vector3.down, out RaycastHit hhh, 3f))
                {
                    if (hhh.collider.tag == "Tile") unitMove(0, -1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                if (selectUnit.GetComponent<Units>().warp && selectUnit.transform.position.x <= 1.1 && selectUnit.GetComponent<Units>().bishop)
                {
                    if (Physics.Raycast(new Vector3(mapMaxX, 1, selectUnit.transform.position.z - 1), Vector3.down, out RaycastHit hhh, 3f))
                    {
                        if (hhh.collider.tag == "Tile") selectUnit.GetComponent<Units>().WarpMove(hhh.collider.transform.position);
                    }
                }
                else
                {
                    if (Physics.Raycast(selectUnit.transform.position + new Vector3(-1, 1, -1), Vector3.down, out RaycastHit hhh, 3f))
                    {
                        if (hhh.collider.tag == "Tile") unitMove(-1, -1);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                if (selectUnit.GetComponent<Units>().warp && selectUnit.transform.position.x >= mapMaxX - 0.1 && selectUnit.GetComponent<Units>().bishop)
                {
                    if (Physics.Raycast(new Vector3(1, 1, selectUnit.transform.position.z - 1), Vector3.down, out RaycastHit hhh, 3f))
                    {
                        if (hhh.collider.tag == "Tile") selectUnit.GetComponent<Units>().WarpMove(hhh.collider.transform.position);
                    }
                }
                else
                {
                    if (Physics.Raycast(selectUnit.transform.position + new Vector3(1, 1, -1), Vector3.down, out RaycastHit hhh, 3f))
                    {
                        if (hhh.collider.tag == "Tile") unitMove(1, -1);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                if (selectUnit.GetComponent<Units>().warp && selectUnit.transform.position.x <= 1.1 && selectUnit.GetComponent<Units>().bishop)
                {
                    if (Physics.Raycast(new Vector3(mapMaxX, 1, selectUnit.transform.position.z + 1), Vector3.down, out RaycastHit hhh, 3f))
                    {
                        if (hhh.collider.tag == "Tile") selectUnit.GetComponent<Units>().WarpMove(hhh.collider.transform.position);
                    }
                }
                else
                {
                    if (Physics.Raycast(selectUnit.transform.position + new Vector3(-1, 1, 1), Vector3.down, out RaycastHit hhh, 3f))
                    {
                        if (hhh.collider.tag == "Tile") unitMove(-1, 1);
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                if (selectUnit.GetComponent<Units>().warp && selectUnit.transform.position.x >= mapMaxX - 0.1 && selectUnit.GetComponent<Units>().bishop)
                {
                    if (Physics.Raycast(new Vector3(1, 1, selectUnit.transform.position.z + 1), Vector3.down, out RaycastHit hhh, 3f))
                    {
                        if (hhh.collider.tag == "Tile") selectUnit.GetComponent<Units>().WarpMove(hhh.collider.transform.position);
                    }
                }
                else
                {
                    if (Physics.Raycast(selectUnit.transform.position + new Vector3(1, 1, 1), Vector3.down, out RaycastHit hhh, 3f))
                    {
                        if (hhh.collider.tag == "Tile") unitMove(1, 1);
                    }
                }
            }
        }

        //턴 시간 제한
        if (playerTurn && !placementTime)
        {
            turnTime += Time.deltaTime;
            if (turnTime >= 120f)
            {
                canMove = true;
                gmrUi.whosTurnTxtChange();
                playerTurn = false;
                turnMove = 0;
                selectOn = false;
                turnTime = 0;
                gmrUi.moveCountTxtChange(turnMaxMove - turnMove);
                enermyUnits.ForEach(unit => unit.GetComponent<Units>().moveAble = true);
                enermyUnits.ForEach(unit => unit.GetComponent<Units>().moveCount = 0);
                AiTurn = true;
                move1EnermyAi = null;
                aiSelect = false;
            }
        }
        else if(!playerTurn && !placementTime)
        {
            turnTime += Time.deltaTime;
            if (turnTime >= 20f)
            {
                canMove = true;
                gmrUi.whosTurnTxtChange();
                playerTurn = true;
                turnMove = 0;
                selectOn = false;
                turnTime = 0;
                gmrUi.moveCountTxtChange(turnMaxMove - turnMove);
                playerUnits.ForEach(unit => unit.GetComponent<Units>().moveAble = true);
                playerUnits.ForEach(unit => unit.GetComponent<Units>().moveCount = 0);
                AiTurn = false;
                move1EnermyAi = null;
                aiSelect = false;
            }
        }

        if (AiTurn)
        {
            aiTime += Time.deltaTime;

            if (aiTime >= 0.5f)
            {
                if (!aiSelect)
                    findEnermyAndPlayer();
                else if (aiSelect)
                {
                    if (move1EnermyAi == null)
                    {
                        aiSelect = false;
                    }
                    else bossAi();
                }
                aiTime = 0;
            }

            if(!enermyUnits.Any(unit => unit.GetComponent<Units>().moveAble))
            {
                canMove = true;
                gmrUi.whosTurnTxtChange();
                turnTime = 0;
                playerTurn = true;
                turnMove = 0;
                selectOn = false;
                gmrUi.moveCountTxtChange(turnMaxMove - turnMove);
                playerUnits.ForEach(unit => unit.GetComponent<Units>().moveAble = true);
                playerUnits.ForEach(unit => unit.GetComponent<Units>().moveCount = 0);
                AiTurn = false;
                move1EnermyAi = null;
                aiSelect = false;
            }
            if (turnMove >= turnMaxMove)
            {
                canMove = true;
                gmrUi.whosTurnTxtChange();
                turnTime = 0;
                playerTurn = true;
                turnMove = 0;
                selectOn = false;
                gmrUi.moveCountTxtChange(turnMaxMove - turnMove);
                playerUnits.ForEach(unit => unit.GetComponent<Units>().moveAble = true);
                playerUnits.ForEach(unit => unit.GetComponent<Units>().moveCount = 0);
                AiTurn = false;
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
        //배치 끝났을 시 버튼 클릭
        if (placementTime)
        {
            if (playerUnits[0].transform.position.y < 1)
            {
                StartCoroutine(placementOverCorou());
                StartCoroutine(startAndDestroy());
            }
        }

        // 턴 종료시 버튼 클릭
        if(playerTurn && !placementTime)
        {
            gmrUi.whosTurnTxtChange();
            turnTime = 0;
            playerTurn = false;
            turnMove = 0;
            selectOn = false;
            gmrUi.moveCountTxtChange(turnMaxMove - turnMove);
            enermyUnits.ForEach(unit => unit.GetComponent<Units>().moveAble = true);
            enermyUnits.ForEach(unit => unit.GetComponent<Units>().moveCount = 0);
            AiTurn = true;
            canMove = true;
            move1EnermyAi = null;
            aiSelect = false;
        }
    }
    
    IEnumerator gmrStartCorou() // 시작 코루틴 / 캐릭터 소환
    {
        libmgr = GameObject.Find("LibMgr").GetComponent<LibMgr>();

        GameObject p1 = Instantiate(playerBass);
        Dictionary<string, object> playerDict = libmgr.unitCode[libmgr.playerUnitsData[0][0]];
        p1.name = playerDict["Name"].ToString();
        p1.transform.position = new Vector3(0, 3, 2);
        p1.GetComponent<Units>().wakeUpUnit();
        p1.GetComponent<Units>().myCodeNum = libmgr.playerUnitsData[0][0];
        p1.GetComponent<Units>().upgradeRank = (int)libmgr.playerUnitsData[0][1];
        p1.GetComponent<Units>().unitEffectTxt = "플레이어";
        p1.GetComponent<Units>().unit2DImage = Resources.Load<Sprite>("Image2D/" + p1.GetComponent<Units>().myCodeNum.ToString());
        playerUnits.Add(p1);
        
        for (int i = 1; i < libmgr.playerUnitsData.Count; i++)
        {
            GameObject fU = Instantiate(friendlyBass);
            Dictionary<string, object> Dict = libmgr.unitCode[libmgr.playerUnitsData[i][0]];
            fU.name = Dict["Name"].ToString();
            fU.transform.position = new Vector3(i, 3, 2);

            Units fUFun = fU.GetComponent<Units>();
            fUFun.myCodeNum = libmgr.playerUnitsData[i][0];
            fUFun.upgradeRank = (int)libmgr.playerUnitsData[i][1];
            fUFun.moveCountUpgrade = Convert.ToSingle(Dict["이동횟수 강화"]);
            fUFun.minNumUpgrade = Convert.ToSingle(Dict["숫자 최소치 강화"]);
            fUFun.maxNumUpgrade = Convert.ToSingle(Dict["숫자최대치 강화"]);
            fUFun.moveMaxCount = (int)Dict["이동 횟수"] + (int)fUFun.moveCountUpgrade *fUFun.upgradeRank;
            fUFun.minNum = (int)Dict["숫자 최소치"] + (int)fUFun.minNumUpgrade *fUFun.upgradeRank;
            fUFun.maxNum = (int)Dict["숫자 최대치"] + (int)fUFun.maxNumUpgrade *fUFun.upgradeRank;
            fUFun.unit2DImage = Resources.Load<Sprite>("Image2D/" + fUFun.myCodeNum);

            Dictionary<string, object> DictEffect = libmgr.unitCodeEffects[libmgr.playerUnitsData[i][0]];
            if (Convert.ToString(DictEffect["대각선만 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = false; }
            if (Convert.ToString(DictEffect["전방위 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = true; }

            if (fUFun.rook && !fUFun.bishop)
                fUFun.unitEffectTxt = "룩";
            else if (!fUFun.rook && fUFun.bishop)
                fUFun.unitEffectTxt = "비숍";
            else if (fUFun.rook && fUFun.bishop)
                fUFun.unitEffectTxt = "퀸";

            if (Convert.ToString(DictEffect["뒤로 이동 불가"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 폰";
                fUFun.pawn = true;
            }
            if (Convert.ToString(DictEffect["홀수 차례만 효과"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 홀수 차례";
                fUFun.oddTurnEffect = true;
            }
            if (Convert.ToString(DictEffect["짝수 차례만 효과"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 짝수 차례";
                fUFun.evenTurnEffect = true;
            }
            if (Convert.ToString(DictEffect["좌우 워프"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 워프";
                fUFun.warp = true;
            }
            if (Convert.ToString(DictEffect["전방향 전투"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 전방향 전투";
                fUFun.attackEvery = true;
            }
            if (Convert.ToString(DictEffect["위치 교체"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 교대";
                fUFun.changePos = true;
            }
            if (Convert.ToString(DictEffect["공격시 상대 숫자 변동"]) != "")
            {
                fUFun.unitEffectTxt += ", 저주";
                fUFun.attackMinusPow = Convert.ToInt32(DictEffect["공격시 상대 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["자신 숫자 변동"]) != "")
            {
                fUFun.unitEffectTxt += ", 축복";
                fUFun.morePower = Convert.ToInt32(DictEffect["자신 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["죽으면 패배"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 심장";
                fUFun.playerHeart = true;
            }
            if (Convert.ToString(DictEffect["위치 고정"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 이동 불가";
                fUFun.cantMove = true;
            }
            if (Convert.ToString(DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"]) != "")
            {
                fUFun.unitEffectTxt += ", 업그레이드";
                fUFun.upgradeCode = DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"];
            }
            if (Convert.ToString(DictEffect["자신 진영에서만 이동 가능"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 士";
                fUFun.onlyMyPlace = true;
            }
            if (Convert.ToString(DictEffect["중립진영에 배치"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 정찰병";
                fUFun.placeAnywhere = true;
            }
            if (Convert.ToString(DictEffect["못 움직임 사망시"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 독";
                fUFun.poision = true;
            }
            if (Convert.ToString(DictEffect["숫자 범위 변하지 않음"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 숫자 변동 없음";
                fUFun.cristalBody = true;
            }
            if (Convert.ToString(DictEffect["차례 종료시 랜덤 이동"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 랜덤 이동";
                fUFun.randomMove = true;
            }
            if (Convert.ToString(DictEffect["다른 아군 숫자 변동"]) != "")
            {
                fUFun.unitEffectTxt += ", 아군 축복";
                fUFun.powerUpTotem = Convert.ToInt32(DictEffect["다른 아군 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["못 움직임 이동 대신"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 빙결";
                fUFun.frozen = true;
            }

            fUFun.wakeUpUnit();
            playerUnits.Add(fU);
        }
        placementTime = true;
        yield return StartCoroutine(stageLoad(libmgr.stageLevelCount));
        yield return StartCoroutine(makeEnermyUnits());
        yield return StartCoroutine(unitplacementCoro());
    }
    
    //배치 종료후 플레이터 턴이 아닌 에너미 턴으로 넘어가는 오류 해결 위해서 코루틴으로 설정
    IEnumerator placementOverCorou()
    {
        gmrUi.whosTurnTxtChange();
        yield return null;
        placementTime = false;
        turnMove = 0;
        turnTime = 0;
        selectOn = false;
        playerTurn = true;
        canMove = true;
        playerUnits.ForEach(pUnits => pUnits.GetComponent<Units>().moveAble = true); // 플레이어 유닛 전원 무브 가능 상태로 람다식
        yield return new WaitForSecondsRealtime(0.5f);
    }

    //배치 종료후 배치 안한 유닛 파괴
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

    //맵 제작 
    public IEnumerator stageLoad(int sLevel)
    {
        Dictionary<string, object> bossMapDict = libmgr.bossCodeStage[libmgr.bossToMeet[sLevel][0]];
        mapMaxX = (int)bossMapDict["MapMaxX"];
        mapMaxZ = (int)bossMapDict["MapMaxZ"];
        friendlyZone = (int)bossMapDict["FriendlyZone"];
        yield return null;
        //적 맵 제작
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
                    zTile.GetComponent<Renderer>().material.mainTexture = Resources.Load("Textures/playerTile_" + bossMapDict["ID"]) as Texture;
                }
                else if (j > mapMaxZ - friendlyZone)
                {
                    GameObject zTile = Instantiate(enermyTiles, tileMap.transform);
                    zTile.transform.position = new Vector3(i, -0.5f, j);
                    zTile.GetComponent<Renderer>().material.mainTexture = Resources.Load("Textures/enermyTile_" + bossMapDict["ID"]) as Texture;
                }
                else
                {
                    GameObject zTile = Instantiate(normalTiles, tileMap.transform);
                    zTile.transform.position = new Vector3(i, -0.5f, j);
                    zTile.GetComponent<Renderer>().material.mainTexture = Resources.Load("Textures/normalTile_" + bossMapDict["ID"]) as Texture;
                }
            }
        }
    }
    //적 유닛 제작
    //보스 생성
    public IEnumerator makeEnermyUnits()
    {
        GameObject BossfU = Instantiate(bossBass);
        Dictionary<string, object> DictBoss = libmgr.unitCode[libmgr.bossToMeet[libmgr.stageLevelCount][0]];
        BossfU.name = DictBoss["Name"].ToString();
        BossfU.GetComponent<Units>().myCodeNum = libmgr.bossToMeet[libmgr.stageLevelCount][0];
        BossfU.GetComponent<Units>().wakeUpUnit();
        BossfU.transform.position = new Vector3(mapMaxX+3, 3, mapMaxZ + 1);
        BossfU.GetComponent<Units>().unitEffectTxt = "보스";
        BossfU.GetComponent<Units>().unit2DImage = Resources.Load<Sprite>("Image2D/" + BossfU.GetComponent<Units>().myCodeNum);
        enermyUnits.Add(BossfU);

        yield return null;

        //적 기본 유닛
        for (int i = 1; i < libmgr.stageLevelCount + 2; i++)
        {
            GameObject fU = Instantiate(enermyBass);
            Dictionary<string, object> DictFirst = libmgr.unitCode[libmgr.bossToMeet[libmgr.stageLevelCount][0]];
            Dictionary<string, object> Dict = libmgr.unitCode[DictFirst["기본"]];
            fU.name = Dict["Name"].ToString();
            fU.transform.position = new Vector3(mapMaxX-i+3, 3, mapMaxZ + 1);

            Units fUFun = fU.GetComponent<Units>();
            fUFun.myCodeNum = DictFirst["기본"];
            fUFun.upgradeRank = libmgr.stageLevelCount;
            fUFun.moveCountUpgrade = Convert.ToSingle(Dict["이동횟수 강화"]);
            fUFun.minNumUpgrade = Convert.ToSingle(Dict["숫자 최소치 강화"]);
            fUFun.maxNumUpgrade = Convert.ToSingle(Dict["숫자최대치 강화"]);
            fUFun.moveMaxCount = (int)Dict["이동 횟수"] + (int)fUFun.moveCountUpgrade * fUFun.upgradeRank;
            fUFun.minNum = (int)Dict["숫자 최소치"] + (int)fUFun.minNumUpgrade * fUFun.upgradeRank;
            fUFun.maxNum = (int)Dict["숫자 최대치"] + (int)fUFun.maxNumUpgrade * fUFun.upgradeRank;
            fUFun.unit2DImage = Resources.Load<Sprite>("Image2D/" + fUFun.myCodeNum);

            Dictionary<string, object> DictEffect = libmgr.unitCodeEffects[DictFirst["기본"]];
            if (Convert.ToString(DictEffect["대각선만 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = false; }
            if (Convert.ToString(DictEffect["전방위 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = true; }

            if (fUFun.rook && !fUFun.bishop)
                fUFun.unitEffectTxt = "룩";
            else if (!fUFun.rook && fUFun.bishop)
                fUFun.unitEffectTxt = "비숍";
            else if (fUFun.rook && fUFun.bishop)
                fUFun.unitEffectTxt = "퀸";

            if (Convert.ToString(DictEffect["뒤로 이동 불가"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 폰";
                fUFun.pawn = true;
            }
            if (Convert.ToString(DictEffect["홀수 차례만 효과"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 홀수 차례";
                fUFun.oddTurnEffect = true;
            }
            if (Convert.ToString(DictEffect["짝수 차례만 효과"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 짝수 차례";
                fUFun.evenTurnEffect = true;
            }
            if (Convert.ToString(DictEffect["좌우 워프"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 워프";
                fUFun.warp = true;
            }
            if (Convert.ToString(DictEffect["전방향 전투"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 전방향 전투";
                fUFun.attackEvery = true;
            }
            if (Convert.ToString(DictEffect["위치 교체"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 교대";
                fUFun.changePos = true;
            }
            if (Convert.ToString(DictEffect["공격시 상대 숫자 변동"]) != "")
            {
                fUFun.unitEffectTxt += ", 저주";
                fUFun.attackMinusPow = Convert.ToInt32(DictEffect["공격시 상대 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["자신 숫자 변동"]) != "")
            {
                fUFun.unitEffectTxt += ", 축복";
                fUFun.morePower = Convert.ToInt32(DictEffect["자신 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["죽으면 패배"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 심장";
                fUFun.playerHeart = true;
            }
            if (Convert.ToString(DictEffect["위치 고정"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 이동 불가";
                fUFun.cantMove = true;
            }
            if (Convert.ToString(DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"]) != "")
            {
                fUFun.unitEffectTxt += ", 업그레이드";
                fUFun.upgradeCode = DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"];
            }
            if (Convert.ToString(DictEffect["자신 진영에서만 이동 가능"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 士";
                fUFun.onlyMyPlace = true;
            }
            if (Convert.ToString(DictEffect["중립진영에 배치"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 정찰병";
                fUFun.placeAnywhere = true;
            }
            if (Convert.ToString(DictEffect["못 움직임 사망시"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 독";
                fUFun.poision = true;
            }
            if (Convert.ToString(DictEffect["숫자 범위 변하지 않음"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 숫자 변동 없음";
                fUFun.cristalBody = true;
            }
            if (Convert.ToString(DictEffect["차례 종료시 랜덤 이동"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 랜덤 이동";
                fUFun.randomMove = true;
            }
            if (Convert.ToString(DictEffect["다른 아군 숫자 변동"]) != "")
            {
                fUFun.unitEffectTxt += ", 아군 축복";
                fUFun.powerUpTotem = Convert.ToInt32(DictEffect["다른 아군 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["못 움직임 이동 대신"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 빙결";
                fUFun.frozen = true;
            }

            fUFun.wakeUpUnit();
            enermyUnits.Add(fU);
        }
        // 적 고유 유닛 3렙
        if (libmgr.stageLevelCount >= 3)
        {
            GameObject fU = Instantiate(enermyBass);
            Dictionary<string, object> UniqueDictFirst = libmgr.unitCode[libmgr.bossToMeet[libmgr.stageLevelCount][0]];
            Dictionary<string, object> UniqueDict = libmgr.unitCode[UniqueDictFirst["고유"]];
            fU.name = UniqueDict["Name"].ToString();
            fU.transform.position = new Vector3(mapMaxX - enermyUnits.Count+3, 3, mapMaxZ + 1);

            Units fUFun = fU.GetComponent<Units>();
            fUFun.myCodeNum = UniqueDictFirst["고유"];
            fUFun.upgradeRank = libmgr.stageLevelCount;
            fUFun.moveCountUpgrade = Convert.ToSingle(UniqueDict["이동횟수 강화"]);
            fUFun.minNumUpgrade = Convert.ToSingle(UniqueDict["숫자 최소치 강화"]);
            fUFun.maxNumUpgrade = Convert.ToSingle(UniqueDict["숫자최대치 강화"]);
            fUFun.moveMaxCount = (int)UniqueDict["이동 횟수"] + (int)fUFun.moveCountUpgrade * fUFun.upgradeRank;
            fUFun.minNum = (int)UniqueDict["숫자 최소치"] + (int)fUFun.minNumUpgrade * fUFun.upgradeRank;
            fUFun.maxNum = (int)UniqueDict["숫자 최대치"] + (int)fUFun.maxNumUpgrade * fUFun.upgradeRank;
            fUFun.unit2DImage = Resources.Load<Sprite>("Image2D/" + fUFun.myCodeNum);

            Dictionary<string, object> DictEffect = libmgr.unitCodeEffects[UniqueDictFirst["고유"]];
            if (Convert.ToString(DictEffect["대각선만 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = false; }
            if (Convert.ToString(DictEffect["전방위 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = true; }

            if (fUFun.rook && !fUFun.bishop)
                fUFun.unitEffectTxt = "룩";
            else if (!fUFun.rook && fUFun.bishop)
                fUFun.unitEffectTxt = "비숍";
            else if (fUFun.rook && fUFun.bishop)
                fUFun.unitEffectTxt = "퀸";

            if (Convert.ToString(DictEffect["뒤로 이동 불가"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 폰";
                fUFun.pawn = true;
            }
            if (Convert.ToString(DictEffect["홀수 차례만 효과"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 홀수 차례";
                fUFun.oddTurnEffect = true;
            }
            if (Convert.ToString(DictEffect["짝수 차례만 효과"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 짝수 차례";
                fUFun.evenTurnEffect = true;
            }
            if (Convert.ToString(DictEffect["좌우 워프"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 워프";
                fUFun.warp = true;
            }
            if (Convert.ToString(DictEffect["전방향 전투"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 전방향 전투";
                fUFun.attackEvery = true;
            }
            if (Convert.ToString(DictEffect["위치 교체"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 교대";
                fUFun.changePos = true;
            }
            if (Convert.ToString(DictEffect["공격시 상대 숫자 변동"]) != "")
            {
                fUFun.unitEffectTxt += ", 저주";
                fUFun.attackMinusPow = Convert.ToInt32(DictEffect["공격시 상대 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["자신 숫자 변동"]) != "")
            {
                fUFun.unitEffectTxt += ", 축복";
                fUFun.morePower = Convert.ToInt32(DictEffect["자신 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["죽으면 패배"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 심장";
                fUFun.playerHeart = true;
            }
            if (Convert.ToString(DictEffect["위치 고정"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 이동 불가";
                fUFun.cantMove = true;
            }
            if (Convert.ToString(DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"]) != "")
            {
                fUFun.unitEffectTxt += ", 업그레이드";
                fUFun.upgradeCode = DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"];
            }
            if (Convert.ToString(DictEffect["자신 진영에서만 이동 가능"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 士";
                fUFun.onlyMyPlace = true;
            }
            if (Convert.ToString(DictEffect["중립진영에 배치"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 정찰병";
                fUFun.placeAnywhere = true;
            }
            if (Convert.ToString(DictEffect["못 움직임 사망시"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 독";
                fUFun.poision = true;
            }
            if (Convert.ToString(DictEffect["숫자 범위 변하지 않음"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 숫자 변동 없음";
                fUFun.cristalBody = true;
            }
            if (Convert.ToString(DictEffect["차례 종료시 랜덤 이동"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 랜덤 이동";
                fUFun.randomMove = true;
            }
            if (Convert.ToString(DictEffect["다른 아군 숫자 변동"]) != "")
            {
                fUFun.unitEffectTxt += ", 아군 축복";
                fUFun.powerUpTotem = Convert.ToInt32(DictEffect["다른 아군 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["못 움직임 이동 대신"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 빙결";
                fUFun.frozen = true;
            }

            fUFun.wakeUpUnit();
            enermyUnits.Add(fU);
        }
        //적 고유 유닛 6렙
        if (libmgr.stageLevelCount >= 6)
        {
            GameObject fU = Instantiate(enermyBass);
            Dictionary<string, object> UniqueDictFirst = libmgr.unitCode[libmgr.bossToMeet[libmgr.stageLevelCount][0]];
            Dictionary<string, object> UniqueDict = libmgr.unitCode[UniqueDictFirst["고유"]];
            fU.name = UniqueDict["Name"].ToString();
            fU.transform.position = new Vector3(mapMaxX - enermyUnits.Count + 3, 3, mapMaxZ + 1);

            Units fUFun = fU.GetComponent<Units>();
            fUFun.myCodeNum = UniqueDictFirst["고유"];
            fUFun.upgradeRank = libmgr.stageLevelCount;
            fUFun.moveCountUpgrade = Convert.ToSingle(UniqueDict["이동횟수 강화"]);
            fUFun.minNumUpgrade = Convert.ToSingle(UniqueDict["숫자 최소치 강화"]);
            fUFun.maxNumUpgrade = Convert.ToSingle(UniqueDict["숫자최대치 강화"]);
            fUFun.moveMaxCount = (int)UniqueDict["이동 횟수"] + (int)fUFun.moveCountUpgrade * fUFun.upgradeRank;
            fUFun.minNum = (int)UniqueDict["숫자 최소치"] + (int)fUFun.minNumUpgrade * fUFun.upgradeRank;
            fUFun.maxNum = (int)UniqueDict["숫자 최대치"] + (int)fUFun.maxNumUpgrade * fUFun.upgradeRank;
            fUFun.unit2DImage = Resources.Load<Sprite>("Image2D/" + fUFun.myCodeNum);

            Dictionary<string, object> DictEffect = libmgr.unitCodeEffects[UniqueDictFirst["고유"]];
            if (Convert.ToString(DictEffect["대각선만 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = false; }
            if (Convert.ToString(DictEffect["전방위 이동"]) == "TRUE") { fUFun.bishop = true; fUFun.rook = true; }

            if (fUFun.rook && !fUFun.bishop)
                fUFun.unitEffectTxt = "룩";
            else if (!fUFun.rook && fUFun.bishop)
                fUFun.unitEffectTxt = "비숍";
            else if (fUFun.rook && fUFun.bishop)
                fUFun.unitEffectTxt = "퀸";

            if (Convert.ToString(DictEffect["뒤로 이동 불가"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 폰";
                fUFun.pawn = true;
            }
            if (Convert.ToString(DictEffect["홀수 차례만 효과"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 홀수 차례";
                fUFun.oddTurnEffect = true;
            }
            if (Convert.ToString(DictEffect["짝수 차례만 효과"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 짝수 차례";
                fUFun.evenTurnEffect = true;
            }
            if (Convert.ToString(DictEffect["좌우 워프"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 워프";
                fUFun.warp = true;
            }
            if (Convert.ToString(DictEffect["전방향 전투"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 전방향 전투";
                fUFun.attackEvery = true;
            }
            if (Convert.ToString(DictEffect["위치 교체"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 교대";
                fUFun.changePos = true;
            }
            if (Convert.ToString(DictEffect["공격시 상대 숫자 변동"]) != "")
            {
                fUFun.unitEffectTxt += ", 저주";
                fUFun.attackMinusPow = Convert.ToInt32(DictEffect["공격시 상대 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["자신 숫자 변동"]) != "")
            {
                fUFun.unitEffectTxt += ", 축복";
                fUFun.morePower = Convert.ToInt32(DictEffect["자신 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["죽으면 패배"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 심장";
                fUFun.playerHeart = true;
            }
            if (Convert.ToString(DictEffect["위치 고정"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 이동 불가";
                fUFun.cantMove = true;
            }
            if (Convert.ToString(DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"]) != "")
            {
                fUFun.unitEffectTxt += ", 업그레이드";
                fUFun.upgradeCode = DictEffect["상대 진영 끝에 도달하면 강화(뒤집기)"];
            }
            if (Convert.ToString(DictEffect["자신 진영에서만 이동 가능"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 士";
                fUFun.onlyMyPlace = true;
            }
            if (Convert.ToString(DictEffect["중립진영에 배치"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 정찰병";
                fUFun.placeAnywhere = true;
            }
            if (Convert.ToString(DictEffect["못 움직임 사망시"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 독";
                fUFun.poision = true;
            }
            if (Convert.ToString(DictEffect["숫자 범위 변하지 않음"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 숫자 변동 없음";
                fUFun.cristalBody = true;
            }
            if (Convert.ToString(DictEffect["차례 종료시 랜덤 이동"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 랜덤 이동";
                fUFun.randomMove = true;
            }
            if (Convert.ToString(DictEffect["다른 아군 숫자 변동"]) != "")
            {
                fUFun.unitEffectTxt += ", 아군 축복";
                fUFun.powerUpTotem = Convert.ToInt32(DictEffect["다른 아군 숫자 변동"]);
            }
            if (Convert.ToString(DictEffect["못 움직임 이동 대신"]) == "TRUE")
            {
                fUFun.unitEffectTxt += ", 빙결";
                fUFun.frozen = true;
            }

            fUFun.wakeUpUnit();
            enermyUnits.Add(fU);
        }
        mainCam.transform.position = new Vector3(mapMaxX / 2 + 0.5f, mapMaxZ + 2, mapMaxZ / 2);
    }

    //보스 유닛 배치
    public IEnumerator unitplacementCoro()
    {
        yield return new WaitForSeconds(0.05f);
        rndbossplace = UnityEngine.Random.Range(1, mapMaxX + 1);
        enermyUnits[0].GetComponent<Units>().placeMove(new Vector3(rndbossplace, enermyUnits[0].transform.position.y, mapMaxZ));
    }
    //다른 유닛 마저 배치
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
                int rnd = UnityEngine.Random.Range(1, mapMaxX + 1);
                int rnd2 = UnityEngine.Random.Range(0, friendlyZone);
                Vector3 upUnitPlace = new Vector3(rnd, 10, mapMaxZ - rnd2);
                if (Physics.Raycast(upUnitPlace, Vector3.down, out RaycastHit hit, 15f))
                {
                    if (hit.collider.tag == "Tile")
                    {
                        enermyUnits[enermyPlaceCount].GetComponent<Units>().placeMove(new Vector3(rnd, enermyUnits[enermyPlaceCount].transform.position.y, mapMaxZ - rnd2));
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
        }
    }

    //전투
    public IEnumerator fighting(GameObject a, GameObject b)
    {
        if (a == playerUnits[0] || b == enermyUnits[0])
        {
            if (a == playerUnits[0])
            {
                lose = true;
                losePanel.SetActive(true);
                loseplayerUnitFight2D.sprite = a.GetComponent<Units>().unit2DImage;
                loseenermyUnitFight2D.sprite = b.GetComponent<Units>().unit2DImage;
            }
            else
            {
                if (libmgr.stageLevelCount == 6)
                {
                    win = true;
                    Application.Quit();
                }
                else
                {
                    win = true;
                    libmgr.money += 100 * (libmgr.stageLevelCount+1);
                    winPanel.SetActive(true);
                    winplayerUnitFight2D.sprite = a.GetComponent<Units>().unit2DImage;
                    winenermyUnitFight2D.sprite = b.GetComponent<Units>().unit2DImage;
                    moneyTxt.text = "+ " + 100 * (libmgr.stageLevelCount + 1) + " <color=yellow>G</color>";
                }
            }
        }
        else
        {
            FightPanel.SetActive(true);
            playerdiceTxt.text = "?";
            enermydiceTxt.text = "?";
            playerUnitFight2D.sprite = a.GetComponent<Units>().unit2DImage;
            enermyUnitFight2D.sprite = b.GetComponent<Units>().unit2DImage;
            yield return new WaitForSecondsRealtime(1f);
            int aRnd = UnityEngine.Random.Range(a.GetComponent<Units>().minNum, a.GetComponent<Units>().maxNum + 1);
            playerdiceTxt.text = aRnd.ToString();
            int bRnd = UnityEngine.Random.Range(b.GetComponent<Units>().minNum, b.GetComponent<Units>().maxNum + 1);
            enermydiceTxt.text = bRnd.ToString();
            yield return new WaitForSecondsRealtime(2f);
            //플레이어 유닛 승리
            if (aRnd > bRnd)
            {
                enermyUnits.Remove(b);
                a.GetComponent<Units>().maxNum -= bRnd;
                a.GetComponent<Units>().minNum -= bRnd;
                if (a.GetComponent<Units>().minNum < 0)
                { a.GetComponent<Units>().minNum = 0; }
                a.GetComponent<Units>().halfNum = (a.GetComponent<Units>().maxNum + a.GetComponent<Units>().minNum) / 2;
                if (b.GetComponent<Units>().playerHeart)
                {
                    if (libmgr.stageLevelCount == 6)
                    {
                        win = true;
                        Application.Quit();
                    }
                    else
                    {
                        win = true;
                        libmgr.money += 100 * (libmgr.stageLevelCount + 1);
                        winPanel.SetActive(true);
                        winplayerUnitFight2D.sprite = a.GetComponent<Units>().unit2DImage;
                        winenermyUnitFight2D.sprite = b.GetComponent<Units>().unit2DImage;
                        moneyTxt.text = "+ " + 100 * (libmgr.stageLevelCount + 1) + " <color=yellow>G</color>";
                    }
                }
                Destroy(b);
            }
            //에너미 유닛 승리
            else if (bRnd > aRnd)
            {
                if (a.GetComponent<Units>().moveAble) turnMove++;

                playerUnits.Remove(a);
                var targetUnitData = libmgr.playerUnitsData.Find(unitData => unitData[0] == a.GetComponent<Units>().myCodeNum &&
                Convert.ToInt32(unitData[1]) == a.GetComponent<Units>().upgradeRank);
                if (targetUnitData != null)
                {
                    libmgr.playerUnitsData.Remove(targetUnitData);
                }
                b.GetComponent<Units>().maxNum -= aRnd;
                b.GetComponent<Units>().minNum -= aRnd;
                if (b.GetComponent<Units>().minNum < 0)
                { b.GetComponent<Units>().minNum = 0; }
                b.GetComponent<Units>().halfNum = (b.GetComponent<Units>().maxNum + b.GetComponent<Units>().minNum) / 2;
                if (a.GetComponent<Units>().playerHeart)
                {
                    lose = true;
                    losePanel.SetActive(true);
                    loseplayerUnitFight2D.sprite = a.GetComponent<Units>().unit2DImage;
                    loseenermyUnitFight2D.sprite = b.GetComponent<Units>().unit2DImage;
                }
                Destroy(a);
            }
            //무승부 양자 파괴
            else
            {              
                var targetUnitData = libmgr.playerUnitsData.Find(unitData => unitData[0] == a.GetComponent<Units>().myCodeNum &&
                Convert.ToInt32(unitData[1]) == a.GetComponent<Units>().upgradeRank);
                if (targetUnitData != null)
                {
                    libmgr.playerUnitsData.Remove(targetUnitData);
                }
                playerUnits.Remove(a);
                enermyUnits.Remove(b);
                if (a.GetComponent<Units>().playerHeart && !b.GetComponent<Units>().playerHeart)
                {
                    lose = true;
                    losePanel.SetActive(true);
                    loseplayerUnitFight2D.sprite = a.GetComponent<Units>().unit2DImage;
                    loseenermyUnitFight2D.sprite = b.GetComponent<Units>().unit2DImage;
                }
                else if (!a.GetComponent<Units>().playerHeart && b.GetComponent<Units>().playerHeart)
                {
                    if (libmgr.stageLevelCount == 6)
                    {
                        win = true;
                        Application.Quit();
                    }
                    else
                    {
                        win = true;
                        libmgr.money += 100 * (libmgr.stageLevelCount + 1);
                        winPanel.SetActive(true);
                        winplayerUnitFight2D.sprite = a.GetComponent<Units>().unit2DImage;
                        winenermyUnitFight2D.sprite = b.GetComponent<Units>().unit2DImage;
                        moneyTxt.text = "+ " + 100 * (libmgr.stageLevelCount + 1) + " <color=yellow>G</color>";
                    }
                }
                Destroy(a);
                Destroy(b);
            }
            FightPanel.SetActive(false);
        }
    }

    void bossAi()
    {
        float AiDistX = move1EnermyAi.transform.position.x - targetPlayerUnit1Ai.transform.position.x;
        float AiDistZ = move1EnermyAi.transform.position.z - targetPlayerUnit1Ai.transform.position.z;
        //접근
        if (move1EnermyAi.GetComponent<Units>().halfNum + bosstype >= targetPlayerUnit1Ai.GetComponent<Units>().halfNum && move1EnermyAi != enermyUnits[0])
        {
            if (move1EnermyAi.GetComponent<Units>().pawn)
            {
                //퀸
                if (move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                {
                    if (AiDistX > 0.1 && AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else
                            move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                    }
                    else if (AiDistX > 0.1 && AiDistZ < -0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else
                            move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else if (AiDistX < -0.1 && AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                    }
                    else if (AiDistX < -0.1 && AiDistZ < -0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                    }
                    else if (Mathf.Abs(AiDistX) <= 0.1 && AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                    }
                    else if (AiDistX > 0.1 && Mathf.Abs(AiDistZ) <= 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else if (AiDistX < -0.1 && Mathf.Abs(AiDistZ) <= 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                    }
                    else move1EnermyAi.GetComponent<Units>().moveAble = false;
                }
                //비숍
                else if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                {
                    if (AiDistX > 0.1 && AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                    }
                    else if (AiDistX > 0.1 && AiDistZ < -0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else if (AiDistX < -0.1 && AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                    }
                    else if (AiDistX < -0.1 && AiDistZ < -0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                    }
                    else if (Mathf.Abs(AiDistX) <= 0.1 && AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                    }
                    else if (AiDistX > 0.1 && Mathf.Abs(AiDistZ) <= 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else if (AiDistX < -0.1 && Mathf.Abs(AiDistZ) <= 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                    }
                    else move1EnermyAi.GetComponent<Units>().moveAble = false;
                }
                //룩
                else if (move1EnermyAi.GetComponent<Units>().rook && !move1EnermyAi.GetComponent<Units>().bishop)
                {
                    if (AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                    }
                    else if (AiDistX > 0.1 && AiDistZ < 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else if (AiDistX < -0.1 && AiDistZ < 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                    }
                    else if (AiDistX > 0.1 && AiDistZ > -0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else move1EnermyAi.GetComponent<Units>().moveAble = false;
                }
            }
            else
            {
                //퀸
                if (move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                {
                    if (AiDistX > 0.1 && AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                    }
                    else if (AiDistX > 0.1 && AiDistZ < -0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else if (AiDistX < -0.1 && AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                    }
                    else if (AiDistX < -0.1 && AiDistZ < -0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                    }
                    else if (Mathf.Abs(AiDistX) <= 0.1 && AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                    }
                    else if (AiDistX > 0.1 && Mathf.Abs(AiDistZ) <= 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else if (AiDistX < -0.1 && Mathf.Abs(AiDistZ) <= 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                    }
                    else move1EnermyAi.GetComponent<Units>().moveAble = false;
                }
                //비숍
                else if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                {
                    if (AiDistX > 0.1 && AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                    }
                    else if (AiDistX > 0.1 && AiDistZ < -0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else if (AiDistX < -0.1 && AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                    }
                    else if (AiDistX < -0.1 && AiDistZ < -0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                    }
                    else if (Mathf.Abs(AiDistX) <= 0.1 && AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                    }
                    else if (AiDistX > 0.1 && Mathf.Abs(AiDistZ) <= 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else if (AiDistX < -0.1 && Mathf.Abs(AiDistZ) <= 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                    }
                    else move1EnermyAi.GetComponent<Units>().moveAble = false;
                }
                //룩
                else if (move1EnermyAi.GetComponent<Units>().rook && !move1EnermyAi.GetComponent<Units>().bishop)
                {
                    if (AiDistZ > 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                    }
                    else if (AiDistX > 0.1 && AiDistZ < 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else if (AiDistX < 0.1 && AiDistX > -0.1 && AiDistZ < 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, 1), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, 1));
                    }
                    else if (AiDistX < -0.1 && AiDistZ < 0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                    }
                    else if (AiDistX > 0.1 && AiDistZ > -0.1)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else move1EnermyAi.GetComponent<Units>().moveAble = false;
                }
            }
        }
        //회피시 거리가 멀어서 자율 행동
        else if (move1EnermyAi.GetComponent<Units>().halfNum + bosstype < targetPlayerUnit1Ai.GetComponent<Units>().halfNum
            && Mathf.Abs(move1EnermyAi.GetComponent<Units>().transform.position.z - targetPlayerUnit1Ai.GetComponent<Units>().transform.position.z) > 3 && move1EnermyAi != enermyUnits[0])
        {
            if (move1EnermyAi.transform.position.x == 1)
            {
                if (move1EnermyAi.GetComponent<Units>().pawn)
                {
                    //퀸
                    if (move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (playerUnits[0].transform.position.x > move1EnermyAi.transform.position.x)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                                move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                        }
                        else
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                        }
                    }
                    //비숍
                    if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (playerUnits[0].transform.position.x > move1EnermyAi.transform.position.x)
                        {
                            if (playerUnits[0].transform.position.z < move1EnermyAi.transform.position.z)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                            }
                            else
                            {
                                 move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                        }
                        else
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                            }
                        }
                    }
                    //룩
                    else
                    {
                        if (playerUnits[0].transform.position.x > move1EnermyAi.transform.position.x)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                                move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                        }
                        else
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                        }
                    }
                }
                else 
                {
                    //퀸
                    if (move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (playerUnits[0].transform.position.x > move1EnermyAi.transform.position.x)
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 1));
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                            }
                        }
                        else
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 1));
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                        }
                    }
                    //비숍
                    if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (playerUnits[0].transform.position.x > move1EnermyAi.transform.position.x)
                        {
                            if (playerUnits[0].transform.position.z < move1EnermyAi.transform.position.z)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 1));
                            }
                        }
                        else
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 1));
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                            }
                        }
                    }
                    //룩
                    else
                    {
                        if (playerUnits[0].transform.position.x > move1EnermyAi.transform.position.x)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                                move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                        }
                        else
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                        }
                    }
                }
            }
            else if (move1EnermyAi.transform.position.x == mapMaxX)
            {
                if (move1EnermyAi.GetComponent<Units>().pawn)
                {
                    //퀸
                    if (move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (playerUnits[0].transform.position.x < move1EnermyAi.transform.position.x)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                                move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                        }
                        else
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                        }
                    }
                    //비숍
                    if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (playerUnits[0].transform.position.x < move1EnermyAi.transform.position.x)
                        {
                            if (playerUnits[0].transform.position.z < move1EnermyAi.transform.position.z)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                            }
                            else
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                        }
                        else
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                            }
                        }
                    }
                    //룩
                    else
                    {
                        if (playerUnits[0].transform.position.x < move1EnermyAi.transform.position.x)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                                move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                        }
                        else
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                        }
                    }
                }
                else
                {
                    //퀸
                    if (move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (playerUnits[0].transform.position.x < move1EnermyAi.transform.position.x)
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 1));
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                            }
                        }
                        else
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 1));
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                        }
                    }
                    //비숍
                    if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (playerUnits[0].transform.position.x < move1EnermyAi.transform.position.x)
                        {
                            if (playerUnits[0].transform.position.z < move1EnermyAi.transform.position.z)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 1));
                            }
                        }
                        else
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 1));
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                            }
                        }
                    }
                    //룩
                    else
                    {
                        if (playerUnits[0].transform.position.x < move1EnermyAi.transform.position.x)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                                move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                        }
                        else
                        {
                            if (playerUnits[0].transform.position.z > move1EnermyAi.transform.position.z)
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                        }
                    }
                }
            }
            else
            {
                if (move1EnermyAi.transform.position.z < mapMaxZ && move1EnermyAi.transform.position.z != 1)
                {
                    if (playerUnits[0].transform.position.z < move1EnermyAi.transform.position.z)
                    {
                        //퀸
                        if (move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                        {
                            if (playerUnits[0].transform.position.x < move1EnermyAi.transform.position.x)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                            }
                            else if (playerUnits[0].transform.position.x > move1EnermyAi.transform.position.x)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                        }
                        //비숍
                        if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                        {
                            if (playerUnits[0].transform.position.x < move1EnermyAi.transform.position.x)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                            }
                        }
                        //룩
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                                move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                        }
                    }
                    else
                    { 
                        //퀸
                        if (move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                        {
                            if (move1EnermyAi.GetComponent<Units>().pawn)
                            {
                                if (playerUnits[0].transform.position.x < move1EnermyAi.transform.position.x)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else
                                        move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                                }
                                else if (playerUnits[0].transform.position.x > move1EnermyAi.transform.position.x)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else
                                        move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                                }
                                else
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                            }
                            else
                            {
                                if (playerUnits[0].transform.position.x < move1EnermyAi.transform.position.x)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else
                                        move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                                }
                                else if (playerUnits[0].transform.position.x > move1EnermyAi.transform.position.x)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else
                                        move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                                }
                                else
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, 1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else
                                        move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, 1));
                                }
                            }
                        }
                        //비숍
                        if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                        {
                            if (move1EnermyAi.GetComponent<Units>().pawn)
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            else
                            {
                                if (playerUnits[0].transform.position.x < move1EnermyAi.transform.position.x)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else
                                        move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 1));
                                }
                                else
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else
                                        move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 1));
                                }
                            }
                        }
                        //룩
                        {
                            if (playerUnits[0].transform.position.x < move1EnermyAi.transform.position.x)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                            }
                            else if (playerUnits[0].transform.position.x > move1EnermyAi.transform.position.x)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                            }
                            else 
                            {
                                if (move1EnermyAi.GetComponent<Units>().pawn)
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                else
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, 1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else
                                        move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, 1));
                                }
                            }
                        }
                    }
                }
                else if (move1EnermyAi.transform.position.z == 1)
                {
                    if (move1EnermyAi.transform.position.x > mapMaxX / 2)
                    {   //비숍
                        if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                        {
                            if (move1EnermyAi.GetComponent<Units>().pawn)
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                            }
                        }
                        //룩
                        else if (move1EnermyAi.GetComponent<Units>().rook)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                                move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                        }
                    }
                    else
                    {
                        //비숍
                        if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                        {
                            if (move1EnermyAi.GetComponent<Units>().pawn)
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else
                                    move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                            }
                        }
                        //룩
                        else if (move1EnermyAi.GetComponent<Units>().rook)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                        }
                    }
                }
                else
                {
                    if (move1EnermyAi.transform.position.x > mapMaxX / 2)
                    {   //비숍
                        if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                        }
                        //룩
                        else if (move1EnermyAi.GetComponent<Units>().rook)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else
                                move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                        }
                    }
                    else
                    {
                        //비숍
                        if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                        }
                        //룩
                        else if (move1EnermyAi.GetComponent<Units>().rook)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                        }
                    }
                }
            }
        }
        //회피
        else
        {
            //맵 맨 위 아닌 경우
            if (move1EnermyAi.transform.position.z < mapMaxZ)
            {
                if (move1EnermyAi.GetComponent<Units>().pawn)
                {
                    //퀸
                    if (move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (move1EnermyAi.transform.position.x == 1)
                        {
                            if (AiDistZ < 0.1 && move1EnermyAi.transform.position.z != 1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                            else move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else if (move1EnermyAi.transform.position.x == mapMaxX)
                        {
                            if (AiDistZ < 0.1 && move1EnermyAi.transform.position.z != 1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                            else move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else
                        {
                            if (AiDistX > 0.1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                            }
                            else if (AiDistX < -0.1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                            }
                            else
                            {
                                int rnd = UnityEngine.Random.Range(0, 2);
                                if (rnd == 0)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                                }
                                else if (rnd == 1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                                }
                            }
                        }
                    }
                    //비숍
                    else if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (move1EnermyAi.transform.position.x == 1)
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else if (move1EnermyAi.transform.position.x == mapMaxX)
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else
                        {
                            if (move1EnermyAi.transform.position.z == 1)
                            {
                                if (AiDistX > 0.1 && AiDistZ < 0.1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                                }
                                else if (AiDistX < -0.1 && AiDistZ < 0.1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                                }
                                else move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                    }
                    //룩
                    else if (move1EnermyAi.GetComponent<Units>().rook && !move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (move1EnermyAi.transform.position.x == 1)
                        {
                            if (AiDistZ < 0.1 && move1EnermyAi.transform.position.z != 1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                            else move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else if (move1EnermyAi.transform.position.x == mapMaxX)
                        {
                            if (AiDistZ < 0.1 && move1EnermyAi.transform.position.z != 1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                            }
                            else move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else
                        {
                            if (AiDistX > 0.1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                            }
                            else if (AiDistX < -0.1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                            }
                            else
                            {
                                int rnd = UnityEngine.Random.Range(0, 2);
                                if (rnd == 0)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                                }
                                else
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                                }
                            }
                        }
                    }
                }
                else
                {
                    //퀸
                    if (move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (move1EnermyAi.transform.position.x == 1)
                        {
                            if (AiDistZ >= -0.1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, 1));
                            }
                            else
                            {
                                int rnd = UnityEngine.Random.Range(0, 2);
                                if (rnd == 0)
                                { move1EnermyAi.GetComponent<Units>().moveAble = false; }
                                else
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                                }
                            }
                        }
                        else if (move1EnermyAi.transform.position.x == mapMaxX)
                        {
                            if (AiDistZ >= -0.1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, 1));
                            }
                            else
                            {
                                int rnd = UnityEngine.Random.Range(0, 2);
                                if (rnd == 0)
                                { move1EnermyAi.GetComponent<Units>().moveAble = false; }
                                else
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                                }
                            }
                        }
                        else
                        {
                            if (AiDistZ < 0.1 && AiDistZ > -0.1)
                            {
                                if (AiDistX > 0.1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 1));
                                }
                                else if (AiDistX < -0.1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 1));
                                }
                            }
                            else
                            {
                                if (AiDistX > 0.1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                                }
                                else if (AiDistX < -0.1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                                }
                                else
                                {
                                    int rnd = UnityEngine.Random.Range(0, 2);
                                    if (rnd == 0)
                                    {
                                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                                        {
                                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                                        }
                                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                                    }
                                    else if (rnd == 1)
                                    {
                                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                                        {
                                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                                        }
                                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                                    }
                                }
                            }
                        }
                    }
                    //비숍
                    else if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (move1EnermyAi.transform.position.x == 1)
                        {
                            if (AiDistZ > -0.1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 1));
                            }
                            else if (AiDistZ < -0.1 && targetPlayerUnit1Ai.transform.position.x == 1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                            }
                            else move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else if (move1EnermyAi.transform.position.x == mapMaxX)
                        {
                            if (AiDistZ > -0.1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 1));
                            }
                            else if (AiDistZ < -0.1 && targetPlayerUnit1Ai.transform.position.x == mapMaxX)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                            }
                            else move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else
                        {
                            if (AiDistZ > -0.1)
                            {
                                if (AiDistX > 0.1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 1));
                                }
                                else if (AiDistX < -0.1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 1));
                                }
                                else
                                {
                                    int rnd = UnityEngine.Random.Range(0, 2);
                                    if (rnd == 0)
                                    {
                                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 1), out RaycastHit hit, 1f))
                                        {
                                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                                        }
                                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 1));
                                    }
                                    else
                                    {
                                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 1), out RaycastHit hit, 1f))
                                        {
                                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                                        }
                                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 1));
                                    }
                                }
                            }
                            else
                            {
                                if (AiDistX > 0.1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                                }
                                else if (AiDistX < -0.1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                                }
                                else
                                {
                                    int rnd = UnityEngine.Random.Range(0, 2);
                                    if (rnd == 0)
                                    {
                                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, -1), out RaycastHit hit, 1f))
                                        {
                                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                                        }
                                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, -1));
                                    }
                                    else
                                    {
                                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, -1), out RaycastHit hit, 1f))
                                        {
                                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                                        }
                                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, -1));
                                    }
                                }
                            }
                        }
                    }
                    //룩
                    else if (move1EnermyAi.GetComponent<Units>().rook && !move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        if (move1EnermyAi.transform.position.x == 1)
                        {
                            if (AiDistZ >= -0.1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, 1));
                            }
                            else
                            {
                                int rnd = UnityEngine.Random.Range(0, 2);
                                if (rnd == 0)
                                { move1EnermyAi.GetComponent<Units>().moveAble = false; }
                                else
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                                }
                            }
                        }
                        else if (move1EnermyAi.transform.position.x == mapMaxX)
                        {
                            if (AiDistZ >= -0.1)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, 1));
                            }
                            else
                            {
                                int rnd = UnityEngine.Random.Range(0, 2);
                                if (rnd == 0)
                                { move1EnermyAi.GetComponent<Units>().moveAble = false; }
                                else
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                                }
                            }
                        }
                        else
                        {
                            if (AiDistZ < -0.1)
                            {
                                if (AiDistX > 0.1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                                }
                                else if (AiDistX < -0.1)
                                {
                                    if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                                    {
                                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                                }
                                else
                                {
                                    if (AiDistZ > -2.1)
                                    {
                                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, -1), out RaycastHit hit, 1f))
                                        {
                                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                                        }
                                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                                    }
                                    else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, -1));
                                }
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(0, 0, 1), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(0, 0, 1));
                            }
                        }
                    }
                }
            }
            // 맵 맨 위
            else
            {
                if (move1EnermyAi.transform.position.x == 1)
                {
                    if (targetPlayerUnit1Ai.transform.position.x == 1 && move1EnermyAi.GetComponent<Units>().rook)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                    }
                    else move1EnermyAi.GetComponent<Units>().moveAble = false;
                }
                else if (move1EnermyAi.transform.position.x == mapMaxX)
                {
                    if (targetPlayerUnit1Ai.transform.position.x == mapMaxX && move1EnermyAi.GetComponent<Units>().rook)
                    {
                        if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                        {
                            move1EnermyAi.GetComponent<Units>().moveAble = false;
                        }
                        else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                    }
                    else move1EnermyAi.GetComponent<Units>().moveAble = false;
                }
                else
                {
                    //비숍
                    if (!move1EnermyAi.GetComponent<Units>().rook && move1EnermyAi.GetComponent<Units>().bishop)
                    {
                        move1EnermyAi.GetComponent<Units>().moveAble = false;
                    }
                    //룩
                    else if (move1EnermyAi.GetComponent<Units>().rook)
                    {
                        if (AiDistX > 0.1)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                        }
                        else if (AiDistX < -0.1)
                        {
                            if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                            {
                                move1EnermyAi.GetComponent<Units>().moveAble = false;
                            }
                            else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                        }
                        else
                        {
                            int rnd = UnityEngine.Random.Range(0, 2);
                            if (rnd == 0)
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(1, 0, 0), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(1, 0, 0));
                            }
                            else
                            {
                                if (Physics.Raycast(move1EnermyAi.transform.position, new Vector3(-1, 0, 0), out RaycastHit hit, 1f))
                                {
                                    move1EnermyAi.GetComponent<Units>().moveAble = false;
                                }
                                else move1EnermyAi.GetComponent<Units>().Move(new Vector3(-1, 0, 0));
                            }
                        }
                    }
                }
            }
        }
        aiSelect = false;
        move1EnermyAi = null;
    }

    // AI 유닛 세팅
    void findEnermyAndPlayer()
    {
        for (int i = 0; i < enermyUnits.Count; i++)
        {
            if (!enermyUnits[i].GetComponent<Units>().moveAble)
            {
                if (move1EnermyAi == enermyUnits[i])
                {
                    move1EnermyAi = null;
                }
            }
            else
            {
                if (enermyUnits[i].GetComponent<Units>().attackEvery)
                {
                    if (Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x) <= 2.1
                    && Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) <= 2.1
                    && enermyUnits[i].GetComponent<Units>().rook && Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x)
                    + Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) <= 3.1)
                    {
                        if (enermyUnits[i].GetComponent<Units>().pawn)
                        {
                            if (playerUnits[0].transform.position.z - enermyUnits[i].transform.position.z >= -1)
                            {
                                move1EnermyAi = enermyUnits[i];
                                targetPlayerUnit1Ai = playerUnits[0];
                                break;
                            }
                        }
                        else
                        {
                            move1EnermyAi = enermyUnits[i];
                            targetPlayerUnit1Ai = playerUnits[0];
                            break;
                        }
                    }
                    else if (Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x)
                    + Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) <= 4.1
                    && enermyUnits[i].GetComponent<Units>().bishop)
                    {
                        if (Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x)
                            - Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) < 0.1)
                        {
                            for (int j = 1; j < playerUnits.Count; j++)
                            {
                                if (Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x) >= Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[j].transform.position.x)
                              && Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) >= Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[j].transform.position.z))
                                {
                                    if (playerUnits[0].transform.position.x - playerUnits[j].transform.position.x <= 1.1 && playerUnits[0].transform.position.z - playerUnits[j].transform.position.z <= 1.1)
                                    { }
                                    else
                                    {
                                        if (enermyUnits[i].GetComponent<Units>().pawn)
                                        {
                                            if (playerUnits[0].transform.position.z - enermyUnits[i].transform.position.z <= 1)
                                            {
                                                move1EnermyAi = enermyUnits[i];
                                                targetPlayerUnit1Ai = playerUnits[0];
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            move1EnermyAi = enermyUnits[i];
                                            targetPlayerUnit1Ai = playerUnits[0];
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (enermyUnits[i].GetComponent<Units>().pawn)
                                    {
                                        if (playerUnits[0].transform.position.z - enermyUnits[i].transform.position.z <= 1)
                                        {
                                            move1EnermyAi = enermyUnits[i];
                                            targetPlayerUnit1Ai = playerUnits[0];
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        move1EnermyAi = enermyUnits[i];
                                        targetPlayerUnit1Ai = playerUnits[0];
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int j = 1; j < playerUnits.Count; j++)
                            {
                                if (Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x) >= Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[j].transform.position.x)
                              && Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) >= Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[j].transform.position.z))
                                {
                                    if (playerUnits[0].transform.position.x - playerUnits[j].transform.position.x <= 1.1 && playerUnits[0].transform.position.z - playerUnits[j].transform.position.z <= 1.1)
                                    { }
                                    else
                                    {
                                        if (enermyUnits[i].GetComponent<Units>().pawn)
                                        {
                                            if (playerUnits[0].transform.position.z - enermyUnits[i].transform.position.z <= 1)
                                            {
                                                move1EnermyAi = enermyUnits[i];
                                                targetPlayerUnit1Ai = playerUnits[0];
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            move1EnermyAi = enermyUnits[i];
                                            targetPlayerUnit1Ai = playerUnits[0];
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    if (enermyUnits[i].GetComponent<Units>().pawn)
                                    {
                                        if (playerUnits[0].transform.position.z - enermyUnits[i].transform.position.z <= 1)
                                        {
                                            move1EnermyAi = enermyUnits[i];
                                            targetPlayerUnit1Ai = playerUnits[0];
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        move1EnermyAi = enermyUnits[i];
                                        targetPlayerUnit1Ai = playerUnits[0];
                                        break;
                                    }
                                }
                            }
                        }
                    }

                }
                else
                { 
                    if (Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x) <= 1.1
                    && Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) <= 1.1
                    && enermyUnits[i].GetComponent<Units>().rook)
                    {
                        if (enermyUnits[i].GetComponent<Units>().pawn)
                        {
                            if (playerUnits[0].transform.position.z - enermyUnits[i].transform.position.z <= -1)
                            {
                                move1EnermyAi = enermyUnits[i];
                                targetPlayerUnit1Ai = playerUnits[0];
                                break;
                            }
                        }
                        else
                        {
                            move1EnermyAi = enermyUnits[i];
                            targetPlayerUnit1Ai = playerUnits[0];
                            break;
                        }
                    }
                    else if (Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x)
                    + Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) <= 3.1
                    && enermyUnits[i].GetComponent<Units>().bishop)
                    {
                        if (Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x)
                   - Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) < 0.1)
                        { }
                        else
                        {
                            for (int j = 1; j < playerUnits.Count; j++)
                            {
                                if (Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x) >= Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[j].transform.position.x)
                              && Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) >= Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[j].transform.position.z))
                                {
                                    if (enermyUnits[i].GetComponent<Units>().pawn)
                                    {
                                        if (playerUnits[0].transform.position.z - enermyUnits[i].transform.position.z <= 1)
                                        {
                                            move1EnermyAi = enermyUnits[i];
                                            targetPlayerUnit1Ai = playerUnits[0];
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        move1EnermyAi = enermyUnits[i];
                                        targetPlayerUnit1Ai = playerUnits[0];
                                        break;
                                    }         
                                }
                                else
                                {
                                    if (enermyUnits[i].GetComponent<Units>().pawn)
                                    {
                                        if (playerUnits[0].transform.position.z - enermyUnits[i].transform.position.z <= 1)
                                        {
                                            move1EnermyAi = enermyUnits[i];
                                            targetPlayerUnit1Ai = playerUnits[0];
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        move1EnermyAi = enermyUnits[i];
                                        targetPlayerUnit1Ai = playerUnits[0];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            
                for (int j = 0; j < playerUnits.Count; j++)
                {
                    if (move1EnermyAi == null)
                    {
                        move1EnermyAi = enermyUnits[i];
                        targetPlayerUnit1Ai = playerUnits[j];
                    }
                    else
                    {

                        if (Mathf.Abs(move1EnermyAi.transform.position.x - playerUnits[0].transform.position.x)
                    + Mathf.Abs(move1EnermyAi.transform.position.z - playerUnits[0].transform.position.z) >
                            Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x)
                    + Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z)
                        && enermyUnits[i].GetComponent<Units>().rook && Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x)
                    + Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) < 3)
                        {
                            if (enermyUnits[i].GetComponent<Units>().pawn)
                            {
                                if (playerUnits[0].transform.position.z - enermyUnits[i].transform.position.z <= -1)
                                {
                                    move1EnermyAi = enermyUnits[i];
                                    targetPlayerUnit1Ai = playerUnits[0];
                                    aiSelect = true;
                                    break;
                                }
                            }
                            else
                            {
                                move1EnermyAi = enermyUnits[i];
                                targetPlayerUnit1Ai = playerUnits[0];
                                aiSelect = true;
                                break;
                            }
                        }
                        else if (Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x)
                        + Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) <= 3.1
                        && enermyUnits[i].GetComponent<Units>().bishop)
                        {
                            if (Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x)
                       - Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) < 0.1)
                            { }
                            else
                            {
                                for (int p = 1; p < playerUnits.Count; p++)
                                {
                                    if (Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x) >= Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[p].transform.position.x)
                                  && Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z) >= Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[p].transform.position.z))
                                    {
                                        if (playerUnits[0].transform.position.x - playerUnits[j].transform.position.x <= 1.1 && playerUnits[0].transform.position.z - playerUnits[j].transform.position.z <= 1.1)
                                        { }
                                        else
                                        {
                                            if (enermyUnits[i].GetComponent<Units>().pawn)
                                            {
                                                if (playerUnits[0].transform.position.z - enermyUnits[i].transform.position.z <= -1)
                                                {
                                                    move1EnermyAi = enermyUnits[i];
                                                    targetPlayerUnit1Ai = playerUnits[0];
                                                    aiSelect = true;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                move1EnermyAi = enermyUnits[i];
                                                targetPlayerUnit1Ai = playerUnits[0];
                                                aiSelect = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (enermyUnits[i].GetComponent<Units>().pawn)
                                        {
                                            if (playerUnits[0].transform.position.z - enermyUnits[i].transform.position.z <= -1)
                                            {
                                                move1EnermyAi = enermyUnits[i];
                                                targetPlayerUnit1Ai = playerUnits[0];
                                                aiSelect = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            move1EnermyAi = enermyUnits[i];
                                            targetPlayerUnit1Ai = playerUnits[0];
                                            aiSelect=true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                        //기존 등록된것보다 플레이어 유닛에게 가까울 때
                        if (Mathf.Abs(move1EnermyAi.transform.position.x - playerUnits[0].transform.position.x)
                    + Mathf.Abs(move1EnermyAi.transform.position.z - playerUnits[0].transform.position.z) - move1EnermyAi.GetComponent<Units>().moveMaxCount >
                            Mathf.Abs(enermyUnits[i].transform.position.x - playerUnits[0].transform.position.x)
                    + Mathf.Abs(enermyUnits[i].transform.position.z - playerUnits[0].transform.position.z))
                        {                          
                            move1EnermyAi = enermyUnits[i];
                            targetPlayerUnit1Ai = playerUnits[j];
                        }
                    }
                }
            }
            aiSelect = true;
        }
    }
    //뒤집기
}
