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
    Button helpButton;

    private LayerMask layerMask;

    public Material[] materialOriginals;
    public Material[] materialCopies;
    public Material[][] materialOriginalsList = new Material[3][];
    public GameObject[] carObjects;
    public int selectedObject;

    [SerializeField] AudioSource audioSource;
    [SerializeField] TextMeshProUGUI uiText;
    [SerializeField] TextMeshProUGUI helpText;
    [SerializeField] TextMeshProUGUI errorHelpText;
    [SerializeField] TextMeshProUGUI wheelText;
    [SerializeField] public GameObject[] objectsInOrder = null;

    public Animator animator;
    public Animator cameraAnimator;
    public Animator rearWingAnimator;

    [SerializeField] private Material highlightMaterial = null;
    private GameObject selectedGameObject;
    private Material selectedGameObjectMaterial;
    public GameObject SelectedGameObject => selectedGameObject;

    private int nextIndex = 0;
    private bool[] helpButtonPressed = { false, false, false, false, false, false, false, false, false, false, false };

    public static int helpCounter = 0;
    public static int errorCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        radioButton = GameObject.FindGameObjectWithTag("RadioButton");
        confirmButton = GameObject.FindGameObjectWithTag("ConfirmButton");
        pitLimiterButton = GameObject.FindGameObjectWithTag("PitLimiterButton");
        drinkButton = GameObject.FindGameObjectWithTag("DrinkButton");
        neutralButton = GameObject.FindGameObjectWithTag("NeutralButton");
        drsButton = GameObject.FindGameObjectWithTag("DRSButton");
        helpButton = GameObject.FindGameObjectWithTag("HelpButton").GetComponent<Button>();
        button1 = GameObject.FindGameObjectWithTag("+1Button");
        button10 = GameObject.FindGameObjectWithTag("-10Button");
        middleRotate = GameObject.FindGameObjectWithTag("MiddleRotate");
        rearWing = GameObject.FindGameObjectWithTag("RearWingPivot");
        helpButton.onClick.AddListener(HelpButtonClicked);
        car = GameObject.FindGameObjectWithTag("Car");

        animator = Camera.main.GetComponent<Animator>();
        cameraAnimator = Camera.main.GetComponent<Animator>();
        rearWingAnimator = rearWing.GetComponent<Animator>();

        layerMask = LayerMask.GetMask("Selectable");


    }

    // Update is called once per frame
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
                    };
                    animator.SetBool("pressed", true);
                    if (nextIndex == 9)
                    {
                        cameraAnimator.SetBool("drs", true);
                        rearWingAnimator.SetBool("open", true);
                    }
                    if (nextIndex == 10)
                    {
                        cameraAnimator.SetBool("drs", false);
                        rearWingAnimator.SetBool("open", false);
                    }
                    ChangeWheelText();
                    nextIndex++;
                    helpText.text = "";
                    if (nextIndex == objectsInOrder.Length)
                    {
                        SceneManager.LoadScene("F1_Demonstrator_PostDemoScreen");
                    }
                    else
                    {
                        if (nextIndex == 1)
                        {
                            uiText.text = "Step 2: Press the Steering Wheel!";
                        }
                        else if (nextIndex == 11)
                        {
                            uiText.text = "Step 12: Press the Middle Rotation Knob!";
                        }
                        else
                        {
                            uiText.text = "Step " + (nextIndex + 1) + ": Press the " + objectsInOrder[nextIndex].name + "!";
                        }
                        
                    }
                }
                else
                {
                    errorCounter++;
                    helpText.text = "Wrong Order! You have made " + errorCounter + " mistakes";
                }
            }
        }
    }

    void ChangeWheelText()
    {
        if (nextIndex == 2)
        {
            wheelText.text = "RADIO ON!";
        }
        else if (nextIndex == 3)
        {
            wheelText.text = "PIT CONFIRMED!";
        }
        else if (nextIndex == 4)
        {
            wheelText.text = "SPEED LIMITED!";
        }
        else if (nextIndex == 5)
        {
            wheelText.text = "SPEED LIMIT LIFTED!";
        }
        else if (nextIndex == 6)
        {
            wheelText.text = "HYDRATION ACTIVATED!";
        }
        else if (nextIndex == 7)
        {
            wheelText.text = "???";
        }
        else if (nextIndex == 8)
        {
            wheelText.text = "???";
        }
        else if (nextIndex == 9)
        {
            wheelText.text = "DRS ACTIVATED!";
        }
        else if (nextIndex == 10)
        {
            wheelText.text = "DRS DEACTIVATED";
        }
        else if (nextIndex == 11)
        {
            wheelText.text = "???";
        }
    }

    void HelpButtonClicked()
    {
        switch (nextIndex)
        {
            case 0:
                {
                    helpText.text = "It's right in front of you!";
                    if (!helpButtonPressed[0])
                    {
                        helpCounter++;
                        helpButtonPressed[0] = true;
                    }
                    break;
                }
            case 1:
                {
                    helpText.text = "It's on the car!";
                    if (!helpButtonPressed[1])
                    {
                        helpCounter++;
                        helpButtonPressed[1] = true;
                    }
                    break;
                }
            case 2:
                {
                    helpText.text = "It's just left from the main display!";
                    if (!helpButtonPressed[2])
                    {
                        helpCounter++;
                        helpButtonPressed[2] = true;
                    }
                    break;
                }
            case 3:
                {
                    helpText.text = "It's the furthest button to the left";
                    if (!helpButtonPressed[3])
                    {
                        helpCounter++;
                        helpButtonPressed[3] = true;
                    }
                    break;
                }
            case 4:
                {
                    helpText.text = "It's the big button in the upper right corner";
                    if (!helpButtonPressed[4])
                    {
                        helpCounter++;
                        helpButtonPressed[4] = true;
                    }
                    break;
                }
            case 5:
                {
                    helpText.text = "It's the big button in the upper left corner";
                    if (!helpButtonPressed[5])
                    {
                        helpCounter++;
                        helpButtonPressed[5] = true;
                    }
                    break;
                }
            case 6:
                {
                    helpText.text = "It's next to the bottom left corner of the display";
                    if (!helpButtonPressed[6])
                    {
                        helpCounter++;
                        helpButtonPressed[6] = true;
                    }
                    break;
                }
            case 7:
                {
                    helpText.text = "It's the one to the left of the Pit Limiter Button";
                    if (!helpButtonPressed[7])
                    {
                        helpCounter++;
                        helpButtonPressed[7] = true;
                    }
                    break;
                }
            case 8:
                {
                    helpText.text = "It's the one to the left of the +1-Button";
                    if (!helpButtonPressed[8])
                    {
                        helpCounter++;
                        helpButtonPressed[8] = true;
                    }
                    break;
                }
            case 9:
                {
                    helpText.text = "It's right from the rotation knob";
                    if (!helpButtonPressed[9])
                    {
                        helpCounter++;
                        helpButtonPressed[9] = true;
                    }
                    break;
                }
            case 10:
                {
                    helpText.text = "It's right from the rotation knob";
                    if (!helpButtonPressed[9])
                    {
                        helpCounter++;
                        helpButtonPressed[9] = true;
                    }
                    break;
                }
            case 11:
                {
                    helpText.text = "It's the big rotation knob under the display";
                    if (!helpButtonPressed[10])
                    {
                        helpCounter++;
                        helpButtonPressed[7] = true;
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
            foreach(GameObject gameObj in carObjects)
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
