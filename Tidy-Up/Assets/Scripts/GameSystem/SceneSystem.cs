using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    public string[] SceneName = new string[5];

    public void MainScene(int number)
    {
        SceneManager.LoadScene(SceneName[number]);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void StartScene()
    {
        SceneManager.LoadScene("IntroScene");
    }

    public void SettingScene()
    {
        SceneManager.LoadScene("SettingScene");
    }
}
