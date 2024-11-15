using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using System.Threading;

public class GameManager : MonoBehaviour
{
    public float playTime;
    public float ingTime;

    public bool isStart;
    public bool isPause;
    public bool isPlaying;
    public bool isDone;

    public GameObject stopUI;
    public GameObject timeUI;

    public TMP_Text timeText;
    
    public ObjectControlManager objectControlManager;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); 
       // timeUI.SetActive(false);
       // stopUI.SetActive(false);
    }
    void Start()
    {
        playTime = 0; 
        isStart = true;
        ingTime = 10;
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.F)) 
        {
            objectControlManager.isFinish = true;
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            objectControlManager.isFinish = false;
        }
            //3초 기다려도 true라면 
        if (objectControlManager.isFinish == true && SceneManager.GetActiveScene().buildIndex != 0)
        {
            //플레이어를 멈추고, UI 띄우기
            //PickupController pickupController = GetComponent<PickupController>();
            //pickupController.playerRigidbody.velocity = Vector3.zero;
            //isStart = false;
            //timeText.text = (int)playTime + "초";
            //timeUI.SetActive(true);
            //ingTime -= Time.deltaTime;
            //if (ingTime <= 0)
            //{
            //    timeUI.SetActive(false);
            //}
            objectControlManager.isFinish = false;
        }
        
        if (isStart == true)
        {
            ingTime = 10;
            if(isPlaying == true)
            {
                playTime += Time.deltaTime;
            }
        }
    }   
}
