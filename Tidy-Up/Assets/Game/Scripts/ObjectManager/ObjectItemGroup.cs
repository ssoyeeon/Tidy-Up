using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItemGroup : MonoBehaviour
    //Ʈ���ſ� ���� ��ũ��Ʈ�Դϴ�.
{
    public GameObject[] ObjectList = new GameObject[6];     //���� �� �ִ� ������Ʈ ����Ʈ 
    public int[] ObjectTriggerCheckList = new int[6];            //������Ʈ�� �������� ���Դ��� Ȯ�� �� ����Ʈ 1�̸� �Ϸ� 0�̸� �̿Ϸ�
   //DoneCheck ��Ƽ� �迭�� �� ���ؼ� ���� �ѱ�� 
   
    public int DoneCheck = 0;                               //üũ�� int �� �Դϴ�. RealDoneChecking �� ���� �����Դϴ�.
    public int RealDoneChecking = 6;                        //Ʈ������ �ִ� ������Ʈ�� ���� ������ �մϴ�. 
    public bool isDone;                                     //üũ�� ������ �� Ȱ��ȭ �� bool �� ����

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
                    // �� �׷쿡 ������ ���� ������Ʈ��� ������
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

            // ������ ����� �ƿ����� ����
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
        // ������ ����� �ƿ����� ����
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
