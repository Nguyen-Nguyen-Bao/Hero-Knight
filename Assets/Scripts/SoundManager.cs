using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider musicslider;
    public Slider soundFxslider;
    public AudioMixer audioMixer;
    public AudioSource peacefulmusic;
    public AudioSource sworncross;
    // Start is called before the first frame update
    void Music01()
    {
        //peacefulmusic.volume = musicslider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(musicslider.value) * 20);
    }
    void Music02()
    {
        //sworncross.volume = soundFxslider.value;
        audioMixer.SetFloat("SoundFx", Mathf.Log10(soundFxslider.value) * 20);
    }
    public void Music(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume)*20);
    }
    public void SoundFx(float volume)
    {
        audioMixer.SetFloat("SoundFx", Mathf.Log10(volume)*20);
    }
    void Start()
    {
        Music01();
        Music02();
    }

    // Update is called once per frame
    void Update()
    {
  
    }
}
