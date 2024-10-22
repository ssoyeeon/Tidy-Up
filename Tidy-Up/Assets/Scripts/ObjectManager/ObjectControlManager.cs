using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlManager : MonoBehaviour
{
    //리스트에 넣고, 오브젝트 컨트롤에서 다 되면 리스트에서 빼야햄
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
