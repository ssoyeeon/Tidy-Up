using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartTextDocBAck : MonoBehaviour
{
    public TextMeshPro introText;
    public float textDuration = 3;
    public float holdDuration = 5;

    void Start()
    {
        FadeOut();
    }

    public IEnumerator FadeOut()
    {
        // 검정 화면 유지
        yield return new WaitForSeconds(holdDuration);

        float elapsedTime = 0f;
        Color color = introText.color;
        color.a = 1f;

        while (elapsedTime < textDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsedTime / textDuration);
            introText.color = color;
            yield return null;
        }

        color.a = 0f;
        introText.color = color;
    }
}
