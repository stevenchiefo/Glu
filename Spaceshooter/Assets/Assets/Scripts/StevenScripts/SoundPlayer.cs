using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : PoolableObject
{
    private AudioSource m_SoundPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        m_SoundPlayer = GetComponent<AudioSource>();
    }

    

    public void PlayAudio()
    {
        m_SoundPlayer.Play();
        StartCoroutine(LifeTimeTimer((int)m_SoundPlayer.clip.length));
    }


    public void SetAudio(AudioClip audioClip)
    {
        if(m_SoundPlayer == null)
        {
            m_SoundPlayer = GetComponent<AudioSource>();
        }
        m_SoundPlayer.clip = audioClip;
    }
}
