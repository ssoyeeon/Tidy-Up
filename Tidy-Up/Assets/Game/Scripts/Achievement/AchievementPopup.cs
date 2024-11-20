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

    void Start()
    {
        Hide();        
    }
    public void ShowAchievement(AchievementData achievement)
    {
        titleText.text = achievement.title;
        descriptionText.text = achievement.description;
        if(achievement.icon != null)
        {
            iconimage.sprite = achievement.icon;
        }
        Show();
    }

    private void Show()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void Hide()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

}
