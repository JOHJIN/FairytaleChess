using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoMainBtn : MonoBehaviour
{
    public GameFlow gmflow;
    public GameMgr gmr;
    // Start is called before the first frame update
    void Start()
    {
        gmflow = GameObject.Find("GameFlow").GetComponent<GameFlow>();
        gmr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        if(gmr.lose)
            this.GetComponent<Button>().onClick.AddListener(gmflow.LoseGoMain);
        else
            this.GetComponent<Button>().onClick.AddListener(gmflow.GoMainBtn);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
