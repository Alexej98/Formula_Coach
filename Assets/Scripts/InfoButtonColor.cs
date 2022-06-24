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

    // Start is called before the first frame update
    void Start()
    {
        buttonColor = infoButton.colors;
        originalColor = buttonColor.selectedColor;
    }
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

    public void ChangeWhenHover()
    {
        buttonColor.selectedColor = wantedColor;
        infoButton.colors = buttonColor;
    }

    public void ChangeWhenLeave()
    {
        buttonColor.selectedColor = originalColor;
        infoButton.colors = buttonColor;
    }
}
