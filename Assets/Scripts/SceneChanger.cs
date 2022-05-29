using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] AudioClip[] audioClips;
    [SerializeField] AudioSource buttonAudioSource;
    [SerializeField] AudioSource backgroundAudioSource;
    GameObject tutorialButton;
    GameObject demoButton;
    GameObject controlButton;
    GameObject quitButton;
    GameObject backButton;
    Animator animator;

    private void Start()
    {
        animator = Camera.main.GetComponent<Animator>();
        tutorialButton = GameObject.FindGameObjectWithTag("TutorialButton");
        demoButton = GameObject.FindGameObjectWithTag("DemoButton");
        controlButton = GameObject.FindGameObjectWithTag("ControlButton");
        quitButton = GameObject.FindGameObjectWithTag("QuitButton");
        backButton = GameObject.FindGameObjectWithTag("BackButton");
    }

    public void LoadSceneFromEnd(string name)
    {
        buttonAudioSource.clip = audioClips[0];
        buttonAudioSource.Play();
        SceneManager.LoadScene(name);
    }

    public void LoadScene(string name)
    {
        StartCoroutine(LoadSceneRoutine(name));
    }

    IEnumerator LoadSceneRoutine(string name)
    {
        if (!backgroundAudioSource.Equals(null))
        {
            backgroundAudioSource.Stop();
        }
        buttonAudioSource.clip = audioClips[0];
        buttonAudioSource.Play();
        Destroy(tutorialButton);
        Destroy(demoButton);
        Destroy(controlButton);
        Destroy(quitButton);
        Destroy(backButton);
        animator.SetBool("start", true);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length + 1.0f);
        SceneManager.LoadScene(name);
    }

    public void HoverSound()
    {
        buttonAudioSource.clip = audioClips[1];
        buttonAudioSource.Play();
    }

    public void CameraAnimation()
    {
        buttonAudioSource.clip = audioClips[0];
        buttonAudioSource.Play();
        animator.SetBool("pressed", true);
    }

    public void CameraAnimationBack()
    {
        buttonAudioSource.clip = audioClips[0];
        buttonAudioSource.Play();
        animator.SetBool("pressed", false);
    }

    public void Quit()
    {
        buttonAudioSource.clip = audioClips[0];
        buttonAudioSource.Play();
        Application.Quit();
    }
}
