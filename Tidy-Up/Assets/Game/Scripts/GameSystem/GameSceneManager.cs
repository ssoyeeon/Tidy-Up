using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public Image fadeImage; // 페이드용 이미지
    public float fadeDuration = 1f; // 페이드 시간
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

        int nowScene = SceneManager.GetActiveScene().buildIndex;
        if(nowScene == 10)
        {
            SceneManager.LoadScene("IntroScene");
        }
        else
        {
            color.a = 1f;
            fadeImage.color = color;
            int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextScene);
        }

    }
}
