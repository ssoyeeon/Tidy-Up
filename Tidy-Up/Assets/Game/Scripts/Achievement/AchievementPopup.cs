using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Image iconimage;
    [SerializeField] private float showDuration = 3f;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeSpeed = 1f; // ���̵� �ӵ� ����

    private Coroutine fadeCoroutine;

    void Start()
    {
        Hide();
    }

    public void ShowAchievement(AchievementData achievement)
    {
        titleText.text = achievement.title;
        descriptionText.text = achievement.description;
        if (achievement.icon != null)
        {
            iconimage.sprite = achievement.icon;
        }
        Show();
    }

    private void Show()
    {
        // ������ ���� ���� ���̵� �ڷ�ƾ�� �ִٸ� ����
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0;

        // ���̵� ��
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // ������ �ð���ŭ ǥ��
        yield return new WaitForSeconds(showDuration);

        // ���̵� �ƿ�
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }

        Hide();
    }

    private void Hide()
    {
        canvasGroup.alpha = 0;
    }

    private void OnDestroy()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
    }
}