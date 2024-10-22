using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlManager : MonoBehaviour
{
    //����Ʈ�� �ְ�, ������Ʈ ��Ʈ�ѿ��� �� �Ǹ� ����Ʈ���� ������
    public List<ObjectControl> objectsList = new List<ObjectControl>();
    public bool isDone;

    void Start()
    {
        isDone = false; 
    }

    void Update()
    {
        AllCheckDone();
    }

    void AllCheckDone()
    {
        if(objectsList.Count <= 0)
        {
            isDone = true;
        }
        else 
            isDone = false;
    }
}
