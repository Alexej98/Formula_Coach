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
    [SerializeField] AudioClip box;
    [SerializeField] AudioClip buttonClick;

    public Animator animator;
    public Animator cameraAnimator;
    public Animator rearWingAnimator;

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
        CameraController.rotationEnabled = false;

        scene = SceneManager.GetActiveScene();
        radioButton = GameObject.FindGameObjectWithTag("RadioButton");
        confirmButton = GameObject.FindGameObjectWithTag("ConfirmButton");
        pitLimiterButton = GameObject.FindGameObjectWithTag("PitLimiterButton");
        drinkButton = GameObject.FindGameObjectWithTag("DrinkButton");
        neutralButton = GameObject.FindGameObjectWithTag("NeutralButton");
        drsButton = GameObject.FindGameObjectWithTag("DRSButton");
        quitButton = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
        button1 = GameObject.FindGameObjectWithTag("+1Button");
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

        //disable collider
        foreach (Collider gbjCollider in f1.GetComponentsInChildren<Collider>())
        {
            gbjCollider.enabled = false;
        }

        //if backButton is clicked...
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

        //if forwardButton is clicked...
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

    //loop the current animation
    void LoopAnimation()
    {
        ChangeStepText();
        if (nextIndex != 0)
        { 
            animator = objectsInOrder[nextIndex].GetComponent<Animator>();
        }
        if (nextIndex == 0)
        {
            animator.Play("New State");
            racingDisplay.SetActive(false);
            cameraAnimator.Play("DemoCameraAnimationToSeat");
        }
        else if (nextIndex == 1)
        {
            racingDisplay.SetActive(true);
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
            racingDisplay.SetActive(false);
            testingDisplay.SetActive(true);
            cameraAnimator.Play("DemoCameraAnimationOneButton");
            animator.Play("Demo+1Button");
        }
        else if (nextIndex == 4)
        {
            cameraAnimator.Play("DemoCameraAnimationRadioButton");
            animator.Play("DemoRadioButton");
            audioSource.clip = box;
            audioSource.Play();
        }
        else if (nextIndex == 5)
        {
            cameraAnimator.Play("DemoCameraAnimationConfirmButton");
            animator.Play("DemoConfirmButton");
        }
        else if (nextIndex == 6)
        {
            cameraAnimator.Play("DemoCameraAnimationPitLimiterButton");
            animator.Play("DemoPitLimiterButton");
        }
        else if (nextIndex == 7)
        {
            cameraAnimator.Play("DemoCameraAnimationNeutralButton");
            animator.Play("DemoNeutralButton");
        }
        else if (nextIndex == 8)
        {
            cameraAnimator.Play("DemoCameraAnimationDrinkButton");
            animator.Play("DemoDrinkButton");
            audioSource.clip = slurp;
            audioSource.Play();
        }
        else if (nextIndex == 9)
        {
            cameraAnimator.Play("DemoCameraAnimationDRSButton");
            animator.Play("DemoDRSButton");
            rearWingAnimator.Play("DemoRearWingAnimation");
        }
        else if (nextIndex == 10)
        {
            cameraAnimator.Play("DemoCameraAnimationDRSButtonAgain");
            animator.Play("DemoDRSButtonAgain");
            rearWingAnimator.Play("DemoRearWingAnimationBack");
        }
        ChangeWheelText();
    }

    //stop the current animation
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
        else if (nextIndex == 9)
        {
            rearWingAnimator.Play("New State");
            animator.Play("New State");
        }
        else
        {
            animator.Play("New State");
        }

        if (nextIndex == 4 || nextIndex == 8)
        {
            audioSource.Stop();
        }
    }

    //change the UIText
    void ChangeStepText()
    {
        if(nextIndex != objectsInOrder.Length)
        {
            if (nextIndex == 0)
            {
                uiText.text = "Step " + (nextIndex + 1) + "/" + (objectsInOrder.Length) + ": Press the car to get in!";
            }
            else if (nextIndex == 9)
            {
                uiText.text = "Step " + (nextIndex + 1) + "/" + (objectsInOrder.Length) + ": Press the DRS button!";
            }
            else if (nextIndex == 10)
            {
                uiText.text = "Step " + (nextIndex + 1) + "/" + (objectsInOrder.Length) + ": Press the DRS button again!";
            }
            else
            {
                uiText.text = "Step " + (nextIndex + 1) + "/" + (objectsInOrder.Length) + ": Press the " + objectsInOrder[nextIndex].name + "!";
            }
            if (nextIndex == 2)
            {
                uiTextSmall.text = "Here you can activate the display mode to change between the display views";
            }
            else if (nextIndex == 3)
            {
                uiTextSmall.text = "While checking the individual information of the car, this button skips through the views on the display";
            }
            else if (nextIndex == 4)
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
            else if (nextIndex == 8)
            {
                uiTextSmall.text = "This button activates the water supply";
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

    //change the text on the wheel display
    void ChangeWheelText()
    {
        if(nextIndex == 3)
        {
            radioOnOff.text = "NOT TESTED";
            radioOnOff.color = Color.red;
        }
        else if (nextIndex == 4)
        {
            radioOnOff.text = "TESTED";
            radioOnOff.color = Color.green;
            pitConfirmed.text = "NOT TESTED";
            pitConfirmed.color = Color.red;
        }
        else if (nextIndex == 5)
        {
            pitConfirmed.text = "TESTED";
            pitConfirmed.color = Color.green;
            pitLimited.text = "NOT TESTED";
            pitLimited.color = Color.red;
        }
        else if (nextIndex == 6)
        {
            pitLimited.text = "TESTED";
            pitLimited.color = Color.green;
            neutral.text = "NOT TESTED";
            neutral.color = Color.red;
        }
        else if (nextIndex == 7)
        {
            neutral.text = "TESTED";
            neutral.color = Color.green;
            drinkActive.text = "NOT TESTED";
            drinkActive.color = Color.red;
        }
        else if (nextIndex == 8)
        {
            drinkActive.text = "TESTED";
            drinkActive.color = Color.green;
            drsActive.text = "NOT TESTED";
            drsActive.color = Color.red;
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

    //quit current mode and load menu scene
    public void QuitButtonClicked()
    {
        audioSource.clip = buttonClick;
        audioSource.Play();
        SceneManager.LoadScene("F1_Demonstrator_EditedMenu");
    }
}
