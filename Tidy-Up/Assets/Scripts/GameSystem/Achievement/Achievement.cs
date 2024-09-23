using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Achievement        //업적 클래스 선언 ( 모노 비해이비어 없애기 ) 
{
    public string name;             //업적 이름
    public string description;      //업적 설명
    public bool isUnlocked;         //잠김 설정
    public int currentProgress;     //진행 상태
    public int goal;                //완료 상태

    public Achievement(string name, string description, int goal)
    {
        this.name = name;                       //Achievement 클래스가 생성 될 때 이름을 인수로 받아서 설정
        this.description = description;         //Achievement 클래스가 생성 될 때 설명을 인수로 받아서 설정
        this.isUnlocked = false;                //Achievement 클래스가 생성 될 때 false
        this.currentProgress = 0;               //Achievement 클래스가 생성 될 때 0
        this.goal = goal;                       //Achievement 클래스가 생성 될 때 완료 상태
    }

    public void AddProgress(int amount)         //업적 진행도 함수
    {
        if(!isUnlocked)                         //잠겨있지 않다면
        {
            currentProgress += amount;
            if(currentProgress >= goal)         //진행도보다 완료 숫자가 더 높을 때 
            {
                isUnlocked = true;
                OnAchievmentUnlocked();         //어적 달성시 Debug.Log 로 출력
            }
        }
    }

    protected virtual void OnAchievmentUnlocked()
    {
        Debug.Log($"업적 달성 : {name}");               //%표시가 들어있는 String에서 {} 변수 값을 사용 할 수 있다.
    }
}
