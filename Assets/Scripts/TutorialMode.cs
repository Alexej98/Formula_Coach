using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialMode : MonoBehaviour
{
    [SerializeField] GameObject rearWing;
    [SerializeField] GameObject steeringWheel;
    [SerializeField] GameObject f1;

    [SerializeField] Button helpButton;

    [SerializeField] GameObject racingDisplay;
    [SerializeField] GameObject testingDisplay;

    private LayerMask layerMask;

    private Material[] materialOriginals;
    private Material[] materialCopies;
    private Material[][] materialOriginalsList = new Material[3][];
    private GameObject[] carObjects;
    private int selectedObject;

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

    private string[] helpTexts =
    {
        "It's right in front of you!",
        "It's on the car!",
        "It's the big rotation knob under the display",
        "It's the one to the left of the Pit Limiter Button",
        "It's just left from the main display!",
        "It's the furthest button to the left",
        "It's the big button in the upper right corner",
        "It's the big button in the upper left corner",
        "It's next to the bottom left corner of the display",
        "It's right from the rotation knob",
        "It's right from the rotation knob"
    };

    private string[] infoTexts =
    {
        "",
        "",
        "Here you can activate the display mode to change between the display views",
        "While checking the individual information of the car, this button skips through the views on the display",
        "By pressing it, you can communicate with your team engineer",
        "This button is used when a driver has to confirm to box in the current lap",
        "When driving into the pit lane, the maximum speed of the car is 60km/h. This button applies the limit",
        "When the car is standing still or the pit lane crew is changing tyres, this button deselects the current gear",
        "This button activates the water supply",
        "One of the most important buttons. It opens the rear wing and makes overtaking easier",
        "By pressing the button again, or braking, the rear wing gets closed"
    };

    private string[] animatorStates =
{
        "pressed",
        "closer",
        "middle",
        "one",
        "radio",
        "confirm",
        "pit",
        "neutral",
        "drink",
        "drs",
        "drsAgain"
    };

    void Start()
    {
        CameraController.rotationEnabled = true;

        racingDisplay.SetActive(false);
        testingDisplay.SetActive(false);

        animator = Camera.main.GetComponent<Animator>();
        cameraAnimator = Camera.main.GetComponent<Animator>();
        cameraAnimator.enabled = false;
        rearWingAnimator = rearWing.GetComponent<Animator>();

        layerMask = LayerMask.GetMask("Selectable");
     
        uiTextSmall.enabled = false;
    }

    void Update()
    {
        UpdateSelection();
        errorHelpText.text = errorCounter + "\n" + helpCounter;
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
                    helpText.text = "Wrong button! Try again";
                }
            }
        }
    }

    //start animation of the current state and change the texts
    IEnumerator waitForSomeTime(RaycastHit hit)
    {
        helpText.text = "";
        CameraController.rotationEnabled = !CameraController.rotationEnabled;
        if (nextIndex != 0)
        {
            animator = objectsInOrder[nextIndex].GetComponent<Animator>();
        }
        PlaySound();
        EnableAnimator();
        DisableCollider(hit);
        if (nextIndex == 2)
        {
            yield return new WaitForSeconds(cameraAnimator.runtimeAnimatorController.animationClips[0].length + 1.0f);
        }
        else if (nextIndex == 3)
        {
            yield return new WaitForSeconds(4f);
            racingDisplay.SetActive(false);
            testingDisplay.SetActive(true);
            yield return new WaitForSeconds(3f);
        }
        else if (nextIndex == 4)
        {
            yield return new WaitForSeconds(7.5f);
            ChangeWheelText();
            yield return new WaitForSeconds(3f);
        }
        else if (nextIndex == 5)
        {
            yield return new WaitForSeconds(4f);
            ChangeWheelText();
            yield return new WaitForSeconds(3f);
        }
        else if (nextIndex == 6)
        {
            yield return new WaitForSeconds(4f);
            ChangeWheelText();
            yield return new WaitForSeconds(3f);
        }
        else if (nextIndex == 7)
        {
            yield return new WaitForSeconds(4f);
            ChangeWheelText();
            yield return new WaitForSeconds(3f);
        }
        else if (nextIndex == 8)
        {
            yield return new WaitForSeconds(12f);
            ChangeWheelText();
            yield return new WaitForSeconds(3f);
        }
        else if (nextIndex == 9)
        {
            yield return new WaitForSeconds(9.5f);
            ChangeWheelText();
            yield return new WaitForSeconds(3f);
        }
        else if (nextIndex == 10)
        {
            yield return new WaitForSeconds(9.5f);
            ChangeWheelText();
            yield return new WaitForSeconds(3f);
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
        EnableCollider();
        nextIndex++;
        helpText.text = "";
        CheckOnEnd();
        CameraController.rotationEnabled = !CameraController.rotationEnabled;
        cameraAnimator.enabled = false;
    }

    //check whether the current step was the last one
    void CheckOnEnd()
    {
        if (nextIndex == objectsInOrder.Length)
        {
            finalTips = helpCounter;
            finalErrors = errorCounter;
            helpCounter = 0;
            errorCounter = 0;
            nextIndex = 0;
            SceneManager.LoadScene("F1_Demonstrator_PostDemoScreen");
        }
        else
        {
            ChangeUIText();
        }
    }

    //play the sound of the current state
    void PlaySound()
    {
        if (nextIndex == 4)
        {
            audioSource.Play();
        }
        else if (nextIndex == 8)
        {
            audioSource.clip = slurp;
            audioSource.Play();
        }
    }

    //disable colliders of the buttons while animation is playing
    void DisableCollider(RaycastHit hit)
    {
        if (nextIndex == 0 || nextIndex == 1)
        {
            hit.collider.gameObject.GetComponent<Collider>().enabled = false;
        }
        foreach (Collider gbjCollider in f1.GetComponentsInChildren<Collider>())
        {
            gbjCollider.enabled = false;
        }
        helpButton.interactable = false;
    }

    //enable colliders after animation is finished
    void EnableCollider()
    {
        helpButton.interactable = true;
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
    }

    //enable the animator of the current state
    void EnableAnimator()
    {
        if (nextIndex != 0 && nextIndex != 10)
        {
            animator.SetBool("pressed", true);
        }
        cameraAnimator.enabled = true;
        cameraAnimator.SetBool(animatorStates[nextIndex], true);
        if (nextIndex == 9)
        {
            rearWingAnimator.SetBool("open", true);
        }
        else if (nextIndex == 10)
        {
            animator.SetBool("pressedAgain", true);
            rearWingAnimator.SetBool("open", false);
        }
    }

    //change the UIText
    void ChangeUIText()
    {
        if (nextIndex == 10)
        {
            uiText.text = "Step " + (nextIndex + 1) + "/" + (objectsInOrder.Length) + ": Press the DRS button again!";
        }
        else
        {
            uiText.text = "Step " + (nextIndex + 1) + "/" + (objectsInOrder.Length) + ": Press the " + objectsInOrder[nextIndex].name + "!";
        }
        uiTextSmall.text = infoTexts[nextIndex];
    }

    //change the text on the wheel display
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

    //show the help text when helpButton is clicked
    public void HelpButtonClicked()
    {
        helpButton.interactable = true;
        helpText.text = helpTexts[nextIndex];
        if (!helpButtonPressed[nextIndex])
        {
            helpCounter++;
            helpButtonPressed[nextIndex] = true;
            helpButton.interactable = false;
        }
    }

    //check whether a game object is selected (hover)
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

    //change the material/s of game object while hovering
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

    //change the material/s of the game object back to normal when leaving the collider
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

    //quit current mode and load menu scene
    public void QuitButtonClicked()
    {
        audioSource.clip = buttonClick;
        audioSource.Play();
        if (SceneManager.GetActiveScene().name == "F1_Demonstrator_TutorialMode")
        {
            helpCounter = 0;
            errorCounter = 0;
            nextIndex = 0;
        }
        SceneManager.LoadScene("F1_Demonstrator_EditedMenu");
    }
}