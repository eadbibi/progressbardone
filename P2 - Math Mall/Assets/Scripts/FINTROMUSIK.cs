using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FINTROMUSIK : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource;
    private float fadeTime = 10f; // time to fade in seconds

    void Start()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = 0f; // set volume to 0%
        audioSource.Play();

        StartCoroutine(FadeIn());

        IEnumerator FadeIn()
        {
            float currentTime = 0f;

            while (currentTime < fadeTime)
            {
                currentTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0f, 0.3f, currentTime / fadeTime);
                yield return null;
            }
            audioSource.volume = 0.3f; // set volume to 100%
        }
    }
}