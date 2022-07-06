using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InfoButtonColor : MonoBehaviour
{
    [SerializeField] Button infoButton;
    [SerializeField] TextMeshProUGUI infoButtonText;
    [SerializeField] TextMeshProUGUI uiTextSmall;
    public bool infoButtonOn = false;
    private ColorBlock buttonColor;
    public Color wantedColor;
    private Color originalColor;

    void Start()
    {
        uiTextSmall.enabled = false;
        buttonColor = infoButton.colors;
        originalColor = buttonColor.selectedColor;
    }

    //change the state and text of the button when clicked
    public void ChangeInfoState()
    {
        infoButtonOn = !infoButtonOn;
        uiTextSmall.enabled = !uiTextSmall.enabled;
        if (infoButtonOn)
        {
            infoButtonText.text = "Info On";
            infoButtonText.color = Color.green;
        }
        else
        {
            infoButtonText.text = "Info Off";
            infoButtonText.color = Color.red;
        }
    }

    //change buttonColor while hovering
    public void ChangeWhenHover()
    {
        buttonColor.selectedColor = wantedColor;
        infoButton.colors = buttonColor;
    }

    //change buttonColor back to normal when leaving
    public void ChangeWhenLeave()
    {
        buttonColor.selectedColor = originalColor;
        infoButton.colors = buttonColor;
    }
}
