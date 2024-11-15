using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlManager : MonoBehaviour
{
    public List<Group> objectItemGroups = new List<Group>();
    public bool isFinish;
    public bool isDone;

    public void Update()
    {
        if(!isDone)
        CheckAll();
    }
    public void CheckAll()
    {
        int doneCount = 0;
        foreach(var item in objectItemGroups)
        {
            if(item.isComplete)
            {
                doneCount++;
            }
        }

        if(doneCount == objectItemGroups.Count)
        {
            Debug.Log("��� �Ϸ�");
            isFinish = true;
            isDone = true;
        }
    }
}
