using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DemoMode : MonoBehaviour
{
    [SerializeField] Button forwardButton;
    [SerializeField] Button backButton;
    private int nextIndex;
    public static bool demoSceneLoaded = false;

    void Start()
    {
        demoSceneLoaded = true;   
    }
    void Update()
    {
        nextIndex = ButtonPresser.nextIndex;
        if (nextIndex == 0)
        {
            backButton.interactable = false;
        }
        if (nextIndex == 1)
        {
            backButton.interactable = true;
        }
        if(nextIndex == 10)
        {
            forwardButton.interactable = false;
        }

    }
}
