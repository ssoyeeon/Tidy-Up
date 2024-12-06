using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicMasterSlider;

    //�����̴� MinValue 0.001 ���� ������ Log10 ������ �Ǿ��ֱ� ������

    private void Awake()
    {   //����Ÿ �����̴��� ���� ����� �� �����ʸ� ���ؼ� �Լ��� ���� �����Ѵ�.
        musicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }
}
