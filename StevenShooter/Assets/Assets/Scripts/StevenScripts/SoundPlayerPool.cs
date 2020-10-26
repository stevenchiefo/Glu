using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerPool : MonoBehaviour
{
    public static SoundPlayerPool Instance;

    [SerializeField] private GameObject m_SoundPlayer;
    private ObjectPool m_SoundPlayers;

    private void Awake()
    {
        if(Instance== null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        m_SoundPlayers = gameObject.AddComponent<ObjectPool>();
        m_SoundPlayers.BeginPool(m_SoundPlayer, 5, transform);
    }

    public void PlayAudio(AudioClip audioClip,Vector2 vector2)
    {
        PoolableObject poolableObject = m_SoundPlayers.GetObject();
        SoundPlayer soundPlayer = poolableObject.GetComponent<SoundPlayer>();
        soundPlayer.SetAudio(audioClip);
        soundPlayer.SpawnObject(vector2);
        soundPlayer.PlayAudio();
    }
}
