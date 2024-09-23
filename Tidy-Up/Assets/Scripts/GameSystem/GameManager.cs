using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float playTime;

    void Start()
    {
        playTime = 0;
    }

    void Update()
    {
        playTime += Time.deltaTime;
    }
}
