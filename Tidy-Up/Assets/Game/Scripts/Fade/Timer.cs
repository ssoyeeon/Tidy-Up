using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float StartTimer = 9;
    public float StopTimer = 3;
    public float NextScene = 20;
    public GameObject firstObject;
    public GameObject Object;
    public GameObject Plane1;
    public GameObject Plane2;

    private void Awake()
    {
        firstObject.SetActive(true);
        Object.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        StartTimer -= Time.deltaTime;
        NextScene -= Time.deltaTime;
        if(StartTimer <= StopTimer) Object.SetActive(true);
        if (StartTimer <= 0) { firstObject.SetActive(false); StartTimer = 0; }
        if (NextScene <= 5) { Plane1.transform.Translate(Vector3.forward * 3 * Time.deltaTime); Plane2.transform.Translate(Vector3.back * 3 * Time.deltaTime); }
        if (NextScene <= 0) SceneManager.LoadScene("Age100");
    }
}
