using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartTextDocBAck : MonoBehaviour
{
    public GameObject canvasGroup;
    public CanvasGroup textCanvas;
    public float holdDuration = 5;
    public float timer = 4;
    private bool isEnd = false;

    private void Awake()
    {
        canvasGroup.SetActive(false);
    }
    void Start()
    {
        
    }

    void Update()
    {
        if(isEnd == false) timer -= Time.deltaTime;

        if(timer < 0)
        {
            isEnd = true;
            canvasGroup.SetActive(true);
            timer = 0;
            StartCoroutine(FadeOut());
        }
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(5f);
        textCanvas.DOFade(0f, 2f);
        canvasGroup.SetActive(false);

    }
}
