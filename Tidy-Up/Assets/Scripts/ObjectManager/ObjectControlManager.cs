using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlManager : MonoBehaviour
{
    //����Ʈ�� �ְ�, ������Ʈ ��Ʈ�ѿ��� �� �Ǹ� ����Ʈ���� ������
    public List<ObjectControl> objectsList = new List<ObjectControl>();
    public bool isDone;
    ObjectControl objectControl;

    void Start()
    {
        isDone = false; 
    }

    void Update()
    {
        if (objectControl.isDone == true)
        {
            //objectsList.Remove(ObjectControl);        //�� objectControl �� ���� �����??�ФФ� ��� �ؾ��ϴ���... �𸣰ھ��..
        }
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
