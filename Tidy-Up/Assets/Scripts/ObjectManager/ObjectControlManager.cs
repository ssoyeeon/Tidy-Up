using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlManager : MonoBehaviour
{
    //리스트에 넣고, 오브젝트 컨트롤에서 다 되면 리스트에서 빼야햄
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
            //objectsList.Remove(ObjectControl);        //왜 objectControl 두 개로 잡힐까여??ㅠㅠㅠ 어떻게 해야하는지... 모르겠어요..
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
