using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingIndicator : MonoBehaviour
{
    public float blinkInterval = 0.5f; // 깜박이는 간격 (초)
    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        StartCoroutine(Blink());
    }

    private System.Collections.IEnumerator Blink()
    {
        while (true)
        {
            objectRenderer.enabled = !objectRenderer.enabled; // 물체의 렌더러를 토글
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}


