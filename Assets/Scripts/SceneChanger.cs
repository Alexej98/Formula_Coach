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
    GameObject title;
    GameObject controls;
    Animator animator;

    private void Start()
    {
        animator = Camera.main.GetComponent<Animator>();
        tutorialButton = GameObject.FindGameObjectWithTag("TutorialButton");
        demoButton = GameObject.FindGameObjectWithTag("DemoButton");
        controlButton = GameObject.FindGameObjectWithTag("ControlButton");
        quitButton = GameObject.FindGameObjectWithTag("QuitButton");
        backButton = GameObject.FindGameObjectWithTag("BackButton");
        title = GameObject.FindGameObjectWithTag("Title");
        controls = GameObject.FindGameObjectWithTag("Controls");
    }

    //load target scene
    public void LoadScene(string name)
    {
        StartCoroutine(LoadSceneRoutine(name));
    }

    //destroy the menu buttons and start transition to start position
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
        Destroy(title);
        animator.SetBool("start", true);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        SceneManager.LoadScene(name);
    }

    //load scene from post game screen
    public void LoadSceneFromEnd(string name)
    {
        buttonAudioSource.clip = audioClips[0];
        buttonAudioSource.Play();
        SceneManager.LoadScene(name);
    }

    //animation to the controls text
    public void CameraAnimation()
    {
        StartCoroutine("ControlsAnimation");
    }

    //disable menu buttons and start transition
    IEnumerator ControlsAnimation()
    {
        buttonAudioSource.clip = audioClips[0];
        buttonAudioSource.Play();
        animator.SetBool("pressed", true);
        tutorialButton.SetActive(false);
        demoButton.SetActive(false);
        controlButton.SetActive(false);
        quitButton.SetActive(false);
        backButton.SetActive(false);
        title.SetActive(false);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        controls.transform.position = new Vector3(controls.transform.position.x, 0.06f, controls.transform.position.z);
        backButton.SetActive(true);
    }

    //animation back to the menu buttons
    public void CameraAnimationBack()
    {
        StartCoroutine("ControlsAnimationBack");
    }

    //start transition, enable menu buttons after transition is finished
    IEnumerator ControlsAnimationBack()
    {
        buttonAudioSource.clip = audioClips[0];
        buttonAudioSource.Play();
        animator.SetBool("pressed", false);
        backButton.SetActive(false);
        controls.transform.position = new Vector3(controls.transform.position.x, 0f, controls.transform.position.z);
        yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        tutorialButton.SetActive(true);
        demoButton.SetActive(true);
        controlButton.SetActive(true);
        quitButton.SetActive(true);
        title.SetActive(true);
    }

    //button hover sound
    public void HoverSound()
    {
        buttonAudioSource.clip = audioClips[1];
        buttonAudioSource.Play();
    }

    //quit the application
    public void Quit()
    {
        buttonAudioSource.clip = audioClips[0];
        buttonAudioSource.Play();
        Application.Quit();
    }
}
