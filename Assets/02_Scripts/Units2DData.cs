using JetBrains.Annotations;
using System;
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

    public GameObject imformation2DChar;
    public GameObject canvas;

    public Image unit2DImage;
    public GameObject unitEffectTxt;
    public GameObject unitMoveTxt;
    public GameObject unitNumTxt;
    public GameObject unitNameTxt;

    public string itsEffect;

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void information2D()
    {
        GameObject info2D = Instantiate(imformation2DChar,canvas.transform);
    }
}
