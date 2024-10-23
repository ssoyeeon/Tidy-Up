using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlManager : MonoBehaviour
{
    //리스트에 넣고, 오브젝트 컨트롤에서 다 되면 리스트에서 빼야햄
    public ObjectControl[] objectsList = new ObjectControl[10];
    ObjectControl objectcontrol;
    public bool isFinish;
    //ObjectControl에서 isDone = true인 애만 리스트에서 제거. 근데 이거 끝날 때 봐야함 볼거임 끝날때 볼거임... 모든 오브젝트가 다 나왔을때.
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
