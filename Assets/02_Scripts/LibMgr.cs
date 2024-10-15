using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
using System.Linq;
using System;

public class LibMgr : MonoBehaviour
{
    public List<Dictionary<string, object>> playerable;
    public List<Dictionary<string, object>> bossList;
    public List<Dictionary<string, object>> normalUnits;
    public List<Dictionary<string, object>> uniqueUnits;
    public List<Dictionary<string, object>> otherUnits;
    public List<Dictionary<string, object>> normalUnitsEffects;
    public List<Dictionary<string, object>> uniqueUnitsEffects;
    public List<Dictionary<string, object>> otherUnitsEffects;
    public List<Dictionary<string, object>> bossStageList;

    public Dictionary<object, Dictionary<string,object>> unitCode = new Dictionary<object, Dictionary<string,object>>();
    public Dictionary<object, Dictionary<string, object>> unitCodeEffects = new Dictionary<object, Dictionary<string, object>>();
    public Dictionary<object, Dictionary<string, object>> bossCodeStage = new Dictionary<object, Dictionary<string, object>>();

    public List<List<object>> playerUnitsData = new List<List<object>>();
    public List<List<object>> bossToMeet = new List<List<object>>();

    public GameFlow gameflow;

    public int stageLevelCount = 0;
    public int playerableNum;
    void Start()
    {
        StartCoroutine(listAndCoad());
    }


    void Update()
    {
        if(Input.GetMouseButton(1))
            enermyLoadAndStage();
        if (Input.GetKeyDown(KeyCode.Keypad0))
            gameflow.StartBtn();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    //플레이어 데이터 로드 + 적 랜덤 + 스테이지 레벨 카운트
    public void loadPlayerableDate(int playerableNum2)
    {
        playerableNum = playerableNum2;
        playerUnitsData.Add(new List<object> { playerable[playerableNum]["ID"], 0 });
        playerUnitsData.Add(new List<object> { playerable[playerableNum]["기본"], 0 });
        playerUnitsData.Add(new List<object> { playerable[playerableNum]["기본"], 0 });
        enermyLoadAndStage();
    }
    public void enermyLoadAndStage()
    {
        int rnd = UnityEngine.Random.Range(0, bossList.Count);
        if (bossToMeet.Count < 1)
        {
            while (rnd == playerableNum) rnd = UnityEngine.Random.Range(0, bossList.Count);
            bossToMeet.Add(new List<object> { bossList[rnd]["ID"], rnd });
            //gameflow.StartBtn();
        }
        else
        {
            stageLevelCount++;

            while (rnd == playerableNum) rnd = UnityEngine.Random.Range(0, bossList.Count);
            bossToMeet.Add(new List<object> { bossList[rnd]["ID"], rnd });
            //gameflow.StartBtn();
        }
    }
    public IEnumerator listAndCoad()
    {
        playerable = CSVReader.Read("플레이어블 CSV");
        bossList = CSVReader.Read("보스 CSV");
        normalUnits = CSVReader.Read("기본 유닛 CSV");
        uniqueUnits = CSVReader.Read("고유 유닛 CSV");
        otherUnits = CSVReader.Read("외부 유닛 CSV");
        normalUnitsEffects = CSVReader.Read("기본 유닛 효과 CSV");
        uniqueUnitsEffects = CSVReader.Read("고유 유닛 효과 CSV");
        otherUnitsEffects = CSVReader.Read("외부 유닛 효과 CSV");
        bossStageList = CSVReader.Read("보스 지형 CSV");

        for (int a = 0; a < playerable.Count; a++)
        {
            unitCode.Add(playerable[a]["ID"], playerable[a]);
        }
        for (int a = 0; a < bossList.Count; a++)
        {
            unitCode.Add(bossList[a]["ID"], bossList[a]);
        }
        for (int a = 0; a < normalUnits.Count; a++)
        {
            unitCode.Add(normalUnits[a]["ID"], normalUnits[a]);
        }
        for (int a = 0; a < otherUnits.Count; a++)
        {
            unitCode.Add(otherUnits[a]["ID"], otherUnits[a]);
        }
        for (int a = 0; a < uniqueUnits.Count; a++)
        {
            unitCode.Add(uniqueUnits[a]["ID"], uniqueUnits[a]);
        }

        for (int a = 0; a < normalUnitsEffects.Count; a++)
        {
            unitCodeEffects.Add(normalUnitsEffects[a]["ID"], normalUnitsEffects[a]);
        }
        for (int a = 0; a < otherUnitsEffects.Count; a++)
        {
            unitCodeEffects.Add(otherUnitsEffects[a]["ID"], otherUnitsEffects[a]);
        }
        for (int a = 0; a < uniqueUnitsEffects.Count; a++)
        {
            unitCodeEffects.Add(uniqueUnitsEffects[a]["ID"], uniqueUnitsEffects[a]);
        }

        for (int a = 0; a < bossStageList.Count; a++)
        {
            bossCodeStage.Add(bossStageList[a]["ID"], bossStageList[a]);
        }
        yield return null;
    }
}
