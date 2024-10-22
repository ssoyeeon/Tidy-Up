using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ObjectControl : MonoBehaviour
{
    public int Number;              //오브젝트 
    public int InObjectNumber;      //오브젝트 넘버
    public bool isDone;             //다 끝났는지

    public void OnTriggerEnter(Collider other)
    {
        
    }

    public void OnTriggerExit(Collider other)
    {
        
    }
}
