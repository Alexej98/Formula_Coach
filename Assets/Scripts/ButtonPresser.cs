using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonPresser : MonoBehaviour
{
    GameObject radioButton;
    GameObject confirmButton;
    GameObject pitLimiterButton;
    GameObject drinkButton;
    GameObject drsButton;
    Button helpButton;

    private LayerMask layerMask;

    [SerializeField] TextMeshProUGUI uiText;
    [SerializeField] TextMeshProUGUI helpText;
    [SerializeField] TextMeshProUGUI errorHelpText;
    [SerializeField] private GameObject[] objectsInOrder = null;
 
    private int nextIndex = 0;
    private int helpCounter = 0;
    private bool[] helpButtonPressed = { false, false, false, false, false };

    public int errorCounter = 0;

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

        layerMask = LayerMask.GetMask("Selectable");
    }

    // Update is called once per frame
    void Update()
    {
        errorHelpText.text = errorCounter + "\n" + helpCounter;
        if (Input.GetMouseButtonDown(0) && nextIndex >= 0 && nextIndex < objectsInOrder.Length)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, layerMask))
            {
                if (hit.collider.gameObject.Equals(objectsInOrder[nextIndex]))
                {
                    nextIndex++;
                    Debug.Log("Nr. " + nextIndex + " " + hit.transform.gameObject);
                    helpText.text = "";
                    if (nextIndex == objectsInOrder.Length)
                    {
                        uiText.text = "Congratulations! You did it! You have made " + errorCounter + " mistakes and used " + helpCounter + " tips";
                        if (errorCounter == 0)
                        {
                            uiText.text = "You are a true professional. See you next season!";
                        }
                        else if (errorCounter > 0 && errorCounter <= 2)
                        {
                            uiText.text = "That's magnificent. Max Verstappen would be proud of you!";
                        }
                        else if (errorCounter > 2 && errorCounter <= 5)
                        {
                            uiText.text = "Not bad for a rookie. I see your potential";
                        }
                        else if (errorCounter > 5 && errorCounter <= 7)
                        {
                            uiText.text = "Keep practicing. You've got a lot to learn";
                        }
                        else
                        {
                            uiText.text = "Get out of my simulator, you piece of Mazepin";
                        }
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
                    helpText.text = "It's just left from the main display!";
                    if (!helpButtonPressed[0])
                    {
                        helpCounter++;
                        helpButtonPressed[0] = true;
                    }
                    break;
                }
            case 1:
                {
                    helpText.text = "It's the furthest button to the left";
                    if (!helpButtonPressed[1])
                    {
                        helpCounter++;
                        helpButtonPressed[1] = true;
                    }
                    break;
                }
            case 2:
                {
                    helpText.text = "It's the big button in the upper left corner";
                    if (!helpButtonPressed[2])
                    {
                        helpCounter++;
                        helpButtonPressed[2] = true;
                    }
                    break;
                }
            case 3:
                {
                    helpText.text = "It's next to the bottom right button";
                    if (!helpButtonPressed[3])
                    {
                        helpCounter++;
                        helpButtonPressed[3] = true;
                    }
                    break;
                }
            case 4:
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
}
