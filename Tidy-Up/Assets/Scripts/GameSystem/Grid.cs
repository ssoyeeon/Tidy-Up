using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other)
        Debug.Log("´ê¾Ò´ç");
    }
}
