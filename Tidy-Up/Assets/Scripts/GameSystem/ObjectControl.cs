using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG;
using DG.Tweening;

public class ObjectControl : MonoBehaviour
{
    public bool isBed;
    public bool isChest;
    public bool isPicture;
    public bool isCorrect;

    void Update()
    {

        if (isCorrect == true)
        {
            Scene scene = SceneManager.GetActiveScene();
            int curScene = scene.buildIndex;
            int nextScene = curScene + 1;
            SceneManager.LoadScene(nextScene);
        }
        
    }
   
    public void OnCollisionStay(Collision collision)
    {
        if(gameObject.tag == "Table")
        {
           
        }
        else
        {
            //두트윈으로 아닐 때 메테리얼 바꾸기 -> 작업물이 들어와야 가능할 듯..ㅜㅜ
        }
        
        if(gameObject.tag == "Ground")
        {

        }
        else
        {

        }

        if(gameObject.tag == "Chest")
        {

        }
        else 
        {

        }
    }
}
