using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItem : MonoBehaviour
    //Ʈ���ſ� ���� ��ũ��Ʈ�Դϴ�.
{
    private GameObject objectInTrigger;                     //Ʈ���� �ȿ� �ִ� ��ü ����
    public float exitDistanceThreshold = 1.0f;              //Ʈ���� ���� �Ÿ� ��� �� ��
    public GameObject[] ObjectList = new GameObject[6];     //���� �� �ִ� ������Ʈ ����Ʈ 
    public int[] ObjectTriggerList = new int[6];            //������Ʈ�� �������� ���Դ��� Ȯ�� �� ����Ʈ 1�̸� �Ϸ� 0�̸� �̿Ϸ�
    public int TriggerNumber;                               //������Ʈ �ѹ��� ���� ������ Ʈ���� �ѹ�.
    private ObjectControl objectControl;
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
