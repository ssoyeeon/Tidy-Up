using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] private Group[] groups;
    private bool isTransitioning;

    private void Update()
    {
        if (isTransitioning) return;

        if(CheckAllGroupsComplete())
        {
            isTransitioning = true;

            AchievementManager.Instance.OnAllGroupsCompleted();
            StartCoroutine(LoadNextScene());
        }
    }

    private bool CheckAllGroupsComplete()
    {
        foreach(var group in groups)
        {
            if(!group.isComplete) return false;
        }
        return true;
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(3f);
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }
}
