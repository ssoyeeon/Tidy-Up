using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float StartTimer = 9;
    public float StopTimer = 3;
    public float NextScene = 20;
    public GameObject firstObject;
    public GameObject Object;
    public Image fadeImage; // 페이드용 이미지
    public float fadeDuration = 1f; // 페이드 시간

    private void Awake()
    {
        firstObject.SetActive(true);
        Object.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        StartTimer -= Time.deltaTime;
        NextScene -= Time.deltaTime;
        if(StartTimer <= StopTimer) Object.SetActive(true);
        if (StartTimer <= 0) { firstObject.SetActive(false); StartTimer = 0; }
        if (NextScene <= 5)
        {
            StartCoroutine(FadeOut());
        }
        if (NextScene <= 0) SceneManager.LoadScene("Age100");
    }

    public IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        color.a = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;
    }
}
