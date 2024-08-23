using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource StartAudio;
    [SerializeField] AudioSource ClickAudio;
    [SerializeField] AudioSource MoveAudio;
    public AudioClip moveAudio1;
    public AudioClip moveAudio2;
    public AudioClip clickAudio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayMoveAudio(AudioClip clip)
    {
        MoveAudio.PlayOneShot(clip);
    }

    public void PlayStartAudio()
    {
        StartAudio.Play();
    }

    public void PlayClickedAudio(AudioClip clip)
    {
        ClickAudio.PlayOneShot(clip);
    }
}
