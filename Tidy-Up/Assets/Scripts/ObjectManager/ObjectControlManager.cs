using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControlManager : MonoBehaviour
{
    public ObjectItem objectItem;
    public bool isFinish;               //�������� �˷��� Bool ��
    public GameObject[] triggerObjects = new GameObject[4];     //Trigger ������Ʈ�� �־�� �����Դϴ�.
    public int[] triggerNumber = new int[4];                    //Trigger���� �� �������� ������ �迭�Դϴ�.

    public void Start()
    {
        
    }
    public void Update()
    {
        /*if(objectItem.isDone == true)
        {
            for (int i = 0; i < triggerObjects.Length; i++)
            {
                triggerNumber[i] = 1;
                Debug.Log( i + "���� �������ϴ�.");
            }
        }*/
    }
}
