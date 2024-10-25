using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    public int[] Number = new int[7];   //0이면 실패 1이면 완료 2로 변경되면 아예 끝 이게 뭐ㅘㅇㄴ마ㅣ 휴ㅕㅛㅑㅁ ㅗㅠ아
    public int TriggerObjectNumber; //트리거 오브젝트의 번호
    public bool isDone;             //다 끝났는지
    public List<GameObject> objectsList = new List<GameObject>(6);
    public int EndNumber;               //체크 숫자
    public int DoneNumber;              //완료 실제 숫자
    public ObjectControlManager objectControlManager;
    public int myID;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        for(int i = 0; i < objectsList.Count; i++)
        {
            if (other.gameObject.GetComponent<ObjectItem>().ItemNumber == objectsList[i].GetComponent<ObjectItem>().ItemNumber)
            {
                Number[i] = 1;
            }
        }
        for (int i = 0; i < objectsList.Count; i++)
        {
            EndNumber += Number[i];
        }
        Debug.Log("EndNumber ->" + EndNumber);
    }

    public void OnTriggerExit(Collider other)
    {
        
    }

    public void DoneCheck()
    {
        if(EndNumber == DoneNumber)
        {
            objectControlManager.ObjectCheckDone(myID);
        }
    }
}
