using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    public int[] Number = new int[7];   //0�̸� ���� 1�̸� �Ϸ� 2�� ����Ǹ� �ƿ� �� �̰� ���Ȥ������� �ޤŤˤ��� �Ǥо�
    public int TriggerObjectNumber; //Ʈ���� ������Ʈ�� ��ȣ
    public bool isDone;             //�� ��������
    public List<GameObject> objectsList = new List<GameObject>(6);
    public int EndNumber;               //üũ ����
    public int DoneNumber;              //�Ϸ� ���� ����
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
        for (int i = 0; i < objectsList.Count; i++)
        {
            if (other.gameObject.GetComponent<ObjectItem>().ItemNumber == objectsList[i].GetComponent<ObjectItem>().ItemNumber)
            {
                Number[i] = 0;
            }
        }
        for (int i = 0; i < objectsList.Count; i++)
        {
            EndNumber -= Number[i];
        }
    }

    public void DoneCheck()
    {
        for (int i = 0; i < objectsList.Count; i++)
        {
            if(Number[i] == 1)
            {
                DoneNumber += 1;
                break;
            }
        }
        /*if(DoneNumber == DoneCheckingTrigger)
        {
            objectControlManager.ObjectCheckDone(myID);
        }*/
    }
    void Update()
    {
        DoneCheck();
    }
}
