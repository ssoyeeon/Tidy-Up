using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItem : MonoBehaviour
    //트리거에 넣을 스크립트입니다.
{
    private GameObject objectInTrigger;                     //트리거 안에 있는 물체 추적
    public float exitDistanceThreshold = 1.0f;              //트리거 나간 거리 계산 할 값
    public GameObject[] ObjectList = new GameObject[6];     //잡을 수 있는 오브젝트 리스트 
    public int[] ObjectTriggerList = new int[6];            //오브젝트가 나갔는지 들어왔는지 확인 할 리스트 1이면 완료 0이면 미완료
    public int TriggerNumber;                               //오브젝트 넘버와 비교할 본인의 트리거 넘버.
    private ObjectControl objectControl;
    public int DoneCheck = 0;                               //체크용 int 수 입니다. RealDoneChecking 과 비교할 예정입니다.
    public int RealDoneChecking = 6;                        //트리거의 있는 오브젝트와 같은 수여야 합니다. 
    public bool isDone;                                     //체크가 끝났을 때 활성화 할 bool 값 변수

    public void Start()
    {
        isDone = false;
        DoneCheck = 0;
    }
    public void Update()
    {
        float distance = Vector3.Distance(transform.position, objectInTrigger.transform.position);

        if (distance > exitDistanceThreshold)
        {
            for(int i = 0; i < ObjectList.Length; i++)
            {
                ObjectTriggerList[i] = 0;
            }
        }
        CheckDone();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (objectControl.objectNumber == TriggerNumber)
        {
            for (int i = 0; i < ObjectList.Length; i++)
            {
                ObjectTriggerList[i] = 1;
                if (ObjectTriggerList[i] == 1)
                {
                    DoneCheck += 1;
                    break;
                }
            }
        }
    }

    public void CheckDone()
    {
        if (DoneCheck != RealDoneChecking)
        {
            for (int i = 0; i < ObjectList.Length; i++)
            {
                if (ObjectTriggerList[i] == 1)
                {
                    DoneCheck += 1;
                }
            }
        }
        else
        {
            isDone = true;  
        }
    }
}
