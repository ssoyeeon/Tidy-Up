using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingIndicator : MonoBehaviour
{
    public float blinkInterval = 0.5f; // �����̴� ���� (��)
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
            objectRenderer.enabled = !objectRenderer.enabled; // ��ü�� �������� ���
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}


