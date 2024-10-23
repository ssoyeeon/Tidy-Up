using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlManager : MonoBehaviour
{
    //����Ʈ�� �ְ�, ������Ʈ ��Ʈ�ѿ��� �� �Ǹ� ����Ʈ���� ������
    public ObjectControl[] objectsList = new ObjectControl[10];
    ObjectControl objectcontrol;
    public bool isFinish;
    //ObjectControl���� isDone = true�� �ָ� ����Ʈ���� ����. �ٵ� �̰� ���� �� ������ ������ ������ ������... ��� ������Ʈ�� �� ��������.
    PickupController pickupcontroller;

    void Update()
    {
        if(pickupcontroller.objectList.Count == 0)  
        {
            AllCheckDone();
        }
    }

    void AllCheckDone()
    {
        if(objectcontrol.isDone == true)
        {
            objectsList[objectcontrol.InObjectNumber] = null;
        }
        if(objectsList.Length <= 0)
        {
            isFinish = true;
        }
        else
            isFinish = false;
    }
}
