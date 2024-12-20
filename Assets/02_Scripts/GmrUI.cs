using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GmrUI : MonoBehaviour
{
    GameMgr gmr;

    public Text whosTurnTxt;
    public Text moveCountTxt;

    public Image whosTurnImage;

    public GameObject selectUnitPanel;
    public Image selectUnitImage;
    public Text selectNameTxt;
    public Text selectEffectTxt;
    public Text selectMoveTxt;
    public Text selectNumTxt;

    // Start is called before the first frame update
    void Start()
    {
        gmr = gameObject.GetComponent<GameMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gmr.selectUnit != null)
        {
            selectUnitPanel.SetActive(true);
            selectNameTxt.text = gmr.selectUnit.GetComponent<Units>().name + " + " + gmr.selectUnit.GetComponent<Units>().upgradeRank;
            selectEffectTxt.text = gmr.selectUnit.GetComponent<Units>().unitEffectTxt;
            selectMoveTxt.text = "Move : " + gmr.selectUnit.GetComponent<Units>().moveMaxCount;
            selectNumTxt.text = gmr.selectUnit.GetComponent<Units>().minNum + " ~ " + gmr.selectUnit.GetComponent<Units>().maxNum;
            selectUnitImage.sprite = gmr.selectUnit.GetComponent<Units>().unit2DImage;
            if (gmr.selectUnit.GetComponent<Units>().maxNum == 0)
            {
                selectNumTxt.text = "-";
            }
        }
        else
        {
            selectUnitPanel.SetActive(false);
        }
    }

    public void whosTurnTxtChange()
    {
        if (gmr.placementTime)
        {
            whosTurnTxt.text = "Player Turn";
            whosTurnImage.sprite = Resources.Load<Sprite>("UIColor/BlueUI");
        }
        else
        {
            if (gmr.playerTurn)
            {
                whosTurnTxt.text = "Enermy Turn";
                whosTurnImage.sprite = Resources.Load<Sprite>("UIColor/RedUI");
            }
            else
            {
                whosTurnTxt.text = "Player Turn";
                whosTurnImage.sprite = Resources.Load<Sprite>("UIColor/BlueUI");
            }
        }
    }
    public void moveCountTxtChange(int a)
    {
        moveCountTxt.text = "���� �̵� Ƚ�� " + a;
    }
}
