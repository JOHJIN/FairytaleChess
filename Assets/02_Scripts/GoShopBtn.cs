using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoShopBtn : MonoBehaviour
{
    public GameFlow gmflow;
    // Start is called before the first frame update
    void Start()
    {
        gmflow = GameObject.Find("GameFlow").GetComponent<GameFlow>();
        this.GetComponent<Button>().onClick.AddListener(gmflow.GoShopBtn);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
