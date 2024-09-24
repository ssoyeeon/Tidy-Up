using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
}
