using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    public int Number;              //넘버 0이면 완료
    public int InObjectNumber;      //리스트에서 사용할 오브젝트 넘버 
    public bool isDone;             //다 끝났는지

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log(Number);
        if(Number == 1)
        {
            isDone = true;
        }
        else isDone = false;
    }

    public void OnTriggerExit(Collider other)
    {
        if (Number == 1)
        {
            isDone = false;
        }
        else isDone = true;
    }
}
