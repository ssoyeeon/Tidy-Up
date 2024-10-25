using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlManager : MonoBehaviour
{    
    public bool isFinish;
    public int[] TriggerDone = new int[10];
    public int TriggerDoneCount = 10;
    public int TriggerCheckCount = 0;
  
    void Update()
    {

    }

    public void ObjectCheckDone(int triggerID)
    {
        TriggerCheckCount = 0;
        TriggerDone[triggerID] = 1;

        for(int i = 0; i < TriggerDone.Length; i++) 
        {
            TriggerCheckCount += TriggerDone[i];
        }

        if(TriggerCheckCount == TriggerDoneCount)
        {
            isFinish = true;
        }
    }
}
