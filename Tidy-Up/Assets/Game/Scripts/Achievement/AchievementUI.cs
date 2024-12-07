using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AchievementUI : MonoBehaviour
{
    [SerializeField] private AchievementPopup popupPrefab;
    private Queue<AchievementData> achievementQueue = new Queue<AchievementData>();
    [SerializeField] private Canvas targetCanvas; // Ÿ�� ĵ���� �߰�
    private AchievementPopup currentPopup;
    private bool isShowingPopup = false;

    private void Start()
    {
        // AchievementManager�� �̺�Ʈ ����
        AchievementManager.Instance.OnAchievementUnlocked += OnAchievementUnlocked;
    }

    private void OnAchievementUnlocked(AchievementData achievement)
    {
        achievementQueue.Enqueue(achievement);
        if (!isShowingPopup)
        {
            ShowNextAchievement();
        }
    }

    private void ShowNextAchievement()
    {
        if (achievementQueue.Count == 0)
        {
            isShowingPopup = false;
            return;
        }

        isShowingPopup = true;
        var achievement = achievementQueue.Dequeue();

        if (currentPopup == null)
        {
            currentPopup = Instantiate(popupPrefab, targetCanvas.transform);
        }

        currentPopup.ShowAchievement(achievement);
        StartCoroutine(WaitForPopup());
    }

    private IEnumerator WaitForPopup()
    {
        yield return new WaitForSeconds(3.5f); // popup duration + �ణ�� ����
        ShowNextAchievement();
    }
}
