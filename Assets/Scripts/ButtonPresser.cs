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
    GameObject drsButton;
    GameObject car;
    Button helpButton;

    private LayerMask layerMask;
    
    [SerializeField] AudioSource audioSource;
    [SerializeField] TextMeshProUGUI uiText;
    [SerializeField] TextMeshProUGUI helpText;
    [SerializeField] TextMeshProUGUI errorHelpText;
    [SerializeField] private GameObject[] objectsInOrder = null;
    public Animator animator;

    [SerializeField] private Material highlightMaterial = null;
    private GameObject selectedGameObject;
    private Material selectedGameObjectMaterial;
    public GameObject SelectedGameObject => selectedGameObject;

    private int nextIndex = 0;
    private bool[] helpButtonPressed = { false, false, false, false, false };

    public static int helpCounter = 0;
    public static int errorCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        radioButton = GameObject.FindGameObjectWithTag("RadioButton");
        confirmButton = GameObject.FindGameObjectWithTag("ConfirmButton");
        pitLimiterButton = GameObject.FindGameObjectWithTag("PitLimiterButton");
        drinkButton = GameObject.FindGameObjectWithTag("DrinkButton");
        drsButton = GameObject.FindGameObjectWithTag("DRSButton");
        helpButton = GameObject.FindGameObjectWithTag("HelpButton").GetComponent<Button>();
        helpButton.onClick.AddListener(HelpButtonClicked);
        car = GameObject.FindGameObjectWithTag("Car");

        animator = Camera.main.GetComponent<Animator>();

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
                Debug.Log(hit.collider.gameObject + ", " + objectsInOrder[nextIndex]);
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
                    nextIndex++;
                    Debug.Log("Nr. " + nextIndex + " " + hit.transform.gameObject);
                    helpText.text = "";
                    if (nextIndex == objectsInOrder.Length)
                    {
                        SceneManager.LoadScene("F1_Demonstrator_PostDemoScreen");
                    }
                    else
                    {
                        uiText.text = "Step " + (nextIndex + 1) + ": Press the " + objectsInOrder[nextIndex].name;
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
                    if (!helpButtonPressed[0])
                    {
                        helpCounter++;
                        helpButtonPressed[0] = true;
                    }
                    break;
                }
            case 2:
                {
                    helpText.text = "It's just left from the main display!";
                    if (!helpButtonPressed[0])
                    {
                        helpCounter++;
                        helpButtonPressed[0] = true;
                    }
                    break;
                }
            case 3:
                {
                    helpText.text = "It's the furthest button to the left";
                    if (!helpButtonPressed[1])
                    {
                        helpCounter++;
                        helpButtonPressed[1] = true;
                    }
                    break;
                }
            case 4:
                {
                    helpText.text = "It's the big button in the upper left corner";
                    if (!helpButtonPressed[2])
                    {
                        helpCounter++;
                        helpButtonPressed[2] = true;
                    }
                    break;
                }
            case 5:
                {
                    helpText.text = "It's next to the bottom right button";
                    if (!helpButtonPressed[3])
                    {
                        helpCounter++;
                        helpButtonPressed[3] = true;
                    }
                    break;
                }
            case 6:
                {
                    helpText.text = "It's right from the rotation knob";
                    if (!helpButtonPressed[4])
                    {
                        helpCounter++;
                        helpButtonPressed[4] = true;
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
        renderer.material = highlightMaterial;
    }

    private void DeSelect()
    {
        selectedGameObject.GetComponent<Renderer>().material = selectedGameObjectMaterial;
        selectedGameObject = null;
    }
    bool AnimatorIsPlaying()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}
