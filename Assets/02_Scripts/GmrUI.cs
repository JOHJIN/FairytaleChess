using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GmrUI : MonoBehaviour
{
    GameMgr gmr;

    public Text whosTurnTxt;
    public Text moveCountTxt;

    public Image whosTurnImage;
    // Start is called before the first frame update
    void Start()
    {
        gmr = gameObject.GetComponent<GameMgr>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void whosTurnTxtChange()
    {
        if (gmr.placementTime)
        {
            whosTurnTxt.text = "Player Turn";
            whosTurnImage.color = Color.cyan;
        }
        else
        {
            if (gmr.playerTurn)
            {
                whosTurnTxt.text = "Enermy Turn";
                whosTurnImage.color = Color.red;
            }
            else
            {
                whosTurnTxt.text = "Player Turn";
                whosTurnImage.color = Color.cyan;
            }
        }
    }
    public void moveCountTxtChange(int a)
    {
        moveCountTxt.text = "남은 이동 횟수 " + a;
    }
}
