using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject Canvas;
    public bool istrue;

    private void Awake()
    {
        Canvas.SetActive(true);
        istrue = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q) && istrue == false)
        {
            Canvas.gameObject.SetActive(false);
            istrue = true;
            return;
        }
        if(istrue == true && Input.GetKeyDown(KeyCode.Q))
        {
                Canvas.SetActive(true);
                istrue = false; return;
        }
    }
}
