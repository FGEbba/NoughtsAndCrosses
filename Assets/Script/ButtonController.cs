using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public string content = " ";
    public bool activated;
    public Button button;
    public Text text;

    public (int, int) gridNumber;

    public void Setup()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
    }

    public void ResetButton()
    {
        content = " ";
        text.text = content;
        activated = false;
        button.onClick.RemoveAllListeners();
        button.transition = Selectable.Transition.ColorTint;
    }


    public void OnClick(char toPrint)
    {
        content = toPrint.ToString();
        text.text = content;
        activated = true;
        button.onClick.RemoveAllListeners();
        button.transition = Selectable.Transition.None;
    }

}
