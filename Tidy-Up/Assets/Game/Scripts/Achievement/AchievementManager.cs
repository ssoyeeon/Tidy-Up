using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [SerializeField] private AchievementData[] achievementList;
    private Dictionary<string, bool> unlockedAchievements = new Dictionary<string, bool>();
    private float levelStartTime;

    public event Action<AchievementData> OnAchievementUnlocked;

    public GameObject bar;

    [SerializeField] private bool resetAchievements = false; // Inspector에서 체크박스로 표시됨


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            InitializeAchievements();
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        levelStartTime = Time.time;

        // 체크박스가 체크되어 있으면 업적 초기화
        if (resetAchievements)
        {
            // PlayerPrefs 초기화
            foreach (var achievement in achievementList)
            {
                PlayerPrefs.DeleteKey($"Achievement_{achievement.id}");
            }
            PlayerPrefs.Save();

            // Dictionary 초기화
            foreach (var achievement in achievementList)
            {
                unlockedAchievements[achievement.id] = false;
            }

            Debug.Log("업적이 초기화되었습니다.");
            resetAchievements = false; // 초기화 후 체크박스 해제
        }
    }

    private void InitializeAchievements()
    {
        foreach (var achievement in achievementList)
        {
            unlockedAchievements[achievement.id] = false;
        }
        LoadAchievements();
    }

    public void OnItemCorrectlyPlaced()
    {
        UnlockAchievement("FIRST_ITEM");
    }

    public void OnFirstItemPickUp()
    {
        UnlockAchievement("FIRST_PICKUP");
    }

    public void OnGroupCompleted()
    {
        UnlockAchievement("GROUP_COMPLETE");
        float timeToComplete = Time.time - levelStartTime;
        if(timeToComplete <= 60f )
        {
            UnlockAchievement("SPEED_MASTER");
        }
    }

    public void OnAllGroupsCompleted()
    {
        UnlockAchievement("ALL_COMPLETE");
    }

    private void UnlockAchievement(string Id)
    {
        if (!unlockedAchievements.ContainsKey(Id)) return;
        if (unlockedAchievements[Id]) return;

        unlockedAchievements[Id] = true;

        var achievement = Array.Find(achievementList, a => a.id == Id);
        if(achievement != null )
        {
            OnAchievementUnlocked?.Invoke(achievement);
            SaveAchievements();
            Debug.Log($"업적 달성 : {achievement.title}");
        }
    }

    private void SaveAchievements()
    {
        foreach(var achievement in unlockedAchievements)
        {
            PlayerPrefs.SetInt($"Achievement_{achievement.Key}", achievement.Value ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    private void LoadAchievements()
    {
        foreach (var achievement in achievementList)
        {
            unlockedAchievements[achievement.id] = PlayerPrefs.GetInt($"Achievement_{achievement.id}", 0) == 1;
        }
    }

    public bool IsAchievementUnlocked(string Id)
    {
        return unlockedAchievements.ContainsKey (Id) && unlockedAchievements[Id];
    }
}
