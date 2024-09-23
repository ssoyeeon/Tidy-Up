using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public bool isOriginal = false;

    public void OnCollisionEnter(Collision collision)
    {
        if(gameObject.tag == "Table")
        {
            isOriginal = true;
        }
        else
        {
            isOriginal = false;
        }
    }
}
