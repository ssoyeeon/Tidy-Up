using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public float playTime;
    public float ingTime;

    public bool isEnd;
    public bool isStart;
    public bool isPause;
    public bool isPlaying;

    public GameObject stopUI;
    public GameObject timeUI;

    public TMP_Text timeText;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); 
        timeUI.SetActive(false);
        stopUI.SetActive(false);
    }
    void Start()
    {
        playTime = 0; 
        isStart = true;
        isEnd = false; 
        ingTime = 10;
    }

    void Update()
    {
        if (isEnd == true)
        {
            isStart = false;
            timeText.text = (int)playTime+ "√ "; 
            timeUI.SetActive(true);
            ingTime -= Time.deltaTime;
            if(ingTime <= 0)
            {
                timeUI.SetActive(false);
            }
        }
        if (isStart == true)
        {
            ingTime = 10;
            if(isPlaying == true)
            {
                playTime += Time.deltaTime;
            }
        }
            Scene scene = SceneManager.GetActiveScene();
            int curScene = scene.buildIndex;
            if(curScene == 1)
            {
                isEnd = false;
                isStart = false;
                isPause = false;
                stopUI.SetActive(false);
            }
    }
        
}
