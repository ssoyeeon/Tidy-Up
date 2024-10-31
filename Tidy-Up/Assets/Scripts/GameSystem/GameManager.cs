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
            //3�� ��ٷ��� true��� 
        if (objectControlManager.isFinish == true && SceneManager.GetActiveScene().buildIndex != 0)
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
            objectControlManager.isFinish = false;
            StartCoroutine(GoToNextScene());
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

    public IEnumerator GoToNextScene()
    {
        for(int i = 0; i < 3; i++)
        {
            timeText.text = (i+1).ToString();
            yield return new WaitForSeconds(1.0f);
        }       
       
        Scene scenes = SceneManager.GetActiveScene();
        int curScenes = scenes.buildIndex;
        SceneManager.LoadScene(curScenes + 1);
    }
        
}
