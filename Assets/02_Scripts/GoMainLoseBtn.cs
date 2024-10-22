using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GoMainLoseBtn : MonoBehaviour
{
    public GameFlow gmflow;
    public GameMgr gmr;
    // Start is called before the first frame update
    void Start()
    {
        gmflow = GameObject.Find("GameFlow").GetComponent<GameFlow>();
        gmr = GameObject.Find("GameMgr").GetComponent<GameMgr>();
        this.GetComponent<Button>().onClick.AddListener(gmflow.LoseGoMain);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
