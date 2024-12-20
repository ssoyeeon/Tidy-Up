using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItemGroup : MonoBehaviour
    //트리거에 넣을 스크립트입니다.
{
    public GameObject[] ObjectList = new GameObject[6];     //잡을 수 있는 오브젝트 리스트 
    public int[] ObjectTriggerCheckList = new int[6];            //오브젝트가 나갔는지 들어왔는지 확인 할 리스트 1이면 완료 0이면 미완료
   //DoneCheck 모아서 배열로 다 더해서 게임 넘기기 
   
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
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Picker")
        {
            ObjectItem objectControl = other.GetComponent<ObjectItem>();
            OutlineController outline = other.GetComponent<OutlineController>();

            for (int i = 0; i < ObjectList.Length; i++)
            {
                if (ObjectList[i].gameObject.GetComponent<ObjectItem>().objectNumber == objectControl.objectNumber && ObjectTriggerCheckList[i] == 0)
                {
                    ObjectTriggerCheckList[i] = 1;
                    objectControl.gameObject.GetComponent<ObjectItem>().group = this;
                    objectControl.gameObject.GetComponent<ObjectItem>().inGroup = true;
                    if (outline) outline.SetHighlightState(OutlineController.HighlightState.Correct);
                    break;
                }
                else if (ObjectList[i].gameObject.GetComponent<ObjectItem>().objectNumber != objectControl.objectNumber)
                {
                    // 이 그룹에 속하지 않은 오브젝트라면 빨간색
                    if (outline) outline.SetHighlightState(OutlineController.HighlightState.Incorrect);
                }
            }

            CheckDone();
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Picker")
        {
            ObjectItem temp = other.GetComponent<ObjectItem>();
            OutlineController outline = other.GetComponent<OutlineController>();
            for (int i = 0; i < ObjectList.Length; i++)
            {
                if (temp.objectNumber == ObjectList[i].GetComponent<ObjectItem>().objectNumber)
                {
                    ObjectTriggerCheckList[i] = 0;
                    break;
                }
            }

            temp.gameObject.GetComponent<ObjectItem>().group = null;
            temp.gameObject.GetComponent<ObjectItem>().inGroup = true;

            // 범위를 벗어나면 아웃라인 제거
            if (outline) outline.SetHighlightState(OutlineController.HighlightState.None);

            CheckDone();
        }
    }

    public void ObjectOut(ObjectItem temp)
    {
        OutlineController outline = temp.gameObject.GetComponent<OutlineController>();
        for (int i = 0; i < ObjectList.Length; i++)
        {
            if(temp.objectNumber == ObjectList[i].GetComponent<ObjectItem>().objectNumber)
            {
                ObjectTriggerCheckList[i] = 0;
                break;
            }           
        }

        temp.gameObject.GetComponent<ObjectItem>().group = null;
        temp.gameObject.GetComponent<ObjectItem>().inGroup = true;
        // 범위를 벗어나면 아웃라인 제거
        if (outline) outline.SetHighlightState(OutlineController.HighlightState.None);
        CheckDone();

    }

    public void CheckDone()
    {
        DoneCheck = 0;

        for (int i = 0; i < ObjectList.Length; i++)
        {
            if (ObjectTriggerCheckList[i] == 1)
            {
                DoneCheck += 1;
            }
        }

        if(RealDoneChecking == DoneCheck)
        {
            isDone = true;
        }
        else
        {
            isDone = false;
        }

                                
    }
}
