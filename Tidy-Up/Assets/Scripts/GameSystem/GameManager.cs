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
        if (objectControlManager.isFinish == true)
        {
            //3�� ��ٷ��� true��� 
            if (objectControlManager.isFinish == true)
            {
                //�÷��̾ ���߰�, UI ����
                //PickupController pickupController = GetComponent<PickupController>();
                //pickupController.playerRigidbody.velocity = Vector3.zero;
                //isStart = false;
                //timeText.text = (int)playTime + "��";
                //timeUI.SetActive(true);
                //ingTime -= Time.deltaTime;
                //if (ingTime <= 0)
                //{
                //    timeUI.SetActive(false);
                //}
                Scene scenes = SceneManager.GetActiveScene();
                int curScenes = scenes.buildIndex;
                SceneManager.LoadScene(curScenes + 1);
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
        if (curScene == 1)
        {
            //objectControlManager.isFinish = false;
            isStart = false;
            isPause = false;
            //stopUI.SetActive(false);
        }
    }
        
}
