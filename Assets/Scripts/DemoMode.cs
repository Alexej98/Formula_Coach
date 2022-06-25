using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DemoMode : MonoBehaviour
{ 
    [SerializeField] Button forwardButton;
    [SerializeField] Button backButton;

    public static bool demoSceneLoaded = false;
    private bool coroutineStarted = false;

    GameObject radioButton;
    GameObject confirmButton;
    GameObject pitLimiterButton;
    GameObject drinkButton;
    GameObject neutralButton;
    GameObject drsButton;
    GameObject button1;
    GameObject button10;
    GameObject middleRotate;
    GameObject car;
    GameObject rearWing;
    GameObject steeringWheel;
    GameObject f1;
    Button quitButton;

    GameObject racingDisplay;
    GameObject testingDisplay;

    private LayerMask layerMask;

    public Material[] materialOriginals;
    public Material[] materialCopies;
    public Material[][] materialOriginalsList = new Material[3][];
    public GameObject[] carObjects;
    public int selectedObject;

    [SerializeField] AudioSource audioSource;
    [SerializeField] TextMeshProUGUI uiText;
    [SerializeField] TextMeshProUGUI uiTextSmall;
    [SerializeField] TextMeshProUGUI helpText;
    [SerializeField] TextMeshProUGUI errorHelpText;
    [SerializeField] TextMeshProUGUI radioOnOff;
    [SerializeField] TextMeshProUGUI pitConfirmed;
    [SerializeField] TextMeshProUGUI pitLimited;
    [SerializeField] TextMeshProUGUI neutral;
    [SerializeField] TextMeshProUGUI drinkActive;
    [SerializeField] TextMeshProUGUI drsActive;
    [SerializeField] public GameObject[] objectsInOrder = null;

    [SerializeField] AudioClip slurp;
    [SerializeField] AudioClip buttonClick;

    public Animator animator;
    public Animator cameraAnimator;
    public Animator rearWingAnimator;

    [SerializeField] private Material highlightMaterial = null;
    private GameObject selectedGameObject;
    private Material selectedGameObjectMaterial;
    public GameObject SelectedGameObject => selectedGameObject;

    public int nextIndex = 0;
    public int buttonIndex = 0;

    public static int helpCounter = 0;
    public static int errorCounter = 0;

    Scene scene;

    void Start()
    {
        demoSceneLoaded = true;

        scene = SceneManager.GetActiveScene();
        radioButton = GameObject.FindGameObjectWithTag("RadioButton");
        confirmButton = GameObject.FindGameObjectWithTag("ConfirmButton");
        pitLimiterButton = GameObject.FindGameObjectWithTag("PitLimiterButton");
        drinkButton = GameObject.FindGameObjectWithTag("DrinkButton");
        neutralButton = GameObject.FindGameObjectWithTag("NeutralButton");
        drsButton = GameObject.FindGameObjectWithTag("DRSButton");
        quitButton = GameObject.FindGameObjectWithTag("QuitButtonDemo").GetComponent<Button>();
        button1 = GameObject.FindGameObjectWithTag("+1Button");
        quitButton.onClick.AddListener(QuitButtonClicked);
        button10 = GameObject.FindGameObjectWithTag("-10Button");
        middleRotate = GameObject.FindGameObjectWithTag("MiddleRotate");
        rearWing = GameObject.FindGameObjectWithTag("RearWingPivot");
        car = GameObject.FindGameObjectWithTag("Car");
        steeringWheel = GameObject.FindGameObjectWithTag("SteeringWheel");
        f1 = GameObject.FindGameObjectWithTag("F1");

        racingDisplay = GameObject.FindGameObjectWithTag("RacingDisplay");
        testingDisplay = GameObject.FindGameObjectWithTag("TestingDisplay");
        racingDisplay.SetActive(false);
        testingDisplay.SetActive(false);

        animator = Camera.main.GetComponent<Animator>();
        cameraAnimator = Camera.main.GetComponent<Animator>();
        rearWingAnimator = rearWing.GetComponent<Animator>();

        layerMask = LayerMask.GetMask("Selectable");

        foreach (Collider gbjCollider in f1.GetComponentsInChildren<Collider>())
        {
            gbjCollider.enabled = false;
        }

        backButton.onClick.AddListener(() =>
        {
            cameraAnimator.enabled = true;
            animator.enabled = true;
            if (nextIndex > 0)
            {
                StopAnimation();
                nextIndex--;
            }
            LoopAnimation();
        });

        forwardButton.onClick.AddListener(() =>
        {
            cameraAnimator.enabled = true;
            animator.enabled = true;
            if (nextIndex < 11)
            {
                StopAnimation();
                nextIndex++;
            }
            LoopAnimation();
        });

        LoopAnimation();
    }

    void LoopAnimation()
    {
        Debug.Log(nextIndex);
        ChangeStepText();
        if (nextIndex != 0)
        { 
            animator = objectsInOrder[nextIndex].GetComponent<Animator>();
        }

        if (nextIndex == 0)
        {
            cameraAnimator.Play("DemoCameraAnimationToSeat");
        }
        else if (nextIndex == 1)
        {
            animator.Play("DemoWheelAnimation");
            cameraAnimator.Play("DemoCameraAnimationToWheel");
        }
        else if (nextIndex == 2)
        {
            cameraAnimator.Play("DemoCameraToMiddleRotate");
            animator.Play("DemoMiddleRotate");            
        }
        else if (nextIndex == 3)
        {
            cameraAnimator.Play("CameraAnimationOneButton");
            animator.Play("Demo+1Button");
        }
        else if (nextIndex == 4)
        {
            cameraAnimator.Play("CameraAnimationRadioButton");
            animator.Play("DemoRadioButton");
            audioSource.Play();
        }
        else if (nextIndex == 4)
        {
            cameraAnimator.Play("CameraAnimationRadioButton");
            animator.Play("DemoRadioButton");
        }
    }

    void StopAnimation()
    {
        if(nextIndex == 1)
        {
            animator.Play("DemoWheelAnimationEnd");
        }
        else if (nextIndex == 2)
        {
            animator.Play("DemoMiddleRotateEnd");
        }
        else
        {
            animator.Play("New State");
        }

        if (nextIndex == 4)
        {
            audioSource.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (nextIndex == 0)
        {
            backButton.interactable = false;
        }
        else 
        {
            backButton.interactable = true;
        }

        if (nextIndex == 10)
        {
            forwardButton.interactable = false;
        }
        else
        {
            forwardButton.interactable = true;
        }
    }

    public IEnumerator waitForSomeTime()
    {
        Debug.Log(nextIndex);
        coroutineStarted = true;
        cameraAnimator.SetFloat("speed", 1.0f);
        animator.SetFloat("speed", 1.0f);
        
        if (nextIndex != 0)
        {
            animator = objectsInOrder[nextIndex].GetComponent<Animator>();
        }
        
        if (objectsInOrder[nextIndex] == radioButton)
        {
            audioSource.Play();
        }
        else if (objectsInOrder[nextIndex] == drinkButton)
        {
            audioSource.clip = slurp;
            audioSource.Play();
        }

        if (nextIndex == 0)
        {
            cameraAnimator.SetBool("pressed", true);
            yield return new WaitForSeconds(cameraAnimator.runtimeAnimatorController.animationClips[0].length);
        }
        else if(nextIndex != 2)
        {
            animator.SetBool("pressed", true);
        }
        if(nextIndex == 1)
        {
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
            racingDisplay.SetActive(true);
            cameraAnimator.SetBool("closer", true);
            yield return new WaitForSeconds(1f);
        }
        if (nextIndex == 2)
        {
            cameraAnimator.SetBool("middle", true);
            yield return new WaitForSeconds(1.3f);
            animator.SetBool("pressed", true);
            yield return new WaitForSeconds(3.7f);
        }
        if (nextIndex == 3)
        {
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        }
        if (nextIndex == 4)
        {
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        }
        if (nextIndex == 5)
        {
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        }
        if (nextIndex == 6)
        {
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        }
        if (nextIndex == 7)
        {
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        }
        if (nextIndex == 8)
        {
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        }
        if (nextIndex == 9)
        {
            cameraAnimator.SetBool("drs", true);
            rearWingAnimator.SetBool("open", true);
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        }
        if (nextIndex == 10)
        {
            cameraAnimator.SetBool("drs", false);
            rearWingAnimator.SetBool("open", false);
            animator.SetBool("pressedAgain", true);
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length);
        }

        ChangeWheelText();

        if (nextIndex == 3)
        {
            racingDisplay.SetActive(false);
            testingDisplay.SetActive(true);
        }
        nextIndex++;
        ChangeStepText();
        if (nextIndex == objectsInOrder.Length)
        {
            SceneManager.LoadScene("F1_Demonstrator_PostDemoScreen");
        }

        animator.Update(0f);
        cameraAnimator.Update(0f);
        coroutineStarted = false;

    }

    void ChangeStepText()
    {
        if(nextIndex != objectsInOrder.Length)
        {
            if (nextIndex == 1)
            {
                uiText.text = "Step 2: Press the Steering Wheel!";
                uiTextSmall.text = "";
            }
            else if (nextIndex == 2)
            {
                uiText.text = "Step 3: Press the middle rotation knob!";
                uiTextSmall.text = "Here you can activate the display mode to change between the display views";
            }
            else if (nextIndex == 6)
            {
                uiText.text = "Step 7: Press the pit limiter button!";
            }
            else if (nextIndex == 9 || nextIndex == 10)
            {
                uiText.text = "Step " + (nextIndex + 1) + ": Press the DRS-button!";
            }
            else
            {
                uiText.text = "Step " + (nextIndex + 1) + ": Press the " + objectsInOrder[nextIndex].name + "!";
            }
            if (nextIndex == 3)
            {
                uiTextSmall.text = "While checking the individual information of the car, this button skips through the views on the display";
            }
            if (nextIndex == 4)
            {
                uiTextSmall.text = "By pressing it, you can communicate with your team engineer";
            }
            else if (nextIndex == 5)
            {
                uiTextSmall.text = "This button is used when a driver has to confirm to box in the current lap";
            }
            else if (nextIndex == 6)
            {
                uiTextSmall.text = "When driving into the pit lane, the maximum speed of the car is 60km/h. This button applies the limit";
            }
            else if (nextIndex == 7)
            {
                uiTextSmall.text = "When the car is standing still or the pit lane crew is changing tyres, this button deselects the current gear";
            }
            if (nextIndex == 8)
            {
                uiTextSmall.text = "This button activates the water supply";
            }
            else if (nextIndex == 8)
            {
                uiTextSmall.text = "It's useful to skip multiple views at once or change car settings";
            }
            else if (nextIndex == 9)
            {
                uiTextSmall.text = "One of the most important buttons. It opens the rear wing and makes overtaking easier";
            }
            else if (nextIndex == 10)
            {
                uiTextSmall.text = "By pressing the button again, or braking, the rear wing gets closed";
            }
        }
    }

    void ChangeWheelText()
    {
        if (nextIndex == 4)
        {
            radioOnOff.text = "TESTED";
            radioOnOff.color = Color.green;
        }
        else if (nextIndex == 5)
        {
            pitConfirmed.text = "TESTED";
            pitConfirmed.color = Color.green;
        }
        else if (nextIndex == 6)
        {
            pitLimited.text = "TESTED";
            pitLimited.color = Color.green;
        }
        else if (nextIndex == 7)
        {
            neutral.text = "TESTED";
            neutral.color = Color.green;
        }
        else if (nextIndex == 8)
        {
            drinkActive.text = "TESTED";
            drinkActive.color = Color.green;
        }
        else if (nextIndex == 9)
        {
            drsActive.text = "TESTED 1/2";
            drsActive.color = Color.yellow;
        }
        else if (nextIndex == 10)
        {
            drsActive.text = "TESTED 2/2";
            drsActive.color = Color.green;
        }
    }

    void QuitButtonClicked()
    {
        audioSource.clip = buttonClick;
        audioSource.Play();
        SceneManager.LoadScene("F1_Demonstrator_EditedMenu");
    }
}
