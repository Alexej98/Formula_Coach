using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] AudioSource buttonAudioSource;
    [SerializeField] AudioSource backgroundAudioSource;
    public void LoadScene(string name)
    {
        if (!backgroundAudioSource.Equals(null))
        {
            backgroundAudioSource.Stop();
        }
        buttonAudioSource.clip = audioClips[0];
        buttonAudioSource.Play();
        SceneManager.LoadScene(name);
    }

    public void HoverSound()
    {
        buttonAudioSource.clip = audioClips[1];
        buttonAudioSource.Play();
    }

    public void Quit()
    {
        buttonAudioSource.clip = audioClips[0];
        buttonAudioSource.Play();
        Application.Quit();
    }
}
