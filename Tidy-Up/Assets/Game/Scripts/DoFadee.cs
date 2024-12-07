using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DoFadee : MonoBehaviour
{
    public float timer = 2;
    public Image image;

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0 ) { image.DOFade( 0, 2f); timer = 0; }
    }
}
