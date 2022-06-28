using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonPresser : MonoBehaviour
{
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
    Button helpButton;
    Button quitButton;
    [SerializeField] Button infoButton;

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

    public static int nextIndex = 0;
    private bool[] helpButtonPressed = { false, false, false, false, false, false, false, false, false, false, false };

    public static int helpCounter = 0;
    public static int errorCounter = 0;
    public static int finalTips = 0;
    public static int finalErrors = 0;

    Scene scene;

    // Start is called before the first frame update
    void Start()
    {
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
        cameraAnimator.enabled = false;
        rearWingAnimator = rearWing.GetComponent<Animator>();

        layerMask = LayerMask.GetMask("Selectable");

        if (scene.name == "F1_Demonstrator_TutorialMode")
        {
            helpButton = GameObject.FindGameObjectWithTag("HelpButton").GetComponent<Button>();
            helpButton.onClick.AddListener(HelpButtonClicked);
        }

        uiTextSmall.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelection();
        if (scene.name == "F1_Demonstrator_TutorialMode")
        {
            errorHelpText.text = errorCounter + "\n" + helpCounter;
        }
        if (Input.GetMouseButtonDown(0) && nextIndex >= 0 && nextIndex < objectsInOrder.Length)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, layerMask))
            {
                if (hit.collider.gameObject.Equals(objectsInOrder[nextIndex]))
                {
                    StartCoroutine(waitForSomeTime(hit));
                }
                else
                {
                    errorCounter++;
                    helpText.text = "Wrong Order! You have made " + errorCounter + " mistakes";
                }
            }
        }
    }

    IEnumerator waitForSomeTime(RaycastHit hit)
    {
        if (nextIndex != 0)
        {
            animator = objectsInOrder[nextIndex].GetComponent<Animator>();
        }
        if (nextIndex == 0 || nextIndex == 1)
        {
            hit.collider.gameObject.GetComponent<Collider>().enabled = false;
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
            cameraAnimator.enabled = true;
            cameraAnimator.SetBool("pressed", true);
        }
        else if(nextIndex == 1)
        {
            animator.SetBool("pressed", true);
            cameraAnimator.enabled = true;
            cameraAnimator.SetBool("closer", true);
        }
        else
        {
            animator.SetBool("pressed", true);
        }
        if (nextIndex == 2)
        {
            cameraAnimator.enabled = true;
            cameraAnimator.SetBool("middle", true);
        }
        if (nextIndex == 9)
        {
            cameraAnimator.enabled = true;
            cameraAnimator.SetBool("drs", true);
            rearWingAnimator.SetBool("open", true);
        }
        if (nextIndex == 10)
        {
            cameraAnimator.enabled = true;
            cameraAnimator.SetBool("drs", false);
            rearWingAnimator.SetBool("open", false);
            animator.SetBool("pressedAgain", true);
        }
        foreach (Collider gbjCollider in f1.GetComponentsInChildren<Collider>())
        {
            gbjCollider.enabled = false;
        }
        ChangeWheelText();
        if (scene.name == "F1_Demonstrator_TutorialMode")
        {
            helpButton.interactable = false;
        }
        infoButton.interactable = false;
        if (nextIndex == 2)
        {
            yield return new WaitForSeconds(cameraAnimator.runtimeAnimatorController.animationClips[0].length + 1.0f);
        }
        else
        {
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length + 1.0f);
        }
        if (nextIndex == 1)
        {
            racingDisplay.SetActive(true);
            yield return new WaitForSeconds(animator.runtimeAnimatorController.animationClips[0].length - 1.5f);
        }
        if (scene.name == "F1_Demonstrator_TutorialMode")
        {
            helpButton.interactable = true;
        }
        infoButton.interactable = true;
        if (nextIndex == 3)
        {
            racingDisplay.SetActive(false);
            testingDisplay.SetActive(true);
        }
        for (int childIndex = 0; childIndex < f1.transform.childCount; childIndex++)
        {
            var child = f1.transform.GetChild(childIndex);
            try
            {
                if (nextIndex == 0)
                {
                    if (!child.tag.Equals("Car"))
                    {
                        child.GetComponent<Collider>().enabled = true;
                    }
                }
                else
                {
                    for (int wheelChildIndex = 0; wheelChildIndex < steeringWheel.transform.childCount; wheelChildIndex++)
                    {
                        var wheelChild = steeringWheel.transform.GetChild(wheelChildIndex);
                        try
                        {
                            wheelChild.GetComponent<Collider>().enabled = true;
                        }
                        catch
                        {
                            try
                            {
                                wheelChild.GetChild(0).GetComponent<Collider>().enabled = true;
                            }
                            catch
                            {
                                continue;
                            };
                        }
                    }
                }
            }
            catch
            {
                continue;
            }

        }
        nextIndex++;
        if (scene.name == "F1_Demonstrator_TutorialMode")
        {
            helpText.text = "";
        }
        if (nextIndex == objectsInOrder.Length)
        {
            finalTips = helpCounter;
            finalErrors = errorCounter;
            helpCounter = 0;
            errorCounter = 0;
            SceneManager.LoadScene("F1_Demonstrator_PostDemoScreen");
        }
        else
        {
            if (nextIndex == 1)
            {
                uiText.text = "Step 2: Press the Steering Wheel!";
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
                uiText.text = "Step " + (nextIndex + 1) + ": Press the DRS button";
            }
            else
            {
                uiText.text = "Step " + (nextIndex + 1) + ": Press the " + objectsInOrder[nextIndex].name + "!";
            }
            if (nextIndex == 3)
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
        cameraAnimator.enabled = false;
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
        helpCounter = 0;
        errorCounter = 0;
        SceneManager.LoadScene("F1_Demonstrator_EditedMenu");
    }

    void HelpButtonClicked()
    {
        switch (nextIndex)
        {
            case 0:
                {
                    helpButton.interactable = true;
                    helpText.text = "It's right in front of you!";
                    if (!helpButtonPressed[0])
                    {
                        helpCounter++;
                        helpButtonPressed[0] = true;
                        helpButton.interactable = false;
                    }
                    break;
                }
            case 1:
                {
                    helpButton.interactable = true;
                    helpText.text = "It's on the car!";
                    if (!helpButtonPressed[1])
                    {
                        helpCounter++;
                        helpButtonPressed[1] = true;
                        helpButton.interactable = false;
                    }
                    break;
                }
            case 2:
                {
                    helpButton.interactable = true;
                    helpText.text = "It's the big rotation knob under the display";
                    if (!helpButtonPressed[2])
                    {
                        helpCounter++;
                        helpButtonPressed[2] = true;
                        helpButton.interactable = false;
                    }
                    break;
                }

            case 3:
                {
                    helpButton.interactable = true;
                    helpText.text = "It's the one to the left of the Pit Limiter Button";
                    if (!helpButtonPressed[3])
                    {
                        helpCounter++;
                        helpButtonPressed[3] = true;
                        helpButton.interactable = false;
                    }
                    break;
                }
            case 4:
                {
                    helpButton.interactable = true;
                    helpText.text = "It's just left from the main display!";
                    if (!helpButtonPressed[4])
                    {
                        helpCounter++;
                        helpButtonPressed[4] = true;
                        helpButton.interactable = false;
                    }
                    break;
                }
            case 5:
                {
                    helpButton.interactable = true;
                    helpText.text = "It's the furthest button to the left";
                    if (!helpButtonPressed[5])
                    {
                        helpCounter++;
                        helpButtonPressed[5] = true;
                        helpButton.interactable = false;
                    }
                    break;
                }
            case 6:
                {
                    helpButton.interactable = true;
                    helpText.text = "It's the big button in the upper right corner";
                    if (!helpButtonPressed[6])
                    {
                        helpCounter++;
                        helpButtonPressed[6] = true;
                        helpButton.interactable = false;
                    }
                    break;
                }
            case 7:
                {
                    helpButton.interactable = true;
                    helpText.text = "It's the big button in the upper left corner";
                    if (!helpButtonPressed[7])
                    {
                        helpCounter++;
                        helpButtonPressed[7] = true;
                        helpButton.interactable = false;
                    }
                    break;
                }
            case 8:
                {
                    helpButton.interactable = true;
                    helpText.text = "It's next to the bottom left corner of the display";
                    if (!helpButtonPressed[8])
                    {
                        helpCounter++;
                        helpButtonPressed[8] = true;
                        helpButton.interactable = false;
                    }
                    break;
                }

            case 9:
            case 10:
                {
                    helpButton.interactable = true;
                    helpText.text = "It's right from the rotation knob";
                    if (!helpButtonPressed[9])
                    {
                        helpCounter++;
                        helpButtonPressed[9] = true;
                        helpButton.interactable = false;
                    }
                    break;
                }
        }
    }

    private void UpdateSelection()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, layerMask))
        {
            if (selectedGameObject == null)
            {
                Select(hit.transform.gameObject);
            }
            else if (!selectedGameObject.Equals(hit.transform.gameObject))
            {
                DeSelect();
                Select(hit.transform.gameObject);
            }
        }
        else if (selectedGameObject != null)
        {
            DeSelect();
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.direction * 20, Color.red);
        }
    }
    private void Select(GameObject selectedGameObject)
    {
        this.selectedGameObject = selectedGameObject;
        var renderer = selectedGameObject.GetComponent<Renderer>();
        selectedGameObjectMaterial = renderer.material;
        if (nextIndex == 0)
        {
            if (selectedGameObject.tag.Equals("Car"))
            {
                selectedObject = 0;
                carObjects = GameObject.FindGameObjectsWithTag("Car");
                foreach (GameObject gameObj in carObjects)
                {
                    renderer = gameObj.GetComponent<Renderer>();
                    materialOriginals = renderer.sharedMaterials;
                    materialCopies = renderer.sharedMaterials;
                    materialOriginalsList[selectedObject] = materialOriginals;
                    for (int i = 0; i < renderer.sharedMaterials.Length; i++)
                    {
                        materialCopies[i] = highlightMaterial;
                    }
                    renderer.sharedMaterials = materialCopies;
                    selectedObject++;
                }
            }
        }
        renderer.material = highlightMaterial;

    }

    private void DeSelect()
    {
        if (nextIndex <= 1)
        {
            selectedObject = 0;
            foreach (GameObject gameObj in carObjects)
            {
                var renderer = gameObj.GetComponent<Renderer>();
                renderer.sharedMaterials = materialOriginalsList[selectedObject];
                selectedObject++;
            }

        }
        selectedGameObject.GetComponent<Renderer>().material = selectedGameObjectMaterial;
        selectedGameObject = null;
    }
    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}