using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioMixer _masterMixer;
    [SerializeField] private AudioSource _bgmSource;

    //���� ����
    //ȿ���� ���
    //������� ���
    private void Start()
    {
        instance = this;
    }
    public void SetVolume(string mixer, float volume) // 0 ~ 1 //UI�� �����ؼ� ���
    {
        _masterMixer.SetFloat(mixer, Mathf.Lerp(-40, 20, volume));
    }

    public void PlayOneShot(AudioSource source, AudioClip clip)
    {
        source.PlayOneShot(clip);
    }

    public void Bgm(AudioClip clip)
    {
        _bgmSource.clip = clip;
        _bgmSource.Play();
    }
}

/*public class test : MonoBehaviour
{
    SoundManager sound = new SoundManager();

    AudioSource source = null;
    AudioClip clip = null;

    private void Start()
    {
        sound.PlayOneShot(source, clip);
    }
}*/
