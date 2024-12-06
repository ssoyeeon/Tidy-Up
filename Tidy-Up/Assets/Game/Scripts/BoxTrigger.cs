using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    public int BoxItem;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(tag == "Picker")
        {
            
        }
    }
}
