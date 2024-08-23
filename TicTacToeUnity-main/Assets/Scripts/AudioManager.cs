using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource ClickAudio;
    [SerializeField] AudioSource MoveAudio;
    public AudioClip moveAudio1;
    public AudioClip moveAudio2;
    public AudioClip clickAudio1;
    public AudioClip clickAudio2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void PlayMoveAudio(AudioClip clip)
    {
        MoveAudio.PlayOneShot(clip);
    }

    public void PlayClickedAudio(AudioClip clip)
    {
        ClickAudio.PlayOneShot(clip);
    }
}
