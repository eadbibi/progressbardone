using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource _musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;

    }
    
    public void ToggleMusic()
    {
        _musicSource.mute = !_musicSource.mute;

    }

}
