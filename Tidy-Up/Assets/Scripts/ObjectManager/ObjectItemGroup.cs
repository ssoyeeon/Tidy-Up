using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItemGroup : MonoBehaviour
    //Ʈ���ſ� ���� ��ũ��Ʈ�Դϴ�.
{
    public GameObject[] ObjectList = new GameObject[6];     //���� �� �ִ� ������Ʈ ����Ʈ 
    public int[] ObjectTriggerCheckList = new int[6];            //������Ʈ�� �������� ���Դ��� Ȯ�� �� ����Ʈ 1�̸� �Ϸ� 0�̸� �̿Ϸ�
   
   
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
           
            for (int i = 0; i < ObjectList.Length; i++)
            {
                if (ObjectList[i].gameObject.GetComponent<ObjectItem>().objectNumber == objectControl.objectNumber && ObjectTriggerCheckList[i] == 0)
                {
                    ObjectTriggerCheckList[i] = 1;
                    objectControl.gameObject.GetComponent<ObjectItem>().group = this;
                    objectControl.gameObject.GetComponent<ObjectItem>().inGroup = true;
                    break;
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

            CheckDone();
        }
    }

    public void ObjectOut(ObjectItem temp)
    {

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
