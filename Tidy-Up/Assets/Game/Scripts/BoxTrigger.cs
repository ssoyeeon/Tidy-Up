using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxTrigger : MonoBehaviour
{
    public int BoxItem;

    void Start()
    {
        BoxItem = GameObject.FindGameObjectsWithTag("Picker").Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Picker"))
        {
            Destroy(other.gameObject);

            BoxItem -= 1;

            if(BoxItem <= 0)
            {
                SceneManager.LoadScene("EndScene");
            }
        }
    }
}
