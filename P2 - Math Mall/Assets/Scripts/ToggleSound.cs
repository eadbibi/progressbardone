using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSound : MonoBehaviour
{
    [SerializeField] private bool _toggleMusic;

    public void Toggle()
    {
        if (_toggleMusic) SoundManager.Instance.ToggleMusic();
    }
}
