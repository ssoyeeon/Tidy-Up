using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlManager : MonoBehaviour
{
    public ObjectItem objectItem;
    public bool isFinish;               //끝났는지 알려줄 Bool 값
    public GameObject[] triggerObjects = new GameObject[4];     //Trigger 오브젝트를 넣어둘 예정입니다.
    public int[] triggerNumber = new int[4];                    //Trigger들이 다 끝났는지 세어줄 배열입니다.

    public void Start()
    {
        
    }
    public void Update()
    {
        /*if(objectItem.isDone == true)
        {
            for (int i = 0; i < triggerObjects.Length; i++)
            {
                triggerNumber[i] = 1;
                Debug.Log( i + "번이 끝났습니다.");
            }
        }*/
    }
}
