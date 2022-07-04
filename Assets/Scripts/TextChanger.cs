using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextChanger : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI uiText;
    [SerializeField] TextMeshProUGUI congratulationText;
    private int errorCounter = ButtonPresser.finalErrors;
    private int helpCounter = ButtonPresser.finalTips;

    void Start()
    {
        ChangeText();
    }

    void ChangeText()
    {
        congratulationText.text = "Congratulations! Your car is ready to take on the podium!\n You have made " + errorCounter + " mistakes and used " + helpCounter + " tips";
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
}
