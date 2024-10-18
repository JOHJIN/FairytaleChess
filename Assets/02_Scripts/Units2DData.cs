using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;

public class Units2DData : MonoBehaviour
{
    public object my2DCodeNum;
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

    float times = 0;
    public bool timecheck = false;
    public GameObject imformation2DChar;
    public GameObject canvas;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        gameObject.GetComponentInChildren<Text>().text = minNum + " ~ " + maxNum;
    }

    // Update is called once per frame
    void Update()
    {
        //if (UnityEngine.Input.GetMouseButtonDown(0))
        //{
        //    timecheck = true;
        //}
        //if (timecheck)
        //{
        //    times += Time.deltaTime;
        //    if (times > 1f)
        //    {
        //        information2D();
        //        timecheck = false;
        //        times = 0f;
        //    }
        //}
        //if (UnityEngine.Input.GetMouseButtonUp(0))
        //{
        //    timecheck = false;
        //    times = 0;
        //}
    }

    public void information2D()
    {
        GameObject info2D = Instantiate(imformation2DChar,canvas.transform);
    }
}
