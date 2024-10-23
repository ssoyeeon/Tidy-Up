using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectControl : MonoBehaviour
{
    public int Number;              //�ѹ� 0�̸� �Ϸ�
    public int InObjectNumber;      //����Ʈ���� ����� ������Ʈ �ѹ� 
    public bool isDone;             //�� ��������

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
