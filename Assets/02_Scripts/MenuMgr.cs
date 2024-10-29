using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuMgr : MonoBehaviour
{
    public GameObject CharacterSelectPanel;
    public GameObject gameFlow;
    public LibMgr library;

    public int charNum = 100;

    public Button strBtn;

    public AudioSource playerAudio;
    public AudioClip clickAudioClip;
    public AudioClip charactSelectAudio;
    public AudioClip backgroundAudio;

    public GameObject soundPanel;
    public Slider mainSoundBar;
    public Slider backGroundSoundBar;
    public Slider effectSoundBar;
    // Start is called before the first frame update
    void Start()
    {
        gameFlow = GameObject.Find("GameFlow");
        library = GameObject.Find("LibMgr").GetComponent<LibMgr>();
        charNum = 100;
        mainSoundBar.value = gameFlow.GetComponent<GameFlow>().mainSoundSize;
        backGroundSoundBar.value = gameFlow.GetComponent<GameFlow>().bgmSoundSize;
        effectSoundBar.value = gameFlow.GetComponent<GameFlow>().effectSoundSize;
        playerAudio.PlayOneShot(backgroundAudio, gameFlow.GetComponent<GameFlow>().bgmSoundSize);
    }

    // Update is called once per frame
    void Update()
    {
        playerAudio.volume = gameFlow.GetComponent<GameFlow>().mainSoundSize;
        if (soundPanel)
        {
            gameFlow.GetComponent<GameFlow>().mainSoundSize = mainSoundBar.value;
            gameFlow.GetComponent<GameFlow>().bgmSoundSize = backGroundSoundBar.value;
            gameFlow.GetComponent<GameFlow>().effectSoundSize = effectSoundBar.value;
        }
    }
    public void characterSelectBtn()
    {
        library.playerUnitsData.Clear();
        library.bossToMeet.Clear();
        library.stageLevelCount = 0;
        library.money = 0;
        library.playerableNum = 0;
        CharacterSelectPanel.SetActive(true);
        strBtn.enabled = false;
    }

    public void charSelectNum(int a)
    {
        charNum = a;
        strBtn.enabled = true;
    }
    public void charcterSelectToStart()
    {
        library.loadPlayerableDate(charNum);
    }
    public void ContinueBtn()
    {
        SceneManager.LoadScene(2);
    }

    public void gameQuitBtn()
    {
        Application.Quit();
    }

    public void clickSound()
    {
        playerAudio.PlayOneShot(clickAudioClip, gameFlow.GetComponent<GameFlow>().effectSoundSize);
    }
    public void cSelectAudio()
    {
        playerAudio.PlayOneShot(charactSelectAudio, gameFlow.GetComponent<GameFlow>().effectSoundSize);
    }
}
